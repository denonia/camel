using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Client;

public readonly struct ReceiveUpdatesPacket
{
    public PresenceFilter Value { get; }

    public ReceiveUpdatesPacket(PresenceFilter value)
    {
        Value = value;
    }

    public static ReceiveUpdatesPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new ReceiveUpdatesPacket((PresenceFilter)reader.ReadInt32());
    }
}