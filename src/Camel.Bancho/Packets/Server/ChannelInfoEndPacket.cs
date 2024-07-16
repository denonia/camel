namespace Camel.Bancho.Packets.Server;

public readonly struct ChannelInfoEndPacket : IWritePacket
{
    public void WriteToStream(IPacketStream stream)
    {
        var packet = new Packet(PacketType.ServerChannelInfoEnd, []);
        stream.Write(packet);
    }
}