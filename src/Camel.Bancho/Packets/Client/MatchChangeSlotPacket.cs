namespace Camel.Bancho.Packets.Client;

public readonly struct MatchChangeSlotPacket
{
    public int SlotId { get; }

    public MatchChangeSlotPacket(int slotId)
    {
        SlotId = slotId;
    }
    
    public static MatchChangeSlotPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new MatchChangeSlotPacket(reader.ReadInt32());
    }
}