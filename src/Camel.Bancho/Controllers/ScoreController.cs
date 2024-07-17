using System.Diagnostics;
using System.Text;
using Camel.Bancho.Middlewares;
using HttpMultipartParser;
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
    [EnableBuffering]
    public async Task<IActionResult> SubmitScore(
        [FromForm(Name = "x")] bool exitedOut,
        [FromForm(Name = "ft")] int failTime,
        [FromForm(Name = "score")] byte[] scoreBase64,
        [FromForm(Name = "token")] string token,
        [FromForm(Name = "fs")] byte[] visualSettings,
        [FromForm(Name = "bmk")] string beatmapHash,
        [FromForm(Name = "sbk")] string? storyboardMd5,
        [FromForm(Name = "iv")] byte[] iv,
        [FromForm(Name = "c1")] string uniqueIds,
        [FromForm(Name = "st")] int scoreTime,
        [FromForm(Name = "pass")] string passwordMd5,
        [FromForm(Name = "osuver")] string osuVersion,
        [FromForm(Name = "s")] byte[] clientHash,
        [FromForm(Name = "i")] byte[]? flCheatScreenshot)
    {
        // asp.net ignores the second field with the same name (score)
        // so replay file (also 'score') is read separately - _-
        Request.Body.Position = 0;
        var content = await MultipartFormDataParser.ParseAsync(Request.Body, Encoding.UTF8);
        var replayFile = content.Files[0];
        Debug.Assert(replayFile.Name == "score");
        
        
        return Ok();
    }
}