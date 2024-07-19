using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct ChannelInfoPacket : IPacket
{
    public PacketType Type => PacketType.ServerChannelInfo;
    
    public string Name { get; }
    public string Topic { get; }
    public int PlayerCount { get; }

    public ChannelInfoPacket(string name, string topic, int playerCount)
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