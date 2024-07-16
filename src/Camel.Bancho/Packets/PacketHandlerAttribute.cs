using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets;

public class PacketHandlerAttribute : Attribute
{
    public PacketType Type { get; }

    public PacketHandlerAttribute(PacketType type)
    {
        Type = type;
    }
}