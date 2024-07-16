using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;

namespace Camel.Core.Services;

public class StatsService
{
    private readonly ApplicationDbContext _dbContext;

    public StatsService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Stats GetUserStats(int userId, GameMode mode)
    {
        var stats = _dbContext.Stats.SingleOrDefault(s => s.UserId == userId && s.Mode == mode);

        if (stats == null)
        {
            stats = new Stats
            {
                UserId = userId,
                Mode = mode
            };
            _dbContext.Stats.Add(stats);
            _dbContext.SaveChanges();
        }

        return stats;
    }
}