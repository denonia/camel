using Camel.Core.Dtos;
using Camel.Core.Enums;

namespace Camel.Core.Interfaces;

public interface IRankingService
{
    Task UpdateUserRankAsync(int userId, GameMode mode, int pp, long rankedScore);
    Task<IEnumerable<UserRankingEntry>> GetGlobalTop(GameMode mode, int count, int offset);
    Task<int> GetGlobalRankPpAsync(int userId, GameMode mode);
    Task<int> GetGlobalRankScoreAsync(int userId, GameMode mode);
}