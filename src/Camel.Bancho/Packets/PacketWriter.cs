using Camel.Bancho.Packets.Server;

namespace Camel.Bancho.Packets;

public class PacketWriter
{
    private readonly IPacketStream _packetStream;

    public PacketWriter(IPacketStream packetStream)
    {
        _packetStream = packetStream;
    }

    public void WriteNotification(string text) =>
        new NotificationPacket(text).WriteToStream(_packetStream);

    public void WriteUserId(int id) =>
        new UserIdPacket(id).WriteToStream(_packetStream);

    public void WriteProtocolVersion(int version) =>
        new ProtocolVersionPacket(version).WriteToStream(_packetStream);
    
    public void WritePrivileges(int privileges) =>
        new PrivilegesPacket(privileges).WriteToStream(_packetStream);

    public void WriteChannelInfo(string name, string topic, int playerCount) =>
        new ChannelInfoPacket(name, topic, playerCount).WriteToStream(_packetStream);
    
    public void WriteChannelInfoEnd() =>
        new ChannelInfoEndPacket().WriteToStream(_packetStream);
}