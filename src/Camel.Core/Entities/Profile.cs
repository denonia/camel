namespace Camel.Core.Entities;

public class Profile
{
    public int Id { get; set; }
    public User User { get; set; }

    public string? Twitter { get; set; }
    public string? Discord { get; set; }
    public string? UserPage { get; set; }
}