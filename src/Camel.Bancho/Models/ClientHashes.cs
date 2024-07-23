namespace Camel.Bancho.Models;

public readonly struct ClientHashes
{
    public bool RunningUnderWine { get; }

    public string OsuPathMd5 { get; }
    public string AdaptersStr { get; }
    public string AdaptersMd5 { get; }
    public string UninstallMd5 { get; }
    public string DiskSignatureMd5 { get; }

    public List<string> Adapters { get; } = [];

    public ClientHashes(string hashesString)
    {
        var entries = hashesString[..^1].Split(':', 5);
        OsuPathMd5 = entries[0];
        AdaptersStr = entries[1];
        AdaptersMd5 = entries[2];
        UninstallMd5 = entries[3];
        DiskSignatureMd5 = entries[4];

        RunningUnderWine = AdaptersStr == "runningunderwine";
        if (!RunningUnderWine)
        {
            AdaptersStr = AdaptersStr[..^1];
            Adapters = AdaptersStr.Split('.').ToList();
        }
    }

    public override string ToString() =>
        $"{OsuPathMd5}:{AdaptersStr}.:{AdaptersMd5}:{UninstallMd5}:{DiskSignatureMd5}:";
}