using Camel.Bancho.Enums;
using Camel.Bancho.Models;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientReceiveUpdates)]
public class ReceiveUpdatesHandler : IPacketHandler<PresenceFilter>
{
    public Task HandleAsync(PresenceFilter filter, UserSession userSession)
    {
        userSession.PresenceFilter = filter;
        return Task.CompletedTask;
    }
}