using Camel.Core.Data;
using Camel.Core.Dtos;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Camel.Core.Services;

public class RedisRankingService : IRankingService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDatabase _redis;

    public RedisRankingService(IConnectionMultiplexer connectionMultiplexer, ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _redis = connectionMultiplexer.GetDatabase();
    }

    public async Task UpdateUserRankAsync(int userId, GameMode mode, int pp, long rankedScore)
    {
        // TODO country ranking
        await _redis.SortedSetAddAsync($"leaderboard:{mode}:pp", userId, pp);
        await _redis.SortedSetAddAsync($"leaderboard:{mode}:score", userId, rankedScore);
    }

    public async Task<IEnumerable<UserRankingEntry>> GetGlobalTop(GameMode mode, int count = 50, int offset = 0)
    {
        var idPps = await _redis.SortedSetRangeByRankWithScoresAsync($"leaderboard:{mode}:pp",
            offset, offset + count, Order.Descending);

        var ids = idPps.Select(x => (int)x.Element);

        var userNames = await _dbContext.Users
            .Where(u => ids.Contains(u.Id))
            .Select(u => new
            {
                u.Id, u.UserName,
                u.Stats.Single(s => s.Mode == mode).Accuracy,
                u.Stats.Single(s => s.Mode == mode).Plays
            })
            .ToListAsync();

        var result = idPps.Select((entry, i) => new UserRankingEntry
        {
            UserId = (int)entry.Element,
            Pp = (int)entry.Score,
            Rank = offset + i + 1,
            UserName = userNames.Single(x => x.Id == (int)entry.Element).UserName!,
            Accuracy = userNames.Single(x => x.Id == (int)entry.Element).Accuracy,
            Plays = userNames.Single(x => x.Id == (int)entry.Element).Plays
        });

        return result;
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