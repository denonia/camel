using Camel.Core.Enums;

namespace Camel.Core.Entities;

public class Beatmap
{
    public int Id { get; set; }
    public int MapsetId { get; set; }
    public string Md5 { get; set; }
    public string Artist { get; set; }
    public string Title { get; set; }
    public string? ArtistUnicode { get; set; }
    public string? TitleUnicode { get; set; }
    public string Version { get; set; }
    public string Creator { get; set; }
    public string? Source { get; set; }
    public string? FileName { get; set; }
    public DateTime LastUpdate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public int TotalLength { get; set; }
    public int MaxCombo { get; set; }
    public bool Frozen { get; set; }
    public int Plays { get; set; }
    public int Passes { get; set; }
    public GameMode Mode { get; set; }
    public float Bpm { get; set; }
    public float CircleSize { get; set; }
    public float ApproachRate { get; set; }
    public float OverallDifficulty { get; set; }
    public float HpDrain { get; set; }
    public float StarRate { get; set; }
    public RankedStatus Status { get; set; }
    public BeatmapSource BeatmapSource { get; set; }

    public IEnumerable<Score> Scores { get; set; }
}