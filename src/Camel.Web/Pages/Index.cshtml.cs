using Camel.Core.Data;
using Camel.Core.Dtos;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Camel.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IRankingService _rankingService;

    // TODO might wanna cache this later
    public int TotalUsers => _dbContext.Users.Count();
    public int TotalScores => _dbContext.Scores.Count(s => s.Status != SubmissionStatus.Failed);
    public IEnumerable<UserRankingEntry> RankingEntries { get; set; }

    public IndexModel(ApplicationDbContext dbContext, IRankingService rankingService)
    {
        _dbContext = dbContext;
        _rankingService = rankingService;
    }

    public async Task OnGetAsync()
    {
        RankingEntries = await _rankingService.GetGlobalTop(GameMode.Standard, 50, 0);
    }
}