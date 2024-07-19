using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct ChannelInfoEndPacket : IPacket
{
    public PacketType Type => PacketType.ServerChannelInfoEnd;
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
    }
}