using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchChangeSlot)]
public class MatchChangeSlotHandler : IPacketHandler<int>
{
    private readonly IMultiplayerService _multiplayerService;

    public MatchChangeSlotHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public async Task HandleAsync(int slotId, UserSession userSession)
    {
        _multiplayerService.ChangeSlot(slotId, userSession);
    }
}