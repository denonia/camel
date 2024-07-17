using Camel.Bancho.Packets;

namespace Camel.Bancho.Models;

public class UserSession
{
    public string Username { get; }
    public string PasswordMd5 { get; }
    public string OsuVersion { get; }
    public int UtcOffset { get; }
    public bool DisplayCity { get; }
    public string ClientHashes { get; }
    public bool BlockNonFriendPm { get; }
    
    public DateTime StartTime { get; } = DateTime.Now;
    public DateTime LastActive { get; set; } = DateTime.Now;

    public PacketQueue PacketQueue { get; }

    public UserSession(LoginRequest loginRequest, PacketQueue packetQueue)
    {
        Username = loginRequest.Username;
        PasswordMd5 = loginRequest.PasswordMd5;
        OsuVersion = loginRequest.OsuVersion;
        UtcOffset = loginRequest.UtcOffset;
        DisplayCity = loginRequest.DisplayCity;
        ClientHashes = loginRequest.ClientHashes;
        BlockNonFriendPm = loginRequest.BlockNonFriendPm;
        PacketQueue = packetQueue;
    }
}