using Camel.Core.Entities;
using System.Net.Http.Json;
using System.Text.Json;
using Camel.Core.Data;
using Camel.Core.Dtos;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Camel.Core.Services;

public class BeatmapService : IBeatmapService
{
    private const string HashApiBaseUrl = "https://osu.direct/api/get_beatmaps?h=";
    private const string IdApiBaseUrl = "https://osu.direct/api/get_beatmaps?b=";
    private const string OsuApiBaseUrl = "https://old.ppy.sh/osu/";

    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public BeatmapService(ApplicationDbContext dbContext, 
        IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
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

    public async Task<Stream?> GetBeatmapStreamAsync(int beatmapId)
    {
        var dataDir = _configuration.GetRequiredSection("DATA_PATH").Value;
        
        var beatmap = await FindBeatmapAsync(beatmapId);
        if (!string.IsNullOrEmpty(beatmap.FileName))
        {
            var existingPath = Path.Combine(Path.GetFullPath(dataDir), "osu", beatmap.FileName);
            return new FileStream(existingPath, FileMode.Open);
        }
        
        var httpClient = _httpClientFactory.CreateClient();
        var url = OsuApiBaseUrl + beatmapId;

        var file = await httpClient.GetAsync(url);

        var fileName = file.Content.Headers.ContentDisposition.FileName.Trim('"');
        var path = Path.Combine(Path.GetFullPath(dataDir), "osu", fileName);

        var fs = new FileStream(path, FileMode.Create);
        var contentStream = await file.Content.ReadAsStreamAsync();
        await contentStream.CopyToAsync(fs);
        fs.Position = 0;

        beatmap.FileName = fileName;
        await _dbContext.SaveChangesAsync();
        
        return fs;
    }

    private async Task<IEnumerable<Beatmap>> FetchFromApiByHash(string md5) => await FetchFromApi(HashApiBaseUrl + md5);
    
    private async Task<IEnumerable<Beatmap>> FetchFromApiById(int beatmapId) => await FetchFromApi(IdApiBaseUrl + beatmapId);

    private async Task<IEnumerable<Beatmap>> FetchFromApi(string url)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync(url);
        // Why do they not just return 404 (39 is length of 'not found' message)
        if (response.Content.Headers.ContentLength <= 39)
            return [];
        
        var stream = await response.Content.ReadAsStreamAsync();
        var diffs = await JsonSerializer.DeserializeAsync<OsuDirectBeatmap[]>(stream);

        if (diffs is null)
            return [];
        
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

        await _dbContext.Beatmaps.AddRangeAsync(beatmaps);
        await _dbContext.SaveChangesAsync();
        return beatmaps;
    }
}