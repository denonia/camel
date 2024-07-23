using Camel.Core.Entities;
using Camel.Core.Enums;

namespace Camel.Core.Interfaces;

public interface IStatsService
{
    Task<IEnumerable<Stats>> GetUserStatsAsync(int userId);
    Task UpdateStatsAfterSubmissionAsync(Stats stats, Score score, Score? personalBest);
}