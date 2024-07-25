namespace Camel.Bancho.Packets.Payloads;

public readonly struct EmptyPayload
{
    public static EmptyPayload ReadFromStream(PacketBinaryReader reader)
    {
        return new EmptyPayload();
    }
}