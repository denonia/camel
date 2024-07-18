using Camel.Core.Entities;

namespace Camel.Bancho.Dtos;

public class ScoreSubmissionResponse
{
    public Score Score { get; }
    public Beatmap Beatmap { get; }
    public Stats Stats { get; }

    public Stats PreviousStats { get; }
    public Score? PreviousPersonalBest { get; }
    
    public ScoreSubmissionResponse(Score score, Beatmap beatmap, Stats stats, Stats previousStats, Score? previousPersonalBest)
    {
        Score = score;
        Beatmap = beatmap;
        Stats = stats;
        PreviousStats = previousStats;
        PreviousPersonalBest = previousPersonalBest;
    }

    public override string ToString()
    {
        string Entry(string name, object? before, object? after) =>
            $"{name}Before:{before ??= ""}|{name}After:{after ??= ""}";

        // TODO: add ranks
        List<string> beatmapRankingEntries =
        [
            Entry("rank", PreviousPersonalBest is null ? null : 1, 1),
            Entry("rankedScore", PreviousPersonalBest?.ScoreNum, Score.ScoreNum),
            Entry("totalScore", PreviousPersonalBest?.ScoreNum, Score.ScoreNum),
            Entry("maxCombo", PreviousPersonalBest?.MaxCombo, Score.MaxCombo),
            Entry("accuracy", PreviousPersonalBest?.Accuracy.ToString("0.00"), Score.Accuracy.ToString("0.00")),
            Entry("pp", PreviousPersonalBest?.Pp, Score.Pp)
        ];

        List<string> overallRankingEntries =
        [
            Entry("rank", 1, 1),
            Entry("rankedScore", PreviousStats.RankedScore, Stats.RankedScore),
            Entry("totalScore", PreviousStats.TotalScore, Stats.TotalScore),
            Entry("maxCombo", PreviousStats.MaxCombo, Stats.MaxCombo),
            Entry("accuracy", PreviousStats.Accuracy.ToString("0.00"), Stats.Accuracy.ToString("0.00")),
            Entry("pp", PreviousStats.Pp, Stats.Pp)
        ];

        List<string> entries =
        [
            $"beatmapId:{Beatmap.Id}",
            $"beatmapSetId:{Beatmap.MapsetId}",
            $"beatmapPlaycount:{Beatmap.Plays}",
            $"beatmapPasscount:{Beatmap.Passes}",
            $"approvedDate:{Beatmap.LastUpdate}",
            "\n",
            "chartId:beatmap",
            // TODO: mapset url
            "chartUrl:https://osu.ppy.sh/b/",
            "chartName:Beatmap Ranking",
            ..beatmapRankingEntries,
            $"onlineScoreId:{Score.Id}",
            "\n",
            "chartId:overall",
            "chartUrl:https://osu.ppy.sh/u/",
            "chartName:Overall Ranking",
            ..overallRankingEntries,
            // TODO: achievements
            $"achievements-new:{""}", 
        ];
        
        return string.Join('|', entries);
    }
}