namespace Camel.Bancho.Packets.Client;

public readonly struct MatchNotReadyPacket
{
    public static MatchNotReadyPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new MatchNotReadyPacket();
    }
}