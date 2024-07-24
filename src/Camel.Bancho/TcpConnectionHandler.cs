using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets;
using Camel.Bancho.Services.Interfaces;
using Microsoft.AspNetCore.Connections;

namespace Camel.Bancho;

public class TcpConnectionHandler : ConnectionHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserSessionService _userSessionService;
    private readonly IPacketHandlerService _packetHandler;
    private readonly ILogger<TcpConnectionHandler> _logger;

    public TcpConnectionHandler(IServiceProvider serviceProvider, IUserSessionService userSessionService,
        IPacketHandlerService packetHandler,
        ILogger<TcpConnectionHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _userSessionService = userSessionService;
        _packetHandler = packetHandler;
        _logger = logger;
    }

    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        var scope = _serviceProvider.CreateScope();
        var banchoService = scope.ServiceProvider.GetRequiredService<IBanchoService>();

        var readResult = await connection.Transport.Input.ReadAsync();
        
        var pq = new PacketQueue();

        var token = await banchoService.HandleLoginRequestAsync(pq, readResult.Buffer.ToArray());
        await SendPendingPacketsAsync(pq, connection.Transport.Output);
        
        connection.Transport.Input.AdvanceTo(readResult.Buffer.End);

        if (string.IsNullOrEmpty(token))
            return;

        var session = _userSessionService.GetSession(token);

        while (!connection.ConnectionClosed.IsCancellationRequested)
        {
            await UpdateAsync(connection.Transport, session);
            await Task.Delay(500);
        }
    }

    private async Task UpdateAsync(IDuplexPipe transport, UserSession session)
    {
        if (transport.Input.TryRead(out var readResult))
        {
            var inStream = new MemoryStream(readResult.Buffer.ToArray());
            
            var reader = new PacketBinaryReader(inStream, Encoding.Default, true);
            while (inStream.Position < inStream.Length)
            {
                var p = reader.ReadPacket();
                if (p.Type != PacketType.ClientPing)
                    _logger.LogDebug("{} -> {}", session.Username, p.Type);

                await _packetHandler.HandleAsync(p.Type, new MemoryStream(p.Data), session);
            }
            
            transport.Input.AdvanceTo(readResult.Buffer.End);
        }

        if (session.PacketQueue.Any())
            await SendPendingPacketsAsync(session.PacketQueue, transport.Output);
    }

    private async Task SendPendingPacketsAsync(PacketQueue packetQueue, PipeWriter output)
    {
        var ms = new MemoryStream();
        var ps = new PacketBinaryWriter(ms);

        foreach (var packet in packetQueue.PendingPackets())
        {
            ps.Write(packet);
        }

        ms.Position = 0;
        await output.WriteAsync(ms.ToArray());
    }
}