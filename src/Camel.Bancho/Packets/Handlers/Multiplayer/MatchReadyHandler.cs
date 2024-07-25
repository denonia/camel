using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchReady)]
public class MatchReadyHandler : IPacketHandler<EmptyPayload>
{
    public async Task HandleAsync(EmptyPayload payload, UserSession userSession)
    {
        userSession.Match?.Ready(true, userSession);
    }
}