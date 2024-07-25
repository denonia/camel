using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Chat;

[PacketHandler(PacketType.ClientMatchInvite)]
public class MatchInviteHandler : IPacketHandler<int>
{
    private readonly IUserSessionService _userSessionService;
    private readonly IMultiplayerService _multiplayerService;

    public MatchInviteHandler(IUserSessionService userSessionService, IMultiplayerService multiplayerService)
    {
        _userSessionService = userSessionService;
        _multiplayerService = multiplayerService;
    }

    public async Task HandleAsync(int targetId, UserSession userSession)
    {
        var target = _userSessionService.GetOnlineUsers().SingleOrDefault(u => u.User.Id == targetId);
        var match = _multiplayerService.ActiveMatch(userSession);

        if (target is not null && match is not null)
        {
            target.PacketQueue.WriteMatchInvite(userSession, target, match);
        }
    }
}