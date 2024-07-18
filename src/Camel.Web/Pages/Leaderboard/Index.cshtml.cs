using Camel.Core.Dtos;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Camel.Web.Pages.Leaderboard;

public class Index : PageModel
{
    private readonly IRankingService _rankingService;

    public Index(IRankingService rankingService)
    {
        _rankingService = rankingService;
    }

    public IEnumerable<UserRankingEntry> RankingEntries { get; set; }
    
    public async Task OnGetAsync()
    {
        RankingEntries = await _rankingService.GetGlobalTop(GameMode.Standard, 50, 0);
    }
}