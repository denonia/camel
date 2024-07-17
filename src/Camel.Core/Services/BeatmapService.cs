using Camel.Core.Entities;
using System.Net.Http.Json;
using Camel.Core.Data;
using Camel.Core.Dtos;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Camel.Core.Services;

public class BeatmapService : IBeatmapService
{
    private const string HashApiBaseUrl = "https://osu.direct/api/get_beatmaps?h=";
    private const string IdApiBaseUrl = "https://osu.direct/api/get_beatmaps?b=";

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
        if (map != null) 
            return map;
        
        var setDiffs = await FetchFromApiByHash(md5);
        return setDiffs.SingleOrDefault(b => b.Md5 == md5);
    }

    public async Task<Beatmap?> FindBeatmapAsync(int beatmapId)
    {
        var map = await _dbContext.Beatmaps.SingleOrDefaultAsync(b => b.Id == beatmapId);
        if (map != null)
            return map;
        
        var setDiffs = await FetchFromApiById(beatmapId);
        return setDiffs.SingleOrDefault(b => b.Id == beatmapId);
    }

    private async Task<List<Beatmap>> FetchFromApiByHash(string md5) => await FetchFromApi(HashApiBaseUrl + md5);
    
    private async Task<List<Beatmap>> FetchFromApiById(int beatmapId) => await FetchFromApi(IdApiBaseUrl + beatmapId);

    private async Task<List<Beatmap>> FetchFromApi(string url)
    {
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
        return beatmaps;
    }
}