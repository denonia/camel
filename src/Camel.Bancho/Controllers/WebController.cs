using Camel.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

[Host("osu.ppy.sh", "osu.camel.local")]
public class WebController : ControllerBase
{
    private readonly IReplayService _replayService;

    public WebController(IReplayService replayService)
    {
        _replayService = replayService;
    }


    [HttpGet("/web/osu-getreplay.php")]
    public async Task<IActionResult> GetReplay(
        [FromQuery(Name = "u")] string userName,
        [FromQuery(Name = "h")] string passwordMd5,
        [FromQuery(Name = "m")] int mode,
        [FromQuery(Name = "c")] int scoreId)
    {
        var replayStream = await _replayService.GetReplayAsync(scoreId);
        if (replayStream is null)
            return NotFound();
        
        // TODO: replay view count if anyone cares
        
        return File(replayStream, "application/octet-stream");
    }
}