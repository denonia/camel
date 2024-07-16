using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

public class ScoreController : ControllerBase
{
    [HttpGet("/web/osu-osz2-getscores.php")]
    public async Task<IActionResult> GetScores(
        [FromQuery(Name = "us")] string userName,
        [FromQuery(Name = "ha")] string passwordMd5,
        [FromQuery(Name = "s")] bool editorSongSelect,
        [FromQuery(Name = "vv")] int leaderboardVersion,
        [FromQuery(Name = "v")] int leaderboardType,
        [FromQuery(Name = "c")] int mapMd5,
        [FromQuery(Name = "f")] string mapFileName,
        [FromQuery(Name = "m")] int mode,
        [FromQuery(Name = "i")] int mapsetId,
        [FromQuery(Name = "mods")] int mods,
        [FromQuery(Name = "h")] int mapPackageHash,
        [FromQuery(Name = "a")] int aqnFilesFound)
    {
        var response = new StringBuilder();
        
        // {ranked_status}|{serv_has_osz2}|{bid}|{bsid}|{len(scores)}|{fa_track_id}|{fa_license_text}
        // {offset}\n{beatmap_name}\n{rating}

        response.AppendFormat("2|false|123|123|2|0|\n");
        response.AppendFormat("0\nFull name\n10.0\n");
        // no personal best
        response.Append("\n");

        response.Append("525151698|Cookiezi|4220072|263|1|95|848|14|44|167|0|0|106585|1|1668739602|1\n");
        response.Append("529711790|hvick225|4039038|248|1|92|859|6|55|160|0|0|108972|2|1686575739|1");
        
        return Ok(response.ToString());
    }
    
    [HttpPost("/web/osu-submit-modular-selector.php")]
    public async Task<IActionResult> SubmitScore()
    {
        return Ok();
    }
}