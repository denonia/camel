using Camel.Core.Enums;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;

namespace Camel.Core.Performance;

public class LegacyHelper
{
    public static Ruleset GetRuleset(GameMode gameMode) => gameMode switch
    {
        GameMode.Standard => new OsuRuleset(),
        GameMode.Taiko => new TaikoRuleset(),
        GameMode.CatchTheBeat => new CatchRuleset(),
        GameMode.Mania => new ManiaRuleset(),
        _ => throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null)
    };
    
    public const LegacyMods KEY_MODS = LegacyMods.Key1 | LegacyMods.Key2 | LegacyMods.Key3 | LegacyMods.Key4 | LegacyMods.Key5 | LegacyMods.Key6 | LegacyMods.Key7 | LegacyMods.Key8
                                       | LegacyMods.Key9 | LegacyMods.KeyCoop;

    // See: https://github.com/ppy/osu-queue-score-statistics/blob/2264bfa68e14bb16ec71a7cac2072bdcfaf565b6/osu.Server.Queues.ScoreStatisticsProcessor/Helpers/LegacyModsHelper.cs
    public static LegacyMods MaskRelevantMods(LegacyMods mods, bool isConvertedBeatmap, int rulesetId)
    {
        LegacyMods relevantMods = LegacyMods.DoubleTime | LegacyMods.HalfTime | LegacyMods.HardRock | LegacyMods.Easy;

        switch (rulesetId)
        {
            case 0:
                if ((mods & LegacyMods.Flashlight) > 0)
                    relevantMods |= LegacyMods.Flashlight | LegacyMods.Hidden | LegacyMods.TouchDevice;
                else
                    relevantMods |= LegacyMods.Flashlight | LegacyMods.TouchDevice;
                break;

            case 3:
                if (isConvertedBeatmap)
                    relevantMods |= KEY_MODS;
                break;
        }

        return mods & relevantMods;
    }
    
    
    /// <summary>
    /// Transforms a given <see cref="Mod"/> combination into one which is applicable to legacy scores.
    /// This is used to match osu!stable/osu!web calculations for the time being, until such a point that these mods do get considered.
    /// </summary>
    public static LegacyMods ConvertToLegacyDifficultyAdjustmentMods(BeatmapInfo beatmapInfo, Ruleset ruleset, Mod[] mods)
    {
        var legacyMods = ruleset.ConvertToLegacyMods(mods);

        // mods that are not represented in `LegacyMods` (but we can approximate them well enough with others)
        if (mods.Any(mod => mod is ModDaycore))
            legacyMods |= LegacyMods.HalfTime;

        return MaskRelevantMods(legacyMods, ruleset.RulesetInfo.OnlineID != beatmapInfo.Ruleset.OnlineID, ruleset.RulesetInfo.OnlineID);
    }

    /// <summary>
    /// Transforms a given <see cref="Mod"/> combination into one which is applicable to legacy scores.
    /// This is used to match osu!stable/osu!web calculations for the time being, until such a point that these mods do get considered.
    /// </summary>
    public static Mod[] FilterDifficultyAdjustmentMods(BeatmapInfo beatmapInfo, Ruleset ruleset, Mod[] mods)
        => ruleset.ConvertFromLegacyMods(ConvertToLegacyDifficultyAdjustmentMods(beatmapInfo, ruleset, mods)).ToArray();
}