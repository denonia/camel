using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct SpectatorCantSpectatePacket : IPacket
{
    public PacketType Type => PacketType.ServerSpectatorCantSpectate;

    public int UserId { get; }

    public SpectatorCantSpectatePacket(int userId)
    {
        UserId = userId;
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(UserId);
    }
}