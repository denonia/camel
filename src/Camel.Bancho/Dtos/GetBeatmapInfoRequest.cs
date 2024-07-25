namespace Camel.Bancho.Dtos;

public class GetBeatmapInfoRequest
{
    public required IEnumerable<string> Filenames { get; set; }
    public required IEnumerable<int> Ids { get; set; }
}