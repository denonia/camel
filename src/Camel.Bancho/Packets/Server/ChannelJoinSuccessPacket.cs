using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct ChannelJoinSuccessPacket : IPacket
{
    public PacketType Type => PacketType.ServerChannelJoinSuccess;

    public string ChannelName { get; }

    public ChannelJoinSuccessPacket(string channelName)
    {
        ChannelName = channelName;
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(ChannelName);
    }
}