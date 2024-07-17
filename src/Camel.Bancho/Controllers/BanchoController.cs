using Camel.Bancho.Dtos;
using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets;
using Camel.Bancho.Services;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Data;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Camel.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

[Host("c.camel.local", "ce.camel.local", "c4.camel.local")]
public class BanchoController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IPacketHandlerService _packetHandler;
    private readonly IUserSessionService _userSessionService;
    private readonly IStatsService _statsService;
    private readonly ILogger<BanchoController> _logger;

    public BanchoController(
        IAuthService authService,
        IPacketHandlerService packetHandler, 
        IUserSessionService userSessionService,
        IStatsService statsService,
        ILogger<BanchoController> logger)
    {
        _authService = authService;
        _packetHandler = packetHandler;
        _userSessionService = userSessionService;
        _statsService = statsService;
        _logger = logger;
    }

    [HttpPost("/")]
    public async Task<IActionResult> Index([FromHeader(Name = "osu-token")] string? accessToken)
    {
        var inStream = new MemoryStream();
        await Request.BodyReader.CopyToAsync(inStream);
        inStream.Position = 0;

        if (accessToken == null)
        {
            try
            {
                var request = LoginRequest.FromBytes(inStream.ToArray());
                return await HandleLoginRequestAsync(request);
            }
            catch
            {
                return BadRequest();
            }
        }

        var session = _userSessionService.GetSession(accessToken);
        if (session == null)
        {
            var pq = new PacketQueue();
            pq.WriteRestart(0);
            return SendPendingPackets(pq);
        }

        session.LastActive = DateTime.Now;

        if (inStream.Length <= 0) return SendPendingPackets(session.PacketQueue);

        var inPacketStream = new PacketStream(inStream);
        foreach (var p in inPacketStream.ReadAll())
        {
            if (p.Type != PacketType.ClientPing && p.Type != PacketType.ClientUserStatsRequest)
                _logger.LogTrace("{} -> {}", session.Username, p.Type);

            await _packetHandler.HandleAsync(p.Type, new MemoryStream(p.Data), session);
        }

        return SendPendingPackets(session.PacketQueue);
    }

    private async Task<FileContentResult> HandleLoginRequestAsync(LoginRequest request)
    {
        var pq = new PacketQueue();

        var (user, authResult) = _authService.AuthenticateUser(request.Username, request.PasswordMd5);
        if (user == null || authResult != PasswordVerificationResult.Success)
        {
            pq.WriteUserId(-1);
            Response.Headers["cho-token"] = "";
            return SendPendingPackets(pq);
        }

        pq.WriteProtocolVersion(19);
        pq.WriteUserId(user.Id);
        pq.WritePrivileges(0);
        pq.WriteNotification("Welcome to OSU camel");
        pq.WriteChannelInfo("#osu", "General channel", 1);
        pq.WriteChannelInfoEnd();

        pq.WriteUserPresence(user.Id, user.UserName, 0, 222, 0, 0, 0, 1);
        
        var stats = await _statsService.GetUserStatsAsync(user.Id, GameMode.Standard);
        pq.WriteUserStats(user.Id, ClientAction.Idle, "", "", 0, stats.Mode, 0,
            stats.RankedScore, stats.Accuracy / 100.0f, stats.Plays, stats.TotalScore, 12, stats.Pp);

        pq.WriteSendMessage("Camel", "Welcome to camel bro", user.UserName, 3);

        var newToken = Guid.NewGuid().ToString();
        var newSession = new UserSession(request, user, stats, pq);
        _userSessionService.AddSession(newToken, newSession);

        Response.Headers["cho-token"] = newToken;
        return SendPendingPackets(pq);
    }

    private FileContentResult SendPendingPackets(PacketQueue packetQueue)
    {
        using var ms = new MemoryStream();
        using var ps = new PacketStream(ms);

        foreach (var packet in packetQueue.PendingPackets())
        {
            packet.WriteToStream(ps);
        }

        return File(ms.ToArray(), "application/octet-stream");
    }

    [HttpGet("web/bancho_connect.php")]
    public IActionResult BanchoConnect() => Ok();
}