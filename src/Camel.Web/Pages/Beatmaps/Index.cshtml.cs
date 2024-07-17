using Camel.Core.Entities;
using Camel.Core.Interfaces;
using Camel.Web.Dtos;
using Camel.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Camel.Web.Pages.Beatmaps;

public class Index : PageModel
{
    private readonly IScoreService _scoreService;
    private readonly IBeatmapService _beatmapService;

    public Index(IScoreService scoreService, IBeatmapService beatmapService)
    {
        _scoreService = scoreService;
        _beatmapService = beatmapService;
    }

    public Beatmap Beatmap { get; set; }
    public IEnumerable<LeaderboardScore> Scores { get; set; }
    
    public async Task<IActionResult> OnGetAsync(int beatmapId)
    {
        Beatmap = await _beatmapService.FindBeatmapAsync(beatmapId);
        if (Beatmap == null)
            return NotFound();
        
        Scores = await _scoreService.GetLeaderboardScoresAsync(beatmapId);

        return Page();
    }
}