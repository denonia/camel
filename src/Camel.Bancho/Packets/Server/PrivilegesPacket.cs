using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct PrivilegesPacket : IPacket
{
    public PacketType Type => PacketType.ServerPrivileges;
    
    public int Privileges { get; }

    public PrivilegesPacket(int privileges)
    {
        Privileges = privileges;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(Privileges);
    }
}