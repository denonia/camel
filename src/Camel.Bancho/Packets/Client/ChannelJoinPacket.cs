namespace Camel.Bancho.Packets.Client;

public readonly struct ChannelJoinPacket
{
    public string ChannelName { get; }

    public ChannelJoinPacket(string channelName)
    {
        ChannelName = channelName;
    }

    public static ChannelJoinPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new ChannelJoinPacket(reader.ReadString());
    }
}