using Microsoft.AspNetCore.Identity;

namespace Camel.Core.Entities;

public class User : IdentityUser<int>
{
    public IEnumerable<Stats> Stats { get; set; }
}