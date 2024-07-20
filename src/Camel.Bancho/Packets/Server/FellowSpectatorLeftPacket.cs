using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct FellowSpectatorLeftPacket : IPacket
{
    public PacketType Type => PacketType.ServerFellowSpectatorLeft;

    public int UserId { get; }

    public FellowSpectatorLeftPacket(int userId)
    {
        UserId = userId;
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(UserId);
    }
}