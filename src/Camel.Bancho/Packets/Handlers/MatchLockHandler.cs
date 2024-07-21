using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientMatchLock)]
public class MatchLockHandler : IPacketHandler<MatchLockPacket>
{
    public async Task HandleAsync(MatchLockPacket packet, UserSession userSession)
    {
        userSession.Match?.LockSlot(packet.SlotId, userSession);
    }
}