using Camel.Bancho.Models;

namespace Camel.Bancho.Packets;

public interface IPacketHandler<T>
{
    void Handle(T packet, UserSession userSession);
}