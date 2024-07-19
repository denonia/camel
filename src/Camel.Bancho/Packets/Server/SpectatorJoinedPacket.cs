using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct SpectatorJoinedPacket : IPacket
{
    public PacketType Type => PacketType.ServerSpectatorJoined;

    public int UserId { get; }

    public SpectatorJoinedPacket(int userId)
    {
        UserId = userId;
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(UserId);
    }
}