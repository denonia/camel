namespace Camel.Bancho.Dtos;

public class GetBeatmapInfoRequest
{
    public IEnumerable<string> Filenames { get; set; }
    public IEnumerable<int> Ids { get; set; }
}