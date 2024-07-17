using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Client;

public readonly struct ReceiveUpdatesPacket
{
    public PresenceFilter Value { get; }

    public ReceiveUpdatesPacket(PresenceFilter value)
    {
        Value = value;
    }

    public static ReceiveUpdatesPacket ReadFromStream(Stream stream)
    {
        return new ReceiveUpdatesPacket((PresenceFilter)stream.ReadInt32());
    }
}