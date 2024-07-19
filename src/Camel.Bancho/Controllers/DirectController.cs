using System.Text;
using System.Web;
using Camel.Bancho.Dtos;
using Camel.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Camel.Bancho.Controllers;

[Host("osu.camel.local")]
public class DirectController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private static string[] Keywords = ["Newest", "Top+Rated", "Most+Played"];

    public DirectController(IHttpClientFactory httpClientFactory)
    {
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

    [HttpGet("/d/{mapsetId}")]
    public IActionResult Download(string mapsetId)
    {
        var noVideo = mapsetId.EndsWith('n');
        if (noVideo)
            mapsetId = mapsetId[..^1];

        var noVideoParam = !noVideo ? "1" : "0";
        var url = $"https://catboy.best/d/{mapsetId}?n={noVideoParam}";
        
        return RedirectPermanent(url);
    }
}