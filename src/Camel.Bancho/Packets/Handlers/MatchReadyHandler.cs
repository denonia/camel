using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientMatchReady)]
public class MatchReadyHandler : IPacketHandler<MatchReadyPacket>
{
    public async Task HandleAsync(MatchReadyPacket packet, UserSession userSession)
    {
        userSession.Match?.Ready(true, userSession);
    }
}