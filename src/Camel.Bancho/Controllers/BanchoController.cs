using System.Text;
using Camel.Bancho.Enums;
using Camel.Bancho.Packets;
using Camel.Bancho.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

[Host("c.ppy.sh", "ce.ppy.sh", "c4.ppy.sh", "c.camel.local", "ce.camel.local", "c4.camel.local")]
public class BanchoController : ControllerBase
{
    private readonly IBanchoService _banchoService;
    private readonly IPacketHandlerService _packetHandler;
    private readonly IUserSessionService _userSessionService;
    private readonly ILogger<BanchoController> _logger;

    public BanchoController(
        IBanchoService banchoService,
        IPacketHandlerService packetHandler,
        IUserSessionService userSessionService,
        ILogger<BanchoController> logger)
    {
        _banchoService = banchoService;
        _packetHandler = packetHandler;
        _userSessionService = userSessionService;
        _logger = logger;
    }

    [HttpPost("/")]
    public async Task<IActionResult> Index([FromHeader(Name = "osu-token")] string? accessToken)
    {
        using var inStream = new MemoryStream();
        await Request.BodyReader.CopyToAsync(inStream);
        inStream.Position = 0;

        if (accessToken == null)
            return await HandleLoginRequestAsync(inStream, Request.HttpContext.Connection.RemoteIpAddress.ToString());

        var session = _userSessionService.GetSession(accessToken);
        if (session == null)
        {
            var pq = new PacketQueue();
            pq.WriteRestart(0);
            return SendPendingPackets(pq);
        }

        session.LastActive = DateTime.Now;

        if (inStream.Length <= 0) return SendPendingPackets(session.PacketQueue);

        var reader = new PacketBinaryReader(inStream, Encoding.Default, true);
        while (inStream.Position < inStream.Length)
        {
            var p = reader.ReadPacket();
            if (p.Type != PacketType.ClientPing)
                _logger.LogDebug("{} -> {}", session.Username, p.Type);

            await _packetHandler.HandleAsync(p.Type, new MemoryStream(p.Data), session);
        }

        return SendPendingPackets(session.PacketQueue);
    }

    private async Task<IActionResult> HandleLoginRequestAsync(MemoryStream stream, string ipAddress)
    {
        try
        {
            var pq = new PacketQueue();
            var token = await _banchoService.HandleLoginRequestAsync(pq, stream.ToArray(), ipAddress);
            
            Response.Headers["cho-token"] = token ?? "";
            return SendPendingPackets(pq);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to handle login request: {}", e.ToString());
            return BadRequest();
        }
    }

    private FileStreamResult SendPendingPackets(PacketQueue packetQueue)
    {
        var ms = new MemoryStream();
        var ps = new PacketBinaryWriter(ms);

        foreach (var packet in packetQueue.PendingPackets())
        {
            ps.Write(packet);
        }

        ms.Position = 0;
        return File(ms, "application/octet-stream", "");
    }
}