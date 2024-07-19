using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Client;

public readonly struct CantSpectatePacket
{
    public static CantSpectatePacket ReadFromStream(PacketBinaryReader reader)
    {
        return new CantSpectatePacket();
    }
}