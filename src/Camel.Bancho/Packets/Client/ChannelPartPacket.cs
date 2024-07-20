namespace Camel.Bancho.Packets.Client;

public readonly struct ChannelPartPacket
{
    public string ChannelName { get; }

    public ChannelPartPacket(string channelName)
    {
        ChannelName = channelName;
    }

    public static ChannelPartPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new ChannelPartPacket(reader.ReadString());
    }
}