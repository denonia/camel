using Camel.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Camel.Core.Data;

public class DatabaseSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public DatabaseSeeder(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await _roleManager.CreateAsync(new Role("Moderator"));
        await _roleManager.CreateAsync(new Role("Supporter"));
        await _roleManager.CreateAsync(new Role("Owner"));
        await _roleManager.CreateAsync(new Role("Developer"));

        await _userManager.CreateAsync(new User
        {
            UserName = "camel",
            Country = "SP"
        });

        await _userManager.CreateAsync(new User
        {
            UserName = "peppy",
            Country = "AU"
        });
    }
}