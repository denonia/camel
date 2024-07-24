namespace Camel.Core.Entities;

public class LoginSession
{
    public int Id { get; set; }
    public string OsuVersion { get; set; }
    public bool RunningUnderWine { get; set; }
    public string? OsuPathMd5 { get; set; }
    public string? AdaptersStr { get; set; }
    public string? AdaptersMd5 { get; set; }
    public string? UninstallMd5 { get; set; }
    public string? DiskSignatureMd5 { get; set; }
    public string IpAddress { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now.ToUniversalTime();

    public int UserId { get; set; }
    public User User { get; set; }

    public LoginSession(int userId, string osuVersion, bool runningUnderWine, string? osuPathMd5, string? adaptersStr,
        string? adaptersMd5, string? uninstallMd5, string? diskSignatureMd5, string ipAddress)
    {
        UserId = userId;
        OsuVersion = osuVersion;
        RunningUnderWine = runningUnderWine;
        OsuPathMd5 = osuPathMd5;
        AdaptersStr = adaptersStr;
        AdaptersMd5 = adaptersMd5;
        UninstallMd5 = uninstallMd5;
        DiskSignatureMd5 = diskSignatureMd5;
        IpAddress = ipAddress;
    }
}