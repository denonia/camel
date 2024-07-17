using System.Text;

namespace Camel.Bancho.Dtos;

public readonly struct LoginRequest
{
    public LoginRequest(string username, string passwordMd5, string osuVersion, int utcOffset, bool displayCity,
        string hashes, bool blockNonFriendPm)
    {
        Username = username;
        PasswordMd5 = passwordMd5;
        OsuVersion = osuVersion;
        UtcOffset = utcOffset;
        DisplayCity = displayCity;
        ClientHashes = hashes;
        BlockNonFriendPm = blockNonFriendPm;
    }

    public string Username { get; }
    public string PasswordMd5 { get; }
    public string OsuVersion { get; }
    public int UtcOffset { get; }
    public bool DisplayCity { get; }
    public string ClientHashes { get; }
    public bool BlockNonFriendPm { get; }

    public static LoginRequest FromBytes(byte[] bytes)
    {
        var str = Encoding.UTF8.GetString(bytes);
        var lines = str.Split('\n');
        var rest = lines[2].Split('|');

        return new LoginRequest(lines[0], lines[1], 
            rest[0], int.Parse(rest[1]), 
            rest[2] == "1", rest[3], rest[4] == "1");
    }
}