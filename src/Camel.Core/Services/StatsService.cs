using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Camel.Core.Services;

public class StatsService : IStatsService
{
    private readonly ApplicationDbContext _dbContext;

    public StatsService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Stats> GetUserStatsAsync(int userId, GameMode mode)
    {
        var stats = await _dbContext.Stats.SingleOrDefaultAsync(s => s.UserId == userId && s.Mode == mode);

        if (stats == null)
        {
            stats = new Stats
            {
                UserId = userId,
                Mode = mode
            };
            _dbContext.Stats.Add(stats);
            await _dbContext.SaveChangesAsync();
        }

        return stats;
    }
}