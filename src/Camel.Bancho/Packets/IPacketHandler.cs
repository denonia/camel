using Camel.Bancho.Models;

namespace Camel.Bancho.Packets;

public interface IPacketHandler<T>
{
    Task HandleAsync(T packet, UserSession userSession);
}