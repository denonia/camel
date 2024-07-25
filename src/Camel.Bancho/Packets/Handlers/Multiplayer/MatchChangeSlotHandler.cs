using Camel.Bancho.Enums;
using Camel.Bancho.Models;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchChangeSlot)]
public class MatchChangeSlotHandler : IPacketHandler<int>
{
    public async Task HandleAsync(int slotId, UserSession userSession)
    {
        userSession.Match?.ChangeSlot(slotId, userSession);
    }
}