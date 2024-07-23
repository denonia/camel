using Camel.Core.Entities;

namespace Camel.Core.Interfaces;

public interface IStatsService
{
    Task<IEnumerable<Stats>> GetUserStatsAsync(int userId);
    Task UpdateStatsAfterSubmissionAsync(int userId, Stats stats, Score score, Score? personalBest);
}