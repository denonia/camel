using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchTransferHost)]
public class MatchTransferHostHandler : IPacketHandler<int>
{
    private readonly IMultiplayerService _multiplayerService;

    public MatchTransferHostHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public async Task HandleAsync(int targetSlotId, UserSession userSession)
    {
        _multiplayerService.TransferHost(targetSlotId, userSession);
    }
}