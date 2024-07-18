using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientUserStatsRequest)]
public class UserStatsRequestHandler : IPacketHandler<UserStatsRequestPacket>
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

    public async Task HandleAsync(UserStatsRequestPacket packet, UserSession userSession)
    {
        // TODO: find out whether the client is really supposed to spam this billion times a second
        var requestedUsers = _userSessionService.GetOnlineUsers()
            .IntersectBy(packet.UserIds, u => u.User.Id);

        foreach (var user in requestedUsers)
        {
            userSession.PacketQueue.WriteUserStats(user, _rankingService.GetUserGlobalRank(user.User.Id));
        }
    }
}