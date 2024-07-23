using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Performance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Camel.Tools;

public class PerformanceRecalculator
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IPerformanceCalculator _performanceCalculator;
    private readonly ILogger<PerformanceRecalculator> _logger;

    private readonly string _dataDir;

    public PerformanceRecalculator(ApplicationDbContext dbContext, 
        IConfiguration configuration,
        IPerformanceCalculator performanceCalculator,
        ILogger<PerformanceRecalculator> logger)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _performanceCalculator = performanceCalculator;
        _logger = logger;
        
        _dataDir = _configuration.GetRequiredSection("DATA_PATH").Value!;
    }

    private string BeatmapPath(string fileName) =>
        Path.Combine(Path.GetFullPath(_dataDir), "osu", fileName);

    public async Task RunAsync()
    {
        var scoresWithBeatmapPaths = _dbContext.Scores
            .AsTracking()
            .Select(s =>
                new Tuple<Score, string>(s, _dbContext.Beatmaps.Single(b => b.Md5 == s.MapMd5).FileName))
            .ToList();

        foreach (var (score, fileName) in scoresWithBeatmapPaths)
        {
            var pp = await _performanceCalculator.CalculateScorePpAsync(score, BeatmapPath(fileName));

            _logger.LogInformation($"Recalculating {score.Id}: {score.Pp} -> {pp}");
            score.Pp = (float)pp;
        }

        await _dbContext.SaveChangesAsync();
    }
}