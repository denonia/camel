using Camel.Bancho.Dtos;
using Camel.Bancho.Enums;
using Camel.Bancho.Packets;
using Camel.Core.Entities;
using Camel.Core.Services;

namespace Camel.Bancho.Models;

public class UserSession
{
    public string Username { get; }
    public string PasswordMd5 { get; }
    public OsuVersion OsuVersion { get; }
    public int UtcOffset { get; }
    public bool? DisplayCity { get; }
    public ClientHashes? ClientHashes { get; }
    public bool? BlockNonFriendPm { get; }
    public Location Location { get; }
    
    public DateTime StartTime { get; } = DateTime.Now;
    public DateTime LastActive { get; set; } = DateTime.Now;

    public PresenceFilter PresenceFilter { get; set; } = PresenceFilter.Nil;
    public UserStatus Status { get; set; }
    
    public User User { get; }
    public IEnumerable<Stats> Stats { get; }

    public PacketQueue PacketQueue { get; }
    public List<UserSession> Spectators { get; } = [];
    public UserSession? Spectating { get; set; } = null;

    public UserSession(LoginRequest loginRequest, User user, Location location, IEnumerable<Stats> stats, PacketQueue packetQueue)
    {
        Username = loginRequest.Username;
        PasswordMd5 = loginRequest.PasswordMd5;
        OsuVersion = OsuVersion.Parse(loginRequest.OsuVersion) ?? throw new ArgumentException("Failed to parse osu! version");
        UtcOffset = loginRequest.UtcOffset;
        DisplayCity = loginRequest.DisplayCity;
        ClientHashes = loginRequest.ClientHashes;
        BlockNonFriendPm = loginRequest.BlockNonFriendPm;
        User = user;
        Location = location;
        Stats = stats;
        PacketQueue = packetQueue;
    }
}