using Camel.Core.Entities;
using Camel.Core.Enums;

namespace Camel.Core.Interfaces;

public interface IStatsService
{
    Task<Stats> GetUserStatsAsync(int userId, GameMode mode);
    Task UpdateStatsAfterSubmissionAsync(Stats stats, Score score, Score? personalBest);
}