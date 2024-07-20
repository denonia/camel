using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct SpectatorLeftPacket : IPacket
{
    public PacketType Type => PacketType.ServerSpectatorLeft;

    public int UserId { get; }

    public SpectatorLeftPacket(int userId)
    {
        UserId = userId;
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(UserId);
    }
}