using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct PongPacket : IPacket
{
    public PacketType Type => PacketType.ServerPong;
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
    }
}