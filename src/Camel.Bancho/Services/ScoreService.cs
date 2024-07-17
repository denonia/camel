using Camel.Bancho.Services.Interfaces;
using Camel.Core.Data;
using Camel.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Camel.Bancho.Services;

public class ScoreService : IScoreService
{
    private readonly ApplicationDbContext _dbContext;

    public ScoreService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SubmitScoreAsync(string userName, Score score)
    {
        var user = await _dbContext.Users.SingleAsync(u => u.UserName == userName);
        score.User = user;
        _dbContext.Scores.Add(score);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IList<Score>> GetMapScoresAsync(string mapMd5)
    {
        return await _dbContext.Scores
            .Where(s => s.MapMd5 == mapMd5)
            .Include(s => s.User)
            .ToListAsync();
    }
}