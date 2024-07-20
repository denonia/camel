using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct FellowSpectatorJoinedPacket : IPacket
{
    public PacketType Type => PacketType.ServerFellowSpectatorJoined;

    public int UserId { get; }

    public FellowSpectatorJoinedPacket(int userId)
    {
        UserId = userId;
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(UserId);
    }
}