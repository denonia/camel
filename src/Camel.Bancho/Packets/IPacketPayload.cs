using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets;

public interface IPacketPayload
{
    void WriteToStream(PacketBinaryWriter writer);
}