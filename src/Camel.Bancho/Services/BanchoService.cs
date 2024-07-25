using Camel.Bancho.Dtos;
using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Camel.Bancho.Services;

public class BanchoService : IBanchoService
{
    private readonly IUserSessionService _userSessionService;
    private readonly IAuthService _authService;
    private readonly IChatService _chatService;
    private readonly IStatsService _statsService;
    private readonly IRankingService _rankingService;
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<BanchoService> _logger;

    public BanchoService(IUserSessionService userSessionService, IAuthService authService, IChatService chatService,
        IStatsService statsService, IRankingService rankingService, ApplicationDbContext dbContext,
        IConfiguration configuration, ILogger<BanchoService> logger)
    {
        _userSessionService = userSessionService;
        _authService = authService;
        _chatService = chatService;
        _statsService = statsService;
        _rankingService = rankingService;
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string?> HandleLoginRequestAsync(PacketQueue pq, byte[] requestBytes, string ipAddress)
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
        var location = await FetchLocationAsync(user, ipAddress);

        var newToken = Guid.NewGuid().ToString();
        var newSession = new UserSession(request, user, location, stats, pq);
        _userSessionService.AddSession(newToken, newSession);

        pq.WriteUserPresence(newSession, rank);
        pq.WriteUserStats(newSession, rank);

        await InsertSessionToDbAsync(request, user.Id, ipAddress);

        foreach (var otherSession in _userSessionService.GetOnlineUsers().Where(u => u != newSession))
        {
            otherSession.PacketQueue.WriteUserPresence(newSession, rank);

            var otherRank = await _rankingService.GetGlobalRankPpAsync(otherSession.User.Id, otherSession.Status.Mode);
            pq.WriteUserPresence(otherSession, otherRank);
        }

        return newToken;
    }

    private async Task<Location> FetchLocationAsync(User user, string ipAddress)
    {
        var path = Path.Combine(_configuration.GetRequiredSection("DATA_PATH").Value!, "GeoLite2-City.mmdb");

        using var reader = new DatabaseReader(path);
        try
        {
            var result = reader.City(ipAddress);
            
            if (user.Country == "XX")
            {
                _dbContext.Attach(user);
                user.Country = result.Country.IsoCode ?? "XX";
                await _dbContext.SaveChangesAsync();
            }

            return new Location(ipAddress,
                result.Location.Latitude ?? 0, result.Location.Longitude ?? 0,
                result.Country.IsoCode ?? "XX",
                result.Country.Name ?? "Unknown",
                result.City.Name ?? "Unknown");
        }
        catch (AddressNotFoundException)
        {
            _logger.LogWarning("Failed to locate {} ({})", user.UserName, ipAddress);
        }

        return new Location(ipAddress, 0, 0, "XX", "Unknown", "Unknown");
    }

    private async Task InsertSessionToDbAsync(LoginRequest loginRequest, int userId, string ipAddress)
    {
        var session = new LoginSession(userId,
            loginRequest.OsuVersion,
            loginRequest.ClientHashes?.RunningUnderWine ?? false,
            loginRequest.ClientHashes?.OsuPathMd5,
            loginRequest.ClientHashes?.AdaptersStr,
            loginRequest.ClientHashes?.AdaptersMd5,
            loginRequest.ClientHashes?.UninstallMd5,
            loginRequest.ClientHashes?.DiskSignatureMd5,
            ipAddress);
        _dbContext.LoginSessions.Add(session);
        await _dbContext.SaveChangesAsync();
    }
}