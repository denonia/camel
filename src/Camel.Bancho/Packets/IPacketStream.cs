namespace Camel.Bancho.Packets;

public interface IPacketStream
{
    void Write(IPacket packet);
    Packet Read();
}