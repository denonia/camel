
namespace Camel.Bancho.Packets.Payloads;

public readonly struct ChannelInfo : IPacketPayload
{
    public string Name { get; }
    public string Topic { get; }
    public int PlayerCount { get; }

    public ChannelInfo(string name, string topic, int playerCount)
    {
        Name = name;
        Topic = topic;
        PlayerCount = playerCount;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(Name);
        writer.Write(Topic);
        writer.Write(PlayerCount);
    }
}