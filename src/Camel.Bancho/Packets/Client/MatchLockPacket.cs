namespace Camel.Bancho.Packets.Client;

public readonly struct MatchLockPacket
{
    public int SlotId { get; }

    public MatchLockPacket(int slotId)
    {
        SlotId = slotId;
    }
    
    public static MatchLockPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new MatchLockPacket(reader.ReadInt32());
    }
}