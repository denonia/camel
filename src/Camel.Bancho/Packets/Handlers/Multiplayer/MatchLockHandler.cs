using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchLock)]
public class MatchLockHandler : IPacketHandler<int>
{
    private readonly IMultiplayerService _multiplayerService;

    public MatchLockHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public Task HandleAsync(int slotId, UserSession userSession)
    {
        _multiplayerService.LockSlot(slotId, userSession);
        return Task.CompletedTask;
    }
}