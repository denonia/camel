using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientMatchNotReady)]
public class MatchNotReadyHandler : IPacketHandler<MatchNotReadyPacket>
{
    public async Task HandleAsync(MatchNotReadyPacket packet, UserSession userSession)
    {
        userSession.Match?.Ready(false, userSession);
    }
}