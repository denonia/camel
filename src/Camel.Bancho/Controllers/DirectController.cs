using AspNetCore.Proxy;
using Camel.Bancho.Dtos;
using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Camel.Bancho.Controllers;

[Host("osu.ppy.sh", "osu.camel.local")]
public class DirectController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;

    private static string[] Keywords = ["Newest", "Top+Rated", "Most+Played"];

    public DirectController(ApplicationDbContext dbContext, IHttpClientFactory httpClientFactory)
    {
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("/web/osu-search.php")]
    public async Task<IActionResult> Search(
        [FromQuery(Name = "u")] string userName,
        [FromQuery(Name = "h")] string passwordMd5,
        [FromQuery(Name = "r")] int rankedStatus,
        [FromQuery(Name = "q")] string query,
        [FromQuery(Name = "m")] int mode,
        [FromQuery(Name = "p")] int pageNumber)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "amount", "100" },
            { "offset", (pageNumber * 100).ToString() }
        };

        // TODO support keywords
        if (!Keywords.Contains(query))
            queryParams["query"] = query;

        if (mode != -1)
            queryParams["mode"] = mode.ToString();

        if (rankedStatus != 4)
            queryParams["status"] = rankedStatus.FromOsuDirect().ToOsuApiStatus().ToString();

        var client = _httpClientFactory.CreateClient();

        var url = QueryHelpers.AddQueryString("https://catboy.best/api/search", queryParams);
        var entries = await client.GetFromJsonAsync<List<OsuDirectApiEntry>>(url);

        var response = new OsuDirectResponse(entries);

        return Ok(response.ToString());
    }

    [HttpGet("/web/osu-search-set.php")]
    public async Task<IActionResult> SearchSet(
        [FromQuery(Name = "u")] string userName,
        [FromQuery(Name = "h")] string passwordMd5,
        [FromQuery(Name = "s")] int? mapsetId,
        [FromQuery(Name = "b")] int? mapId,
        [FromQuery(Name = "c")] string? beatmapMd5)
    {
        if (mapsetId == null && mapId == null && string.IsNullOrEmpty(beatmapMd5))
            return BadRequest();

        IQueryable<Beatmap> beatmaps = _dbContext.Beatmaps;

        if (mapsetId is not null)
            beatmaps = beatmaps.Where(b => b.MapsetId == mapsetId);
        if (mapId is not null)
            beatmaps = beatmaps.Where(b => b.Id == mapId);
        if (beatmapMd5 is not null)
            beatmaps = beatmaps.Where(b => b.Md5 == beatmapMd5);

        var result = await beatmaps
            .Select(b => new { b.MapsetId, b.Artist, b.Title, b.Creator, b.LastUpdate })
            .FirstOrDefaultAsync();
        if (result is null)
            return NotFound();

        var response = new SearchSetResponse(
            $"{result.MapsetId}.osz", result.Artist, result.Title, result.Creator, 2, 10.0f, result.LastUpdate,
            result.MapsetId,
            0, false, false, 0, 0);
        return Ok(response.ToString());
    }

    [HttpGet("/d/{mapsetId}")]
    public Task Download(string mapsetId)
    {
        var noVideo = mapsetId.EndsWith('n');
        if (noVideo)
            mapsetId = mapsetId[..^1];

        var noVideoParam = !noVideo ? "1" : "0";
        var url = $"https://catboy.best/d/{mapsetId}?n={noVideoParam}";

        return this.HttpProxyAsync(url);
    }
}