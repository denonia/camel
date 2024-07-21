using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

public class BeatmapController : ControllerBase
{
    [Host("b.ppy.sh", "b.camel.local")]
    [HttpGet("/{*path}")]
    public IActionResult Index([FromRoute] string path) =>
        RedirectPermanent($"https://b.ppy.sh/{path}");

    [Host("osu.ppy.sh", "osu.camel.local")]
    [HttpGet("/beatmaps/{*path}")]
    public IActionResult BeatmapsRedirect([FromRoute] string path) =>
        RedirectPermanent($"https://osu.ppy.sh/beatmaps/{path}");
    
    [Host("osu.ppy.sh", "osu.camel.local")]
    [HttpGet("/beatmapsets/{*path}")]
    public IActionResult BeatmapsetsRedirect([FromRoute] string path) =>
        RedirectPermanent($"https://osu.ppy.sh/beatmapsets/{path}");
}