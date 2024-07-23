using System.Diagnostics;
using Camel.Core.Entities;
using Camel.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Camel.Core.Performance;

// https://github.com/MaxOhn/rosu-pp
public class ExternalPerformanceCalculator : IPerformanceCalculator
{
    private readonly IBeatmapService _beatmapService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ExternalPerformanceCalculator> _logger;

    public ExternalPerformanceCalculator(IBeatmapService beatmapService, IConfiguration configuration,
        ILogger<ExternalPerformanceCalculator> logger)
    {
        _beatmapService = beatmapService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<double> CalculateScorePpAsync(Score score)
    {
        var beatmapPath = await _beatmapService.GetBeatmapPathAsync(score.MapMd5);
        if (string.IsNullOrEmpty(beatmapPath))
        {
            _logger.LogError("Failed to fetch beatmap for pp calculation: {}", score.MapMd5);
            return 0.0;
        }

        var fileName = _configuration.GetRequiredSection("PP_CALC_PATH").Value;

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                ArgumentList =
                {
                    beatmapPath,
                    ((byte)score.Mode).ToString(),
                    score.Mods.ToString(), score.MaxCombo.ToString(),
                    score.Count300.ToString(),
                    score.Count100.ToString(),
                    score.Count50.ToString(),
                    score.CountKatu.ToString(),
                    score.CountGeki.ToString(),
                    score.CountMiss.ToString(),
                    "0",
                    // score.Status == SubmissionStatus.Failed ? "1" : "0",
                    (score.Count50 + score.Count100 + score.Count300 + score.CountMiss).ToString()
                },
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };
        process.Start();
        await process.WaitForExitAsync();
        var resultStr = (await process.StandardOutput.ReadToEndAsync()).Trim();
        if (double.TryParse(resultStr, out var result))
            return result;

        _logger.LogError("Failed to calculate pp for beatmap {} ({})", score.MapMd5,
            await process.StandardError.ReadToEndAsync());
        return 0.0;
    }
}