using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Camel.Web.Dtos;
using Camel.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Camel.Web.Pages.Users;

public class Index : PageModel
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IScoreService _scoreService;

    public Index(ApplicationDbContext dbContext, IScoreService scoreService)
    {
        _dbContext = dbContext;
        _scoreService = scoreService;
    }

    public User User { get; set; }
    public Stats Stats { get; set; }
    public IEnumerable<ProfileScore> Scores { get; set; }

    public async Task<IActionResult> OnGetAsync(int userId)
    {
        User = await _dbContext.Users
            .Include(u => u.Stats)
            .SingleOrDefaultAsync(u => u.Id == userId);
        if (User is null)
            return NotFound();

        Stats = User.Stats.Single(s => s.Mode == GameMode.Standard);
        Scores = await _scoreService.GetUserBestScoresAsync(userId, GameMode.Standard);

        return Page();
    }
}