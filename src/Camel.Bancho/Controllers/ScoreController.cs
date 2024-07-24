using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Camel.Bancho.Dtos;
using Camel.Bancho.Middlewares;
using Camel.Bancho.Models;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using HttpMultipartParser;
using Microsoft.AspNetCore.Mvc;

namespace Camel.Bancho.Controllers;

[Host("osu.ppy.sh", "osu.camel.local")]
public class ScoreController : ControllerBase
{
    private readonly IScoreService _scoreService;
    private readonly IStatsService _statsService;
    private readonly IUserSessionService _userSessionService;
    private readonly ICryptoService _cryptoService;
    private readonly IBeatmapService _beatmapService;
    private readonly IReplayService _replayService;
    private readonly ICacheService _cacheService;
    private readonly ILogger<ScoreController> _logger;

    public ScoreController(
        IScoreService scoreService,
        IStatsService statsService,
        IUserSessionService userSessionService,
        ICryptoService cryptoService,
        IBeatmapService beatmapService,
        IReplayService replayService,
        ICacheService cacheService,
        ILogger<ScoreController> logger)
    {
        _scoreService = scoreService;
        _statsService = statsService;
        _userSessionService = userSessionService;
        _cryptoService = cryptoService;
        _beatmapService = beatmapService;
        _replayService = replayService;
        _cacheService = cacheService;
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
        var session = _userSessionService.GetSessionFromApi(userName, passwordMd5);
        if (session == null)
            return Unauthorized();

        if (_cacheService.IsInUnsubmittedCache(mapMd5))
            return Ok("-1|false");

        var beatmap = await _beatmapService.FindBeatmapAsync(mapMd5);
        if (beatmap is null)
        {
            _cacheService.AddUnsubmittedMap(mapMd5);
            return Ok("-1|false");
        }

        var personalBest = await _scoreService.GetPersonalBestAsync(session.User.Id, mapMd5);
        var scores = await _scoreService.GetLeaderboardScoresAsync(mapMd5);

        var response = new LeaderboardResponse(beatmap, personalBest, userName, scores);

        return Ok(response.ToString());
    }

    [HttpPost("/web/osu-submit-modular.php")]
    [HttpPost("/web/osu-submit-modular-selector.php")]
    [EnableBuffering]
    public async Task<IActionResult> SubmitScore(
        [FromForm(Name = "x")] bool exitedOut,
        [FromForm(Name = "ft")] int failTime,
        [FromForm(Name = "score")] string scoreBase64,
        [FromForm(Name = "fs")] byte[] visualSettings,
        [FromForm(Name = "iv")] string ivBase64,
        [FromForm(Name = "pass")] string passwordMd5,
        [FromForm(Name = "s")] string clientHashBase64,

        // not present in 2013
        [FromForm(Name = "c1")] string? uniqueIds,
        [FromForm(Name = "i")] byte[]? flCheatScreenshot,

        // 2015 client
        [FromForm(Name = "pl")] string? processListHash,

        // latest
        [FromForm(Name = "st")] int? scoreTime,
        [FromForm(Name = "osuver")] string? osuVersion,
        [FromForm(Name = "token")] string? token,
        [FromForm(Name = "bmk")] string? beatmapHash,
        [FromForm(Name = "sbk")] string? storyboardMd5)
    {
        // asp.net ignores the second field with the same name (score)
        // so replay file (also 'score') is read separately - _-
        var content = await MultipartFormDataParser.ParseAsync(Request.Body, Encoding.UTF8);
        var replayFile = content.Files[0];
        Debug.Assert(replayFile.Name == "score");

        var (scoreData, clientHash) = _cryptoService.DecryptRijndaelData(
            Convert.FromBase64String(ivBase64), osuVersion,
            Convert.FromBase64String(scoreBase64), Convert.FromBase64String(clientHashBase64));

        // Why
        var userName = scoreData[1];
        if (userName.EndsWith(' '))
            userName = userName[..^1];

        var session = _userSessionService.GetSessionFromApi(userName, passwordMd5);
        if (session == null)
            return Unauthorized();

        var score = Score.FromSubmission(scoreData);

        if (await _scoreService.ExistsAsync(score.OnlineChecksum))
            return BadRequest("error: no");

        if (score.Status == SubmissionStatus.Failed)
            score.TimeElapsed = failTime;
        else
            score.TimeElapsed = scoreTime ?? 0;

        ValidateScore(score, session, osuVersion, uniqueIds, clientHash, beatmapHash);

        var stats = session.Stats.Single(s => s.Mode == score.Mode);
        var prevStats = new Stats(stats);
        var previousPb = await _scoreService.SubmitScoreAsync(session.User.Id, score);
        await _statsService.UpdateStatsAfterSubmissionAsync(session.User.Id, stats, score, previousPb);

        if (score.Status != SubmissionStatus.Failed)
        {
            await _replayService.SaveReplayAsync(score.Id, replayFile.Data);

            var beatmap = await _beatmapService.FindBeatmapAsync(score.MapMd5);

            _logger.LogInformation("{} ({}) has submitted a new score: {} - {} [{}] - {} pp ({})",
                userName, session.OsuVersion, beatmap.Artist, beatmap.Title, beatmap.Version, score.Pp.ToString("N0"),
                string.Join('|', scoreData));

            var newFormat = Request.Path.Value.Contains("-selector");
            var submissionResponse =
                new ScoreSubmissionResponse(newFormat, score, beatmap, stats, prevStats, previousPb);
            return Ok(submissionResponse.ToString());
        }

        return Ok("error: no");
    }

    private static string? Md5Hex(string? s) =>
        string.IsNullOrEmpty(s)
            ? null
            : Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(s))).ToLower();

    private bool ValidateScore(Score score,
        UserSession userSession,
        string? osuVersion,
        string? uniqueIds,
        string clientHash,
        string? beatmapHash)
    {
        var ids = uniqueIds?.Split('|', 2);
        var uninstallMd5 = Md5Hex(ids?[0]);
        var diskSignatureMd5 = Md5Hex(ids?[1]);

        List<bool> checks =
        [
            CheckMatch(osuVersion, userSession.OsuVersion.DateNumber.ToString(), "osu! version", userSession.Username),
            CheckMatch(clientHash, userSession.ClientHashes.ToString(), "Client hash", userSession.Username),
            CheckMatch(uninstallMd5, userSession.ClientHashes.UninstallMd5, "Uninstall md5", userSession.Username),
            CheckMatch(diskSignatureMd5, userSession.ClientHashes.DiskSignatureMd5, "Disk signature md5",
                userSession.Username),
            CheckMatch(beatmapHash, score.MapMd5, "Beatmap hash", userSession.Username)
            // TODO online checksum
        ];

        return checks.All(c => c);
    }

    private bool CheckMatch(string? first, string second, string criteria, string userName)
    {
        // don't care for now
        if (first is null)
            return false;

        if (!string.Equals(first, second))
        {
            _logger.LogWarning("{}: {} mismatch: {}, expected {}", userName, criteria,
                first, second);
            return false;
        }

        return true;
    }
}