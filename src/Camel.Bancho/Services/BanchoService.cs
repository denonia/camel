using Camel.Bancho.Dtos;
using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Camel.Bancho.Services;

public class BanchoService : IBanchoService
{
    private readonly IUserSessionService _userSessionService;
    private readonly IAuthService _authService;
    private readonly IChatService _chatService;
    private readonly IStatsService _statsService;
    private readonly IRankingService _rankingService;
    private readonly ILogger<BanchoService> _logger;

    public BanchoService(IUserSessionService userSessionService, IAuthService authService, IChatService chatService,
        IStatsService statsService, IRankingService rankingService, ILogger<BanchoService> logger)
    {
        _userSessionService = userSessionService;
        _authService = authService;
        _chatService = chatService;
        _statsService = statsService;
        _rankingService = rankingService;
        _logger = logger;
    }

    public async Task<string?> HandleLoginRequestAsync(PacketQueue pq, byte[] requestBytes)
    {
        var request = LoginRequest.FromBytes(requestBytes);

        var (user, authResult) = _authService.AuthenticateUser(request.Username, request.PasswordMd5);
        if (user == null || authResult != PasswordVerificationResult.Success)
        {
            pq.WriteUserId(-1);
            return null;
        }

        pq.WriteProtocolVersion(19);
        pq.WriteUserId(user.Id);
        pq.WritePrivileges(Privileges.Supporter);

        foreach (var channel in _chatService.AutoJoinChannels())
        {
            pq.WriteChannelInfo(channel.Name, channel.Topic, channel.ParticipantCount);
        }

        pq.WriteChannelInfoEnd();

        var stats = await _statsService.GetUserStatsAsync(user.Id);
        var rank = await _rankingService.GetGlobalRankPpAsync(user.Id, GameMode.Standard);

        var newToken = Guid.NewGuid().ToString();
        var newSession = new UserSession(request, user, stats, pq);
        _userSessionService.AddSession(newToken, newSession);

        pq.WriteUserPresence(newSession, rank);
        pq.WriteUserStats(newSession, rank);

        _logger.LogInformation($"{user.UserName} (ID: {user.Id}) has logged in from {request.OsuVersion}");

        foreach (var otherSession in _userSessionService.GetOnlineUsers().Where(u => u != newSession))
        {
            otherSession.PacketQueue.WriteUserPresence(newSession, rank);

            var otherRank = await _rankingService.GetGlobalRankPpAsync(otherSession.User.Id, otherSession.Status.Mode);
            pq.WriteUserPresence(otherSession, otherRank);
        }

        return newToken;
    }
}