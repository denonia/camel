
namespace Camel.Bancho.Dtos;

public readonly struct OsuDirectResponse
{
    public IList<OsuDirectApiEntry> Maps { get; }

    public OsuDirectResponse(IList<OsuDirectApiEntry> maps)
    {
        Maps = maps;
    }

    public override string ToString()
    {
        string SetInfo(int setId, string artist, string title, string creator, int rankedStatus, DateTime lastUpdate,
            bool hasVideo, string diffs) =>
            $"{setId}.osz|{artist}|{title}|{creator}|{rankedStatus}|10.0|{lastUpdate}|{setId}|0|{hasVideo}|0|0|0|{diffs}";

        string DiffInfo(double difficultyRating, string diffName, double cs, double od, double ar, double hp,
            double mode) =>
            $"[{difficultyRating:0.##}⭐] {diffName} {{cs: {cs:0.#} / od: {od:0.#} / ar: {ar:0.#} / hp: {hp:0.#}}}@{mode}";

        var length = Maps.Count == 100 ? 101 : Maps.Count;

        var lengthRow = new List<string> { length.ToString() };

        var rows = Maps.Select(mapset =>
        {
            var diffs = mapset.ChildrenBeatmaps.Select(diff => DiffInfo(
                diff.DifficultyRating, diff.DiffName, diff.CS, diff.OD, diff.AR, diff.HP, diff.Mode));
            var diffsStr = string.Join(',', diffs);

            return SetInfo(mapset.SetID, mapset.Artist, mapset.Title, mapset.Creator, mapset.RankedStatus,
                mapset.LastUpdate, mapset.HasVideo, diffsStr);
        });

        return string.Join('\n', lengthRow.Concat(rows));
    }
}