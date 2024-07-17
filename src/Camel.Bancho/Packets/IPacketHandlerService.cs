using Camel.Bancho.Enums;
using Camel.Bancho.Models;

namespace Camel.Bancho.Packets;

public interface IPacketHandlerService
{
    Task HandleAsync(PacketType type, Stream stream, UserSession userSession);
}