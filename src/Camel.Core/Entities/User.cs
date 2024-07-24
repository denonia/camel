using Microsoft.AspNetCore.Identity;

namespace Camel.Core.Entities;

public class User : IdentityUser<int>
{
    public string Country { get; set; }
    
    public IEnumerable<Stats> Stats { get; set; }
    public IEnumerable<Score> Scores { get; set; }
    public IEnumerable<LoginSession> LoginSessions { get; set; }
}