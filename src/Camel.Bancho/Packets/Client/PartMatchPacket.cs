namespace Camel.Bancho.Packets.Client;

public readonly struct PartMatchPacket
{
    public static PartMatchPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new PartMatchPacket();
    }
}