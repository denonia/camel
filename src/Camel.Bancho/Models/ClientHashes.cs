namespace Camel.Bancho.Models;

public readonly struct ClientHashes
{
    private readonly string _raw;
    
    public bool RunningUnderWine { get; }

    public string OsuPathMd5 { get; }
    public string AdaptersStr { get; }
    public string AdaptersMd5 { get; }
    public string? UninstallMd5 { get; }
    public string? DiskSignatureMd5 { get; }

    public List<string> Adapters { get; } = [];

    public ClientHashes(string hashesString)
    {
        _raw = hashesString;
        var entries = hashesString.Split(':');
        OsuPathMd5 = entries[0];
        AdaptersStr = entries[1];
        AdaptersMd5 = entries[2];
        UninstallMd5 = entries.ElementAtOrDefault(3);
        DiskSignatureMd5 = entries.ElementAtOrDefault(4);

        RunningUnderWine = AdaptersStr == "runningunderwine";
        if (!RunningUnderWine)
        {
            Adapters = AdaptersStr.Split('.').ToList();
        }
    }

    public override string ToString() => _raw;
    // $"{OsuPathMd5}:{AdaptersStr}:{AdaptersMd5}:{UninstallMd5}:{DiskSignatureMd5}";
}