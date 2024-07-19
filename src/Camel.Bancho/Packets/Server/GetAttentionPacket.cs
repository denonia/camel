using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct GetAttentionPacket : IPacket
{
    public PacketType Type => PacketType.ServerGetAttention;
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
    }
}