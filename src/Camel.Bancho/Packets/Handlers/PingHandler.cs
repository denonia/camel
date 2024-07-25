using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientPing)]
public class PingHandler : IPacketHandler<EmptyPayload>
{
    public Task HandleAsync(EmptyPayload packet, UserSession userSession)
    {
        userSession.PacketQueue.WritePong();
        return Task.CompletedTask;
    }
}