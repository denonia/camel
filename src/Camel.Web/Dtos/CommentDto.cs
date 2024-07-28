namespace Camel.Web.Dtos;

public class CommentDto
{
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public int AuthorId { get; set; }
    public string Text { get; set; }
    public DateTime PostedAt { get; set; }
}