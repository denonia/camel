﻿using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets;
using Camel.Bancho.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

public class BanchoController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly PacketHandlerService _packetHandler;
    private readonly UserSessionService _userSessionService;
    private readonly ILogger<BanchoController> _logger;

    public BanchoController(
        AuthService authService,
        PacketHandlerService packetHandler, UserSessionService userSessionService,
        ILogger<BanchoController> logger)
    {
        _authService = authService;
        _packetHandler = packetHandler;
        _userSessionService = userSessionService;
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
            var request = LoginRequest.FromBytes(inStream.ToArray());
            return HandleLoginRequest(request);
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
            if (p.Type != PacketType.ClientPing)
                _logger.LogTrace("{} -> {}", session.Username, p.Type);

            _packetHandler.Handle(p.Type, new MemoryStream(p.Data), session);
        }

        return SendPendingPackets(session.PacketQueue);
    }

    private FileContentResult HandleLoginRequest(LoginRequest request)
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

        pq.WriteUserPresence(1, user.UserName, 0, 0, 0, 0, 0, 1);
        pq.WriteUserStats(1, ClientAction.Editing, "Darude - Sandstorm", "", 0, GameMode.Standard, 1,
            long.MaxValue / 2, 0.993f, 10498, long.MaxValue / 2, 12, 0);

        pq.WriteSendMessage("Camel", "Welcome to camel bro", user.UserName, 2);

        var newToken = Guid.NewGuid().ToString();
        var newSession = new UserSession(user.UserName, pq);
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