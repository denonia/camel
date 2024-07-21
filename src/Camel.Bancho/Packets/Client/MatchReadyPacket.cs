namespace Camel.Bancho.Packets.Client;

public readonly struct MatchReadyPacket
{
    public static MatchReadyPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new MatchReadyPacket();
    }
}