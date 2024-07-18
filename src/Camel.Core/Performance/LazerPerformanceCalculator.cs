using Camel.Core.Enums;
using Camel.Core.Interfaces;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Database;
using osu.Game.IO;
using osu.Game.Online.API.Requests.Responses;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osu.Game.Scoring.Legacy;

namespace Camel.Core.Performance;

public class LazerPerformanceCalculator : IPerformanceCalculator
{
    private readonly IBeatmapService _beatmapService;

    public LazerPerformanceCalculator(IBeatmapService beatmapService)
    {
        _beatmapService = beatmapService;
    }
    
    public async Task<double> CalculateScorePpAsync(Entities.Score score, int beatmapId)
    {
        var ruleset = LegacyHelper.GetRuleset(GameMode.Standard);
        var performanceCalculator = ruleset.CreatePerformanceCalculator();

        await using var osuStream = await _beatmapService.GetBeatmapStreamAsync(beatmapId);
        var reader = new LineBufferedReader(osuStream);
        var osuBeatmap = Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
        var workingBeatmap = new CalculatorWorkingBeatmap(osuBeatmap, beatmapId);

        var statistics = new Dictionary<HitResult, int>
        {
            { HitResult.Miss, score.CountMiss },
            { HitResult.Meh, score.Count50 },
            { HitResult.Ok, score.Count100 },
            { HitResult.Great, score.Count300 },
        };
        
        var scoreInfo = new ScoreInfo(osuBeatmap.BeatmapInfo, ruleset.RulesetInfo)
        {
            IsLegacyScore = true,
            LegacyTotalScore = score.ScoreNum,
            Accuracy = score.Accuracy / 100f,
            MaxCombo = score.MaxCombo,
            Passed = score.Status != SubmissionStatus.Failed,
            Mods = ruleset.ConvertFromLegacyMods((LegacyMods)score.Mods) as Mod[],
            Statistics = statistics,
        };
        LegacyScoreDecoder.PopulateMaximumStatistics(scoreInfo, workingBeatmap);
        StandardisedScoreMigrationTools.UpdateFromLegacy(scoreInfo, workingBeatmap);
        
        var difficultyCalculator = ruleset.CreateDifficultyCalculator(workingBeatmap);
        var attributes = difficultyCalculator.Calculate(
            LegacyHelper.FilterDifficultyAdjustmentMods(workingBeatmap.BeatmapInfo, ruleset, scoreInfo.Mods));
        
        var performanceAttributes = performanceCalculator?.Calculate(scoreInfo, attributes);
        
        return performanceAttributes.Total;
    }
}