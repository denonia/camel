namespace Camel.Core.Dtos;

public class UserRankingEntry
{
    public int UserId { get; set; }
    public int Rank { get; set; }
    public string UserName { get; set; }
    public int Pp { get; set; }
    public float Accuracy { get; set; }
    public int Plays { get; set; }
}