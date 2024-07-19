using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientChangeAction)]
public class ChangeActionHandler : IPacketHandler<ChangeActionPacket>
{
    private readonly IUserSessionService _userSessionService;
    private readonly IRankingService _rankingService;

    public ChangeActionHandler(IUserSessionService userSessionService, IRankingService rankingService)
    {
        _userSessionService = userSessionService;
        _rankingService = rankingService;
    }
    
    public async Task HandleAsync(ChangeActionPacket packet, UserSession userSession)
    {
        // TODO: mapId?
        userSession.Status = new UserStatus(packet.Action, packet.InfoText, packet.MapMd5, packet.Mods, packet.Mode, 0);

        foreach (var user in _userSessionService.GetOnlineUsers())
        {
            var rank = await _rankingService.GetGlobalRankPpAsync(userSession.User.Id, userSession.Status.Mode);
            user.PacketQueue.WriteUserStats(userSession, rank);
        }
    }
}