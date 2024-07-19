namespace Camel.Bancho.Packets.Client;

public readonly struct LogoutPacket
{
    public static LogoutPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new LogoutPacket();
    }
}