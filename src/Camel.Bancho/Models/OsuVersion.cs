using System.Text.RegularExpressions;

namespace Camel.Bancho.Models;

public readonly struct OsuVersion
{
    private static string _osuVersionRegex = @"^b(?<date>\d{8})(?:\.(?<revision>\d))?(?<stream>beta|cuttingedge|dev|tourney)?$";

    public string Raw { get; }
    public DateOnly Date { get; }
    public int DateNumber { get; }
    public string? Revision { get; }
    public string? Stream { get; }

    public OsuVersion(string raw, DateOnly date, int dateNumber, string? revision, string? stream)
    {
        Raw = raw;
        Date = date;
        DateNumber = dateNumber;
        Revision = revision;
        Stream = stream;
    }

    public override string ToString() => Raw;

    public static OsuVersion? Parse(string input)
    {
        var match = Regex.Match(input, _osuVersionRegex);
        if (!match.Success)
            return null;

        var date = DateOnly.ParseExact(match.Groups["date"].Value, "yyyyMMdd");
        var dateNumber = int.Parse(match.Groups["date"].Value);
        var revision = match.Groups["revision"].Value;
        var stream = match.Groups["stream"].Value;

        if (string.IsNullOrEmpty(revision))
            revision = null;
        if (string.IsNullOrEmpty(stream))
            stream = "stable";
        
        return new OsuVersion(input, date, dateNumber, revision, stream);
    }
}