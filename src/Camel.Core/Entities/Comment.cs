namespace Camel.Core.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime PostedAt { get; set; } = DateTime.Now.ToUniversalTime();

    public User Author { get; set; }
    public int AuthorId { get; set; }

    public User? User { get; set; }
    public int? UserId { get; set; }

    public Beatmap? Beatmap { get; set; }
    public int? BeatmapId { get; set; }

    public Score? Score { get; set; }
    public int? ScoreId { get; set; }
}