using Camel.Bancho.Enums;
using Camel.Bancho.Packets;
using Camel.Bancho.Packets.v1;
using Camel.Bancho.Packets.v1.Server;
using Microsoft.AspNetCore.Connections;

namespace Camel.Bancho;

public class BanchoTcpConnectionHandler : ConnectionHandler
{
    private readonly ILogger<BanchoTcpConnectionHandler> _logger;

    public BanchoTcpConnectionHandler(ILogger<BanchoTcpConnectionHandler> logger)
    {
        _logger = logger;
    }
    
    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        var sr = new StreamReader(connection.Transport.Input.AsStream());
        var userName = await sr.ReadLineAsync();
        var password = await sr.ReadLineAsync();
        var other = await sr.ReadLineAsync();

        var packetQueue = new PacketQueue();
        packetQueue.WriteUserId(13);
        var statsv1 = new UserStatsPacket(
            13, "allein", 12345, 99.34f / 100f, 13, 12345, 1, "", ClientAction.Unknown, "", "", 0, 3, "Ukraine");
        packetQueue.WritePacket(statsv1);

        while (!connection.ConnectionClosed.IsCancellationRequested)
        {
            // var input = await connection.Transport.Input.ReadAsync();
            // var inMs = new MemoryStream(input.Buffer.ToArray());
            // var inStream = new LegacyPacketStream(inMs);
            //
            // foreach (var packet in inStream.ReadAll())
            // {
            //     Console.WriteLine(packet.Type);
            // }

            var outMs = new MemoryStream();
            var outStream = new LegacyPacketStream(outMs);
            
            foreach (var packet in packetQueue.PendingPackets())
            {
                packet.WriteToStream(outStream);
            }

            if (outMs.Length > 0)
            {
                await connection.Transport.Output.WriteAsync(outMs.ToArray());
            }
            
            await Task.Delay(100);
        }
    }
}