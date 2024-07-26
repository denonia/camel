using Microsoft.AspNetCore.Identity;

namespace Camel.Core.Entities;

public class User : IdentityUser<int>
{
    public DateTime JoinedAt { get; set; } = DateTime.Now.ToUniversalTime();
    public string Country { get; set; }
    public Profile Profile { get; set; }
    
    public IEnumerable<Stats> Stats { get; set; }
    public IEnumerable<Score> Scores { get; set; }
    public IEnumerable<LoginSession> LoginSessions { get; set; }
    
    public IEnumerable<Relationship> Added { get; set; }
    public IEnumerable<Relationship> AddedBy { get; set; }
}