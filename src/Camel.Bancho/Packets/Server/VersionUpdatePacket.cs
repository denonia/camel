using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct VersionUpdatePacket : IPacket
{
    public PacketType Type => PacketType.ServerVersionUpdate;
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
    }
}