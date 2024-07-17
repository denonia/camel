using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientUserStatsRequest)]
public class UserStatsRequestHandler : IPacketHandler<UserStatsRequestPacket>
{
    private readonly IUserSessionService _userSessionService;
    private readonly ILogger<UserStatsRequestHandler> _logger;

    public UserStatsRequestHandler(IUserSessionService userSessionService, ILogger<UserStatsRequestHandler> logger)
    {
        _userSessionService = userSessionService;
        _logger = logger;
    }

    public async Task HandleAsync(UserStatsRequestPacket packet, UserSession userSession)
    {
        // TODO: find out whether the client is really supposed to spam this billion times a second
        foreach (var userId in packet.UserIds)
        {
            var requestedUser = _userSessionService.GetOnlineUsers().SingleOrDefault(u => u.User.Id == userId);
            if (requestedUser == null)
            {
                _logger.LogWarning("{} requested for non-existing user: {}", userSession.Username, userId);
                return;
            }

            userSession.PacketQueue.WriteUserStats(userId, 
                requestedUser.Status.Action, requestedUser.Status.InfoText, requestedUser.Status.MapMd5, requestedUser.Status.Mods,
                requestedUser.Stats.Mode, requestedUser.Status.MapId,
                requestedUser.Stats.RankedScore, requestedUser.Stats.Accuracy / 100.0f, requestedUser.Stats.Plays,
                requestedUser.Stats.TotalScore, 12, requestedUser.Stats.Pp);
        }
    }
}