using Camel.Core.Entities;
using Camel.Core.Enums;

namespace Camel.Core.Interfaces;

public interface IStatsService
{
    Task<Stats> GetUserStatsAsync(int userId, GameMode mode);
    Task UpdateStatsAfterSubmissionAsync(int userId, Score score, Score? personalBest);
}