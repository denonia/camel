using Camel.Core.Enums;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Game.Beatmaps;
using osu.Game.Skinning;

namespace Camel.Core.Performance;

public class CalculatorWorkingBeatmap : WorkingBeatmap
{
    private readonly Beatmap _beatmap;
    
    public CalculatorWorkingBeatmap(Beatmap beatmap, int? beatmapId = null) : base(beatmap.BeatmapInfo, null)
    {
        _beatmap = beatmap;
        
        beatmap.BeatmapInfo.Ruleset = LegacyHelper.GetRuleset((GameMode)beatmap.BeatmapInfo.Ruleset.OnlineID).RulesetInfo;

        if (beatmapId.HasValue)
            beatmap.BeatmapInfo.OnlineID = beatmapId.Value;
    }

    protected override IBeatmap GetBeatmap() => _beatmap;
    public override Texture GetBackground() => null;
    protected override Track GetBeatmapTrack() => null;
    protected override ISkin GetSkin() => null;
    public override Stream GetStream(string storagePath) => null;
}