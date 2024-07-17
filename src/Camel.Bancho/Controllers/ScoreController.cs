using System.Diagnostics;
using System.Text;
using Camel.Bancho.Middlewares;
using Camel.Bancho.Services;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Data;
using Camel.Core.Entities;
using HttpMultipartParser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace Camel.Bancho.Controllers;

public class ScoreController : ControllerBase
{
    private readonly IScoreService _scoreService;
    private readonly CryptoService _cryptoService;
    private readonly ILogger<ScoreController> _logger;

    public ScoreController(IScoreService scoreService, CryptoService cryptoService, ILogger<ScoreController> logger)
    {
        _scoreService = scoreService;
        _cryptoService = cryptoService;
        _logger = logger;
    }
    
    [HttpGet("/web/osu-osz2-getscores.php")]
    public async Task<IActionResult> GetScores(
        [FromQuery(Name = "us")] string userName,
        [FromQuery(Name = "ha")] string passwordMd5,
        [FromQuery(Name = "s")] bool editorSongSelect,
        [FromQuery(Name = "vv")] int leaderboardVersion,
        [FromQuery(Name = "v")] int leaderboardType,
        [FromQuery(Name = "c")] string mapMd5,
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

        var scores = await _scoreService.GetMapScoresAsync(mapMd5);

        response.Append($"2|false|123|123|{scores.Count}|0|\n");
        response.AppendFormat("0\nFull name\n10.0\n");
        // no personal best
        response.Append("\n");

        foreach (var (score, place) in scores.Select((s, i) => (s, i + 1)))
        {
            response.Append(
                $"{score.Id}|{score.User.UserName}|{score.ScoreNum}|{score.MaxCombo}|{score.Count50}|{score.Count100}|{score.Count300}|{score.CountMiss}|{score.CountKatu}|{score.CountGeki}|{score.Perfect}|{score.Mods}|{score.UserId}|{place}|{score.SetAt.Subtract(DateTime.UnixEpoch).TotalSeconds}|1\n");
        }

        return Ok(response.ToString());
    }

    [HttpPost("/web/osu-submit-modular-selector.php")]
    [EnableBuffering]
    public async Task<IActionResult> SubmitScore(
        [FromForm(Name = "x")] bool exitedOut,
        [FromForm(Name = "ft")] int failTime,
        [FromForm(Name = "score")] string scoreBase64,
        [FromForm(Name = "token")] string token,
        [FromForm(Name = "fs")] byte[] visualSettings,
        [FromForm(Name = "bmk")] string beatmapHash,
        [FromForm(Name = "sbk")] string? storyboardMd5,
        [FromForm(Name = "iv")] string ivBase64,
        [FromForm(Name = "c1")] string uniqueIds,
        [FromForm(Name = "st")] int scoreTime,
        [FromForm(Name = "pass")] string passwordMd5,
        [FromForm(Name = "osuver")] string osuVersion,
        [FromForm(Name = "s")] string clientHashBase64,
        [FromForm(Name = "i")] byte[]? flCheatScreenshot)
    {
        // asp.net ignores the second field with the same name (score)
        // so replay file (also 'score') is read separately - _-
        var content = await MultipartFormDataParser.ParseAsync(Request.Body, Encoding.UTF8);
        var replayFile = content.Files[0];
        Debug.Assert(replayFile.Name == "score");

        var (scoreData, clientHash) = _cryptoService.DecryptRijndaelData(
            Convert.FromBase64String(ivBase64), osuVersion,
            Convert.FromBase64String(scoreBase64), Convert.FromBase64String(clientHashBase64));

        var score = Score.FromSubmission(scoreData);
        await _scoreService.SubmitScoreAsync(scoreData[1], score);
        
        _logger.LogInformation("{} has submitted a new score: {}", scoreData[1], string.Join('|', scoreData));

        return Ok();
    }
}