using Camel.Core.Enums;

namespace Camel.Web.Dtos;

public class ProfileScore
{
    public int Id { get; set; }
    public int ScoreNum { get; set; }
    public int MaxCombo { get; set; }
    public Mods Mods { get; set; }
    public int Count300 { get; set; }
    public int Count100 { get; set; }
    public int Count50 { get; set; }
    public int CountMiss { get; set; }
    public int CountGeki { get; set; }
    public int CountKatu { get; set; }
    public DateTime SetAt { get; set; }
    public bool Perfect { get; set; }
    public Grade Grade { get; set; }
    public float Accuracy { get; set; }
    public float Pp { get; set; }

    public int BeatmapId { get; set; }
    public string Artist { get; set; }
    public string Title { get; set; }
    public string Version { get; set; }
    public string OsuVersion { get; set; }
}