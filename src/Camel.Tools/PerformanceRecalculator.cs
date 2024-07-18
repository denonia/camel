using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Performance;
using Microsoft.Extensions.Logging;

namespace Camel.Tools;

public class PerformanceRecalculator
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPerformanceCalculator _performanceCalculator;
    private readonly ILogger<PerformanceRecalculator> _logger;

    public PerformanceRecalculator(ApplicationDbContext dbContext, IPerformanceCalculator performanceCalculator,
        ILogger<PerformanceRecalculator> logger)
    {
        _dbContext = dbContext;
        _performanceCalculator = performanceCalculator;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        var scoresWithBeatmapIds = _dbContext.Scores.Select(s =>
                new Tuple<Score, int>(s, _dbContext.Beatmaps.Single(b => b.Md5 == s.MapMd5).Id))
            .ToList();

        foreach (var (score, beatmapId) in scoresWithBeatmapIds)
        {
            var pp = await _performanceCalculator.CalculateScorePpAsync(score, beatmapId);

            _logger.LogInformation($"Recalculating {score.Id}: {score.Pp} -> {pp}");
            score.Pp = (float)pp;
        }

        await _dbContext.SaveChangesAsync();
    }
}