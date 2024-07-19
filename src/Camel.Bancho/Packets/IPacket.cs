using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets;

public interface IPacket
{
    public PacketType Type { get; }
    
    void WriteToStream(PacketBinaryWriter writer);
}