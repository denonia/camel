using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchNotReady)]
public class MatchNotReadyHandler : IPacketHandler<EmptyPayload>
{
    public async Task HandleAsync(EmptyPayload payload, UserSession userSession)
    {
        userSession.Match?.Ready(false, userSession);
    }
}