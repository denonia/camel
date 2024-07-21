using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientPartMatch)]
public class PartMatchHandler : IPacketHandler<PartMatchPacket>
{
    private readonly IMultiplayerService _multiplayerService;

    public PartMatchHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public async Task HandleAsync(PartMatchPacket packet, UserSession userSession)
    {
        _multiplayerService.LeaveMatch(userSession);
    }
}