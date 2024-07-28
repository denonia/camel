using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Camel.Web.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IScoreService = Camel.Web.Services.Interfaces.IScoreService;

namespace Camel.Web.Pages.Users;

public class Index : PageModel
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IScoreService _scoreService;
    private readonly IRankingService _rankingService;

    public Index(ApplicationDbContext dbContext, IScoreService scoreService, IRankingService rankingService)
    {
        _dbContext = dbContext;
        _scoreService = scoreService;
        _rankingService = rankingService;
    }

    public User User { get; set; }
    public Stats? Stats { get; set; }
    public IEnumerable<ProfileScore> Scores { get; set; }
    public int Rank { get; set; }
    
    public string AvatarUrl => $"https://a.allein.xyz/{User.Id}";

    public async Task<IActionResult> OnGetAsync(int userId)
    {
        User = await _dbContext.Users
            .Include(u => u.Stats)
            .SingleOrDefaultAsync(u => u.Id == userId);
        if (User is null)
            return NotFound();

        Stats = User.Stats.SingleOrDefault(s => s.Mode == GameMode.Standard);
        Scores = await _scoreService.GetUserBestScoresAsync(userId, GameMode.Standard);
        Rank = await _rankingService.GetGlobalRankPpAsync(userId, GameMode.Standard);

        return Page();
    }
}