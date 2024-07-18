using Camel.Core.Enums;
using Camel.Core.Interfaces;
using StackExchange.Redis;

namespace Camel.Core.Services;

public class RedisRankingService : IRankingService
{
    private readonly IDatabase _redis;

    public RedisRankingService(IConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer.GetDatabase();
    }

    public async Task UpdateUserStatsAsync(int userId, GameMode mode, int pp, long rankedScore)
    {
        // TODO country ranking
        await _redis.SortedSetAddAsync($"leaderboard:{mode}:pp", userId, pp);
        await _redis.SortedSetAddAsync($"leaderboard:{mode}:score", userId, rankedScore);
    }

    public async Task<int> GetGlobalRankPpAsync(int userId, GameMode mode)
    {
        return (int?)(await _redis.SortedSetRankAsync($"leaderboard:{mode}:pp", userId, Order.Descending) + 1) ?? 0;
    }
    
    public async Task<int> GetGlobalRankScoreAsync(int userId, GameMode mode)
    {
        return (int?)(await _redis.SortedSetRankAsync($"leaderboard:{mode}:score", userId, Order.Descending) + 1) ?? 0;
    }
}