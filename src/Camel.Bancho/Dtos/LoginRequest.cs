using System.Text;
using Camel.Bancho.Models;

namespace Camel.Bancho.Dtos;

public readonly struct LoginRequest
{
    public LoginRequest(string username, string passwordMd5, string osuVersion, int utcOffset, bool? displayCity,
        ClientHashes? clientHashes, bool? blockNonFriendPm)
    {
        Username = username;
        PasswordMd5 = passwordMd5;
        OsuVersion = osuVersion;
        UtcOffset = utcOffset;
        DisplayCity = displayCity;
        ClientHashes = clientHashes;
        BlockNonFriendPm = blockNonFriendPm;
    }

    public string Username { get; }
    public string PasswordMd5 { get; }
    public string OsuVersion { get; }
    public int UtcOffset { get; }
    public bool? DisplayCity { get; }
    public ClientHashes? ClientHashes { get; }
    public bool? BlockNonFriendPm { get; }

    public static LoginRequest FromBytes(byte[] bytes)
    {
        var str = Encoding.UTF8.GetString(bytes);
        var lines = str.Split('\n').Select(l => l.Replace("\r", "")).ToList();
        var rest = lines[2].Split('|');

        var displayCity = rest.ElementAtOrDefault(2) == "1";
        var blockNonFriendPm = rest.ElementAtOrDefault(4) == "1";

        var clientHashesStr = rest.ElementAtOrDefault(3);
        ClientHashes? clientHashes = !string.IsNullOrEmpty(clientHashesStr) ? new ClientHashes(clientHashesStr) : null;

        return new LoginRequest(lines[0], lines[1],
            rest[0], int.Parse(rest[1]),
            displayCity, clientHashes, blockNonFriendPm);
    }
}