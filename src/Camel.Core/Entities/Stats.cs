using Camel.Core.Enums;

namespace Camel.Core.Entities;

public class Stats
{
    public int Id { get; set; }
    public GameMode Mode { get; set; }
    public long TotalScore { get; set; }
    public long RankedScore { get; set; }
    public short Pp { get; set; }
    public int Plays { get; set; }
    public int PlayTime { get; set; }
    public float Accuracy { get; set; }
    public int MaxCombo { get; set; }
    public int TotalHits { get; set; }
    public int ReplayViews { get; set; }
    public int XHCount { get; set; }
    public int XCount { get; set; }
    public int SHCount { get; set; }
    public int SCount { get; set; }
    public int ACount { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}