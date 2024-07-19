namespace Camel.Bancho.Packets.Client;

public readonly struct SpectateFramesPacket
{
    public SpectateFramesPacket()
    {
        
    }

    public static SpectateFramesPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new SpectateFramesPacket();
    }
}