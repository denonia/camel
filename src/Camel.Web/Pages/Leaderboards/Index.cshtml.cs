using Camel.Core.Dtos;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Camel.Web.Pages.Leaderboards;

public class Index : PageModel
{
    private readonly IRankingService _rankingService;

    public Index(IRankingService rankingService)
    {
        _rankingService = rankingService;
    }

    public IEnumerable<UserRankingEntry> RankingEntries { get; set; }

    [FromQuery] public GameMode Mode { get; set; } = GameMode.Standard;

    public string ModeName => Mode switch
    {
        GameMode.Standard => "osu!standard",
        GameMode.Taiko => "osu!taiko",
        GameMode.CatchTheBeat => "osu!catch",
        GameMode.Mania => "osu!mania",
        _ => throw new ArgumentOutOfRangeException()
    };

    public async Task<IActionResult> OnGetAsync()
    {
        RankingEntries = await _rankingService.GetGlobalTop(Mode, 50, 0);

        return Page();
    }
}