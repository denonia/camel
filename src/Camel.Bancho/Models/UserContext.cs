using Camel.Bancho.Packets;

namespace Camel.Bancho.Models;

public struct UserContext
{
    public string Username { get; set; }
    public PacketWriter PacketWriter { get; set; }

    public UserContext(string username, PacketWriter packetWriter)
    {
        Username = username;
        PacketWriter = packetWriter;
    }
}