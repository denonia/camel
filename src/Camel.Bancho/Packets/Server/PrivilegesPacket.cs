using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct PrivilegesPacket : IWritePacket
{
    public int Privileges { get; }

    public PrivilegesPacket(int privileges)
    {
        Privileges = privileges;
    }
    
    public void WriteToStream(IPacketStream stream)
    {
        var packet = new Packet(PacketType.ServerPrivileges, BitConverter.GetBytes(Privileges));
        stream.Write(packet);
    }
}