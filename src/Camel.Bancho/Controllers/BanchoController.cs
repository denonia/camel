using System.Text;
using Camel.Bancho.Dtos;
using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
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
    private readonly IRankingService _rankingService;
    private readonly ILogger<BanchoController> _logger;

    public BanchoController(
        IAuthService authService,
        IPacketHandlerService packetHandler,
        IUserSessionService userSessionService,
        IStatsService statsService,
        IRankingService rankingService,
        ILogger<BanchoController> logger)
    {
        _authService = authService;
        _packetHandler = packetHandler;
        _userSessionService = userSessionService;
        _statsService = statsService;
        _rankingService = rankingService;
        _logger = logger;
    }

    [HttpPost("/")]
    public async Task<IActionResult> Index([FromHeader(Name = "osu-token")] string? accessToken)
    {
        using var inStream = new MemoryStream();
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

        var reader = new PacketBinaryReader(inStream, Encoding.Default, true);
        while (inStream.Position < inStream.Length)
        {
            var p = reader.ReadPacket();
            if (p.Type != PacketType.ClientPing && p.Type != PacketType.ClientUserStatsRequest)
                _logger.LogTrace("{} -> {}", session.Username, p.Type);

            await _packetHandler.HandleAsync(p.Type, new MemoryStream(p.Data), session);
        }

        return SendPendingPackets(session.PacketQueue);
    }

    private async Task<FileStreamResult> HandleLoginRequestAsync(LoginRequest request)
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
        pq.WritePrivileges(Privileges.Supporter);
        pq.WriteNotification("Welcome to OSU camel");
        pq.WriteChannelInfo("#osu", "General channel", 1);
        pq.WriteChannelInfoEnd();

        var newToken = Guid.NewGuid().ToString();
        var newSession = new UserSession(request, user, pq);
        _userSessionService.AddSession(newToken, newSession);

        var stats = await _statsService.GetUserStatsAsync(user.Id, GameMode.Standard);
        var rank = await _rankingService.GetGlobalRankPpAsync(user.Id, stats.Mode);
        pq.WriteUserPresence(newSession, rank);
        pq.WriteUserStats(newSession, rank);

        pq.WriteSendMessage("Camel", "Welcome to camel bro", user.UserName, 3);

        foreach (var otherSession in _userSessionService.GetOnlineUsers().Where(u => u != newSession))
        {
            otherSession.PacketQueue.WriteUserPresence(newSession, rank);

            var otherRank = await _rankingService.GetGlobalRankPpAsync(otherSession.User.Id, otherSession.Status.Mode);
            pq.WriteUserPresence(otherSession, otherRank);
        }

        Response.Headers["cho-token"] = newToken;
        return SendPendingPackets(pq);
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

    [HttpGet("web/bancho_connect.php")]
    public IActionResult BanchoConnect() => Ok();
}