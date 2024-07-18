using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientRequestStatusUpdate)]
public class RequestStatusUpdateHandler : IPacketHandler<RequestStatusUpdatePacket>
{
    public async Task HandleAsync(RequestStatusUpdatePacket packet, UserSession userSession)
    {
        userSession.PacketQueue.WriteUserStats(userSession);
    }
}