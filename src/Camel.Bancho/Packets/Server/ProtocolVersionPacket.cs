using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct ProtocolVersionPacket : IPacket
{
    public PacketType Type => PacketType.ServerProtocolVersion;
    
    public int ProtocolVersion { get; }

    public ProtocolVersionPacket(int protocolVersion)
    {
        ProtocolVersion = protocolVersion;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(ProtocolVersion);
    }
}