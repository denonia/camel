using Camel.Bancho.Enums;
using Camel.Bancho.Models;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchLock)]
public class MatchLockHandler : IPacketHandler<int>
{
    public async Task HandleAsync(int slotId, UserSession userSession)
    {
        userSession.Match?.LockSlot(slotId, userSession);
    }
}