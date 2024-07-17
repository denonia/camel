using Camel.Bancho.Services.Interfaces;
using Camel.Bancho.ViewModels;
using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
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

    public async Task<IList<LeaderboardScore>> GetLeaderboardScoresAsync(string mapMd5)
    {
        return await _dbContext.Database.SqlQuery<LeaderboardScore>(
            $"""
             SELECT s.id, s.count100, s.count300, s.count50, 
             s.count_geki, s.count_katu, s.count_miss, 
             s.max_combo, s.mods, s.perfect, 
             s.score_num, s.set_at, s.user_id, u.user_name  
             FROM scores s 
             INNER JOIN USERS u ON u.id = s.user_id 
             WHERE s.map_md5 = {mapMd5} AND s.status = {SubmissionStatus.Best} 
             ORDER BY s.score_num DESC LIMIT 50
             """
        ).ToListAsync();
    }

    public async Task<Score?> GetPersonalBestAsync(string userName, string mapMd5)
    {
        return await _dbContext.Scores.SingleOrDefaultAsync(s =>
            s.User.UserName == userName && 
            s.MapMd5 == mapMd5 && 
            s.Status == SubmissionStatus.Best);
    }

    public async Task<bool> ExistsAsync(string onlineChecksum) =>
        await _dbContext.Scores.AnyAsync(s => s.OnlineChecksum == onlineChecksum);
}