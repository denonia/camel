namespace Camel.Bancho.Packets;

public interface IPacketStream
{
    void Write(Packet packet);
    Packet Read();
}