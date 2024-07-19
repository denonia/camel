using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public class UserLogoutPacket : IPacket
{
    public PacketType Type => PacketType.ServerUserLogout;

    public int UserId { get; set; }

    public UserLogoutPacket(int userId)
    {
        UserId = userId;
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(UserId);
    }
}