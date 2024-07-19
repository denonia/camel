namespace Camel.Bancho.Packets.Client;

public readonly struct RequestStatusUpdatePacket
{
    public static RequestStatusUpdatePacket ReadFromStream(PacketBinaryReader reader)
    {
        return new RequestStatusUpdatePacket();
    }
}