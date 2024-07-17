using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientChangeAction)]
public class ChangeActionHandler : IPacketHandler<ChangeActionPacket>
{
    private readonly IUserSessionService _userSessionService;

    public ChangeActionHandler(IUserSessionService userSessionService)
    {
        _userSessionService = userSessionService;
    }
    
    public async Task HandleAsync(ChangeActionPacket packet, UserSession userSession)
    {
        // TODO: mapId?
        userSession.Status = new UserStatus(packet.Action, packet.InfoText, packet.MapMd5, packet.Mods, packet.Mode, 0);
    }
}