namespace Camel.Bancho.Packets;

public interface IWritePacket
{
    void WriteToStream(IPacketStream stream);
}