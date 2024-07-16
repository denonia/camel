using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets;

public readonly struct Packet
{
    public PacketType Type { get; }
    public byte[] Data { get; }

    public Packet(PacketType type, byte[] data)
    {
        Type = type;
        Data = data;
    }
}