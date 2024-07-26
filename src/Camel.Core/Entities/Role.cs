using Microsoft.AspNetCore.Identity;

namespace Camel.Core.Entities;

public class Role : IdentityRole<int>
{
    public Role(string name) : base(name)
    {
    }
}