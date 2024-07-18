﻿using System.Diagnostics;
using System.Text;
using Camel.Bancho.Dtos;
using Camel.Bancho.Middlewares;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Camel.Core.Performance;
using HttpMultipartParser;
using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

[Host("osu.camel.local")]
public class ScoreController : ControllerBase
{
    private readonly IScoreService _scoreService;
    private readonly IStatsService _statsService;
    private readonly IUserSessionService _userSessionService;
    private readonly ICryptoService _cryptoService;
    private readonly IBeatmapService _beatmapService;
    private readonly IPerformanceCalculator _performanceCalculator;
    private readonly ILogger<ScoreController> _logger;

    public ScoreController(
        IScoreService scoreService,
        IStatsService statsService,
        IUserSessionService userSessionService,
        ICryptoService cryptoService,
        IBeatmapService beatmapService,
        IPerformanceCalculator performanceCalculator,
        ILogger<ScoreController> logger)
    {
        _scoreService = scoreService;
        _statsService = statsService;
        _userSessionService = userSessionService;
        _cryptoService = cryptoService;
        _beatmapService = beatmapService;
        _performanceCalculator = performanceCalculator;
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
        var scores = await _scoreService.GetLeaderboardScoresAsync(mapMd5);
        var response = new LeaderboardResponse(scores);

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

        var session = _userSessionService.GetSessionFromApi(scoreData[1], passwordMd5);
        if (session == null)
            return Unauthorized();

        var score = Score.FromSubmission(scoreData);

        if (await _scoreService.ExistsAsync(score.OnlineChecksum))
            return BadRequest();

        var beatmap = await _beatmapService.FindBeatmapAsync(score.MapMd5);
        var pp = await _performanceCalculator.CalculateScorePpAsync(score, beatmap.Id);
        score.Pp = (float)pp;
        
        if (score.Status == SubmissionStatus.Best)
            session.PacketQueue.WriteNotification($"u got {(int)pp} pp gz");

        var previousPb = await _scoreService.SubmitScoreAsync(scoreData[1], score);
        var stats = session.User.Stats.Single(s => s.Mode == score.Mode);
        await _statsService.UpdateStatsAfterSubmissionAsync(stats, score, previousPb);

        _logger.LogInformation("{} has submitted a new score: {}", scoreData[1], string.Join('|', scoreData));

        return Ok();
    }
}