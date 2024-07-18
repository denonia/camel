namespace Camel.Bancho.Packets.Client;

public readonly struct RequestStatusUpdatePacket
{
    public static RequestStatusUpdatePacket ReadFromStream(Stream stream)
    {
        return new RequestStatusUpdatePacket();
    }
}