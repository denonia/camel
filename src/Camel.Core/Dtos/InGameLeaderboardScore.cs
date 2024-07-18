namespace Camel.Core.Dtos;

public class InGameLeaderboardScore
{
    public int Id { get; set; }
    public int ScoreNum { get; set; }
    public int MaxCombo { get; set; }
    public int Mods { get; set; }
    public int Count300 { get; set; }
    public int Count100 { get; set; }
    public int Count50 { get; set; }
    public int CountMiss { get; set; }
    public int CountGeki { get; set; }
    public int CountKatu { get; set; }
    public DateTime SetAt { get; set; }
    public bool Perfect { get; set; }
    
    public int UserId { get; set; }
    public string UserName { get; set; }
}