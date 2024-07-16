using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets;
using Camel.Bancho.Packets.Server;
using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

public class BanchoController : ControllerBase
{
    private readonly PacketHandlerService _packetHandler;
    private readonly ILogger<BanchoController> _logger;

    public BanchoController(PacketHandlerService packetHandler, ILogger<BanchoController> logger)
    {
        _packetHandler = packetHandler;
        _logger = logger;
    }
    
    [HttpPost("/")]
    public async Task<IActionResult> Index([FromHeader(Name = "osu-token")] string? accessToken)
    {
        using var ms = new MemoryStream();
        using var ps = new PacketStream(ms);
        var pw = new PacketWriter(new PacketStream(ms));
        var ctx = new UserContext("player", pw);
        
        if (accessToken == null)
        {
            pw.WriteProtocolVersion(19);
            pw.WriteUserId(1);
            pw.WritePrivileges(0);
            pw.WriteNotification("Welcome to OSU camel");
            pw.WriteChannelInfo("#osu", "General channel", 1);
            pw.WriteChannelInfoEnd();

            var presence = new UserPresencePacket(1, "player", 0, 0, 0, 0, 0, 1);
            presence.WriteToStream(ps);

            var stats = new UserStatsPacket(1, 1, "Busy", "", 0, GameMode.Standard, 1, 1234, 1, 1, 1, 1, 1);
            stats.WriteToStream(ps);

            var message = new SendMessagePacket("Camel", "Welcome to camel bro", "player", 2);
            message.WriteToStream(ps);
            
            Response.Headers["cho-token"] = "pog";
            return File(ms.ToArray(), "application/octet-stream");
        }

        var inStream = new MemoryStream();
        await Request.BodyReader.CopyToAsync(inStream);
        inStream.Position = 0;
        
        if (inStream.Length > 0)
        {
            var streamIn = new PacketStream(inStream);
            while (!streamIn.AtEnd)
            {
                var p = streamIn.Read();
                
                if (p.Type != PacketType.ClientPing)
                    _logger.LogDebug("Got a packet, type: {}", p.Type);

                var packetMs = new MemoryStream(p.Data);
                _packetHandler.Handle(p.Type, packetMs, ctx);
            }
        }

        return File(ms.ToArray(), "application/octet-stream");
    }

    [HttpGet("web/bancho_connect.php")]
    public IActionResult BanchoConnect() => Ok();
}