namespace Camel.Bancho.Packets.Client;

public readonly struct StopSpectatingPacket
{
    public static StopSpectatingPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new StopSpectatingPacket();
    }
}