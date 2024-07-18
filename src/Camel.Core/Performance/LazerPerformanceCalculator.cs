using Camel.Core.Enums;
using Camel.Core.Interfaces;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Scoring;

namespace Camel.Core.Performance;

public class LazerPerformanceCalculator : IPerformanceCalculator
{
    private readonly IBeatmapService _beatmapService;

    public LazerPerformanceCalculator(IBeatmapService beatmapService)
    {
        _beatmapService = beatmapService;
    }
    
    public async Task<double> CalculateScorePpAsync(Entities.Beatmap beatmap, Entities.Score score)
    {
        var ruleset = LegacyHelper.GetRuleset(GameMode.Standard);
        var performanceCalculator = ruleset.CreatePerformanceCalculator();

        var osuStream = await _beatmapService.GetBeatmapStreamAsync(beatmap.Id);
        var reader = new LineBufferedReader(osuStream);
        var osuBeatmap = Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
        var workingBeatmap = new CalculatorWorkingBeatmap(osuBeatmap, beatmap.Id);
        
        var scoreInfo = new ScoreInfo(osuBeatmap.BeatmapInfo, ruleset.RulesetInfo)
        {
            
        };
        
        var difficultyCalculator = ruleset.CreateDifficultyCalculator(workingBeatmap);
        var attributes = difficultyCalculator.Calculate(
            LegacyHelper.FilterDifficultyAdjustmentMods(workingBeatmap.BeatmapInfo, ruleset, scoreInfo.Mods));
        
        var performanceAttributes = performanceCalculator?.Calculate(scoreInfo, attributes);
        
        return performanceAttributes.Total;
    }
}