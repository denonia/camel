using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct UserIdPacket : IPacket
{
    public PacketType Type => PacketType.ServerUserId;
    
    public int Id { get; }

    public UserIdPacket(int id)
    {
        Id = id;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(Id);
    }
}