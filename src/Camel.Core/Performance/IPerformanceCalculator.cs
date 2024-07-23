using Camel.Core.Entities;

namespace Camel.Core.Performance;

public interface IPerformanceCalculator
{
    Task<double> CalculateScorePpAsync(Score score, string? beatmapPath = null);
}