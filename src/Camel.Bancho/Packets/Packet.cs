namespace Camel.Bancho.Packets;

public struct Packet
{
    public required PacketType Type { get; set; }
    public required byte[] Data { get; set; }

    public Packet(PacketType type, byte[] data)
    {
        Type = type;
        Data = data;
    }
}