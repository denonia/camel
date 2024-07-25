using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientUserStatsRequest)]
public class UserStatsRequestHandler : IPacketHandler<int[]>
{
    private readonly IUserSessionService _userSessionService;
    private readonly IRankingService _rankingService;
    private readonly ILogger<UserStatsRequestHandler> _logger;

    public UserStatsRequestHandler(IUserSessionService userSessionService, IRankingService rankingService,
        ILogger<UserStatsRequestHandler> logger)
    {
        _userSessionService = userSessionService;
        _rankingService = rankingService;
        _logger = logger;
    }

    public async Task HandleAsync(int[] userIds, UserSession userSession)
    {
        var requestedUsers = _userSessionService.GetOnlineUsers()
            .Where(u => u.User.Id != userSession.User.Id)
            .IntersectBy(userIds, u => u.User.Id);

        foreach (var user in requestedUsers)
        {
            var rank = await _rankingService.GetGlobalRankPpAsync(user.User.Id, user.Status.Mode);
            userSession.PacketQueue.WriteUserStats(user, rank);
        }
    }
}