using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientMatchChangeSlot)]
public class MatchChangeSlotHandler : IPacketHandler<MatchChangeSlotPacket>
{
    public async Task HandleAsync(MatchChangeSlotPacket packet, UserSession userSession)
    {
        userSession.Match?.ChangeSlot(packet.SlotId, userSession);
    }
}