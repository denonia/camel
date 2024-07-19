using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct PrivilegesPacket : IPacket
{
    public PacketType Type => PacketType.ServerPrivileges;
    
    public Privileges Privileges { get; }

    public PrivilegesPacket(Privileges privileges)
    {
        Privileges = privileges;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write((int)Privileges);
    }
}