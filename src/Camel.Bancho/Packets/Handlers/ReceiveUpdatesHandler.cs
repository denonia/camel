using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientReceiveUpdates)]
public class ReceiveUpdatesHandler : IPacketHandler<ReceiveUpdatesPacket>
{
    public async Task HandleAsync(ReceiveUpdatesPacket packet, UserSession userSession)
    {
        userSession.PresenceFilter = packet.Value;
    }
}