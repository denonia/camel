using Camel.Core.Entities;

namespace Camel.Bancho.Dtos;

public class ScoreSubmissionResponse
{
    public bool NewFormat { get; }
    public Score Score { get; }
    public Beatmap Beatmap { get; }
    public Stats Stats { get; }

    public Stats PreviousStats { get; }
    public Score? PreviousPersonalBest { get; }

    public ScoreSubmissionResponse(bool newFormat, Score score, Beatmap beatmap, Stats stats, Stats previousStats,
        Score? previousPersonalBest)
    {
        NewFormat = newFormat;
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
            Entry("accuracy", 
                NewFormat ? PreviousStats.Accuracy.ToString("0.00") : (PreviousStats.Accuracy / 100).ToString("0.0000"), 
                NewFormat ? Stats.Accuracy.ToString("0.00") : (Stats.Accuracy / 100).ToString("0.0000")),
            Entry("pp", PreviousStats.Pp, Stats.Pp),
            Entry("beatmapRanking", 1, 1),
        ];

        List<string> entries =
        [
            // TODO chart url, tonextrank, beatmap rank, achievements
            $"beatmapId:{Beatmap.Id}",
            $"beatmapSetId:{Beatmap.MapsetId}",
            $"beatmapPlaycount:{Beatmap.Plays}",
            $"beatmapPasscount:{Beatmap.Passes}",
            $"approvedDate:{Beatmap.LastUpdate}",
            "\n",
            "chartId:overall",
            "chartUrl:https://osu.ppy.sh/u/",
            "chartName:Overall Ranking",
            "chartEndDate:",
            ..overallRankingEntries,
            $"onlineScoreId:{Score.Id}",
            "toNextRankUser:",
            "toNextRank:0",
            $"achievements:{""}",
            $"achievements-new:{""}"
        ];

        if (NewFormat)
        {
            List<string> beatmapChart =
            [
                "\n",
                "chartId:beatmap",
                // TODO: mapset url
                "chartUrl:https://osu.ppy.sh/b/",
                "chartName:Beatmap Ranking",
                ..beatmapRankingEntries,
                $"onlineScoreId:{Score.Id}",
            ];
            entries.AddRange(beatmapChart);
        }

        return string.Join('|', entries);
    }
}