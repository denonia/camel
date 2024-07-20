using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

[Host("b.ppy.sh", "b.camel.local")]
public class BeatmapController : ControllerBase
{
    [HttpGet("/{*path}")]
    public IActionResult Index([FromRoute] string path)
    {
        return RedirectPermanent($"https://b.ppy.sh/{path}");
    }
}