using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct UserIdPacket : IWritePacket
{
    public int Id { get; }

    public UserIdPacket(int id)
    {
        Id = id;
    }

    public void WriteToStream(IPacketStream stream)
    {
        var packet = new Packet(PacketType.ServerUserId, BitConverter.GetBytes(Id));
        stream.Write(packet);
    }
}