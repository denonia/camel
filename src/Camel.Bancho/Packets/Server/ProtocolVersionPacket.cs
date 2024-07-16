using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct ProtocolVersionPacket : IWritePacket
{
    public int ProtocolVersion { get; }

    public ProtocolVersionPacket(int protocolVersion)
    {
        ProtocolVersion = protocolVersion;
    }

    public void WriteToStream(IPacketStream stream)
    {
        var packet = new Packet(PacketType.ServerProtocolVersion, BitConverter.GetBytes(ProtocolVersion));
        stream.Write(packet);
    }
}