using Camel.Core.Entities;
using System.Net.Http;
using System.Net.Http.Json;
using Camel.Core.Data;
using Camel.Core.Dtos;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Camel.Core.Services;

public class BeatmapService : IBeatmapService
{
    private const string ApiBaseUrl = "https://osu.direct/api/get_beatmaps?h=";

    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;

    public BeatmapService(ApplicationDbContext dbContext, IHttpClientFactory httpClientFactory)
    {
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Beatmap?> FindBeatmapAsync(string md5)
    {
        var map = await _dbContext.Beatmaps.SingleOrDefaultAsync(b => b.Md5 == md5);
        return map ?? await FetchFromApi(md5);
    }

    private async Task<Beatmap?> FetchFromApi(string md5)
    {
        var url = ApiBaseUrl + md5;
        var httpClient = _httpClientFactory.CreateClient();

        var diffs = await httpClient.GetFromJsonAsync<OsuDirectBeatmap[]>(url);
        if (diffs == null)
            return null;
        
        var beatmaps = diffs.Select(d =>
            new Beatmap
            {
                Id = d.BeatmapId,
                MapsetId = d.BeatmapsetId,
                Md5 = d.FileMd5,
                Artist = d.Artist,
                Title = d.Title,
                Version = d.Version,
                Creator = d.Creator,
                FileName = "",
                LastUpdate = d.LastUpdate,
                TotalLength = d.TotalLength,
                MaxCombo = d.MaxCombo,
                Mode = (GameMode)d.Mode,
                Bpm = d.Bpm,
                CircleSize = d.DiffSize,
                ApproachRate = d.DiffApproach,
                OverallDifficulty = d.DiffOverall,
                HpDrain = d.DiffDrain,
                StarRate = d.Difficultyrating
            }
        ).ToList();

        _dbContext.Beatmaps.AddRange(beatmaps);
        await _dbContext.SaveChangesAsync();
        return beatmaps.SingleOrDefault(b => b.Md5 == md5);
    }
}