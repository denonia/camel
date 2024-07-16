namespace Camel.Bancho.Packets.Server;

public readonly struct ChannelInfoPacket : IWritePacket
{
    public string Name { get; }
    public string Topic { get; }
    public int PlayerCount { get; }

    public ChannelInfoPacket(string name, string topic, int playerCount)
    {
        Name = name;
        Topic = topic;
        PlayerCount = playerCount;
    }

    public void WriteToStream(IPacketStream stream)
    {
        using var ms = new MemoryStream();
        
        ms.WriteBanchoString(Name);
        ms.WriteBanchoString(Topic);
        ms.Write(BitConverter.GetBytes(PlayerCount));
        
        var packet = new Packet(PacketType.ServerChannelInfo, ms.ToArray());
        stream.Write(packet);
    }
}