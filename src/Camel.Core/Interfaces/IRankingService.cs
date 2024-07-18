using Camel.Core.Enums;

namespace Camel.Core.Interfaces;

public interface IRankingService
{
    Task UpdateUserStatsAsync(int userId, GameMode mode, int pp, long rankedScore);
    Task<int> GetGlobalRankPpAsync(int userId, GameMode mode);
    Task<int> GetGlobalRankScoreAsync(int userId, GameMode mode);
}