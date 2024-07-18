using Camel.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

[Host("osu.camel.local")]
public class WebController : ControllerBase
{
    private readonly IScoreService _scoreService;
    private readonly IConfiguration _configuration;

    public WebController(IScoreService scoreService, IConfiguration configuration)
    {
        _scoreService = scoreService;
        _configuration = configuration;
    }


    [HttpGet("/web/osu-getreplay.php")]
    public async Task<IActionResult> GetReplay(
        [FromQuery(Name = "u")] string userName,
        [FromQuery(Name = "h")] string passwordMd5,
        [FromQuery(Name = "m")] int mode,
        [FromQuery(Name = "c")] int scoreId)
    {
        var score = await _scoreService.FindScoreAsync(scoreId);
        if (score is null)
            return NotFound();
        
        var dataDir = _configuration.GetRequiredSection("DataDir").Value;
        var path = Path.Combine(Path.GetFullPath(dataDir), "osr", $"{score.Id}.osr");

        if (!System.IO.File.Exists(path))
            return NotFound();
        
        // TODO: replay view count if anyone cares
        
        var fs = new FileStream(path, FileMode.Open);
        return File(fs, "application/octet-stream");
    }

    [HttpGet("/beatmaps/{beatmapId}")]
    public IActionResult BeatmapsRoute([FromRoute] int beatmapId) => 
        Redirect($"https://localhost:7270/beatmaps/{beatmapId}");
}