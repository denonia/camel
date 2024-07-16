using Camel.Bancho.Packets;

namespace Camel.Bancho.Models;

public class UserSession
{
    public string Username { get; }
    public DateTime StartTime { get; } = DateTime.Now;
    public DateTime LastActive { get; set; } = DateTime.Now;

    public PacketQueue PacketQueue { get; }

    public UserSession(string username, PacketQueue packetQueue)
    {
        Username = username;
        PacketQueue = packetQueue;
    }
}