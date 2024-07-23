using Camel.Core.Data;
using Camel.Core.Dtos;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Camel.Core.Performance;
using Microsoft.EntityFrameworkCore;

namespace Camel.Core.Services;

public class ScoreService : IScoreService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPerformanceCalculator _performanceCalculator;

    public ScoreService(ApplicationDbContext dbContext, IPerformanceCalculator performanceCalculator)
    {
        _dbContext = dbContext;
        _performanceCalculator = performanceCalculator;
    }

    public async Task<Score?> FindScoreAsync(int scoreId)
    {
        return await _dbContext.Scores.SingleOrDefaultAsync(s => s.Id == scoreId);
    }

    public async Task<Score?> SubmitScoreAsync(int userId, Score score)
    {
        if (score.Pp <= 0)
            score.Pp = (float)await _performanceCalculator.CalculateScorePpAsync(score);

        var pb = await _dbContext.Scores.AsTracking().SingleOrDefaultAsync(s =>
            s.User.Id == userId &&
            s.Mode == score.Mode &&
            s.MapMd5 == score.MapMd5 &&
            s.Status == SubmissionStatus.Best);

        if (score.Status != SubmissionStatus.Failed)
        {
            if (pb == null)
                score.Status = SubmissionStatus.Best;
            else if (score.ScoreNum > pb.ScoreNum)
            {
                pb.Status = SubmissionStatus.Submitted;
                score.Status = SubmissionStatus.Best;
            }
        }

        score.UserId = userId;
        _dbContext.Scores.Add(score);

        await _dbContext.SaveChangesAsync();

        return pb;
    }

    public async Task<IList<InGameLeaderboardScore>> GetLeaderboardScoresAsync(string mapMd5)
    {
        return await _dbContext.Database.SqlQuery<InGameLeaderboardScore>(
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

    public async Task<IList<ScorePpAcc>> GetUserBestScoresAsync(int userId, GameMode mode)
    {
        return await _dbContext.Database.SqlQuery<ScorePpAcc>(
            $"""
             SELECT s.pp, s.accuracy
             FROM scores s 
             WHERE s.user_id = {userId} AND s.status = {SubmissionStatus.Best} AND s.mode = {mode}
             ORDER BY s.pp DESC
             """
        ).ToListAsync();
    }

    public async Task<Score?> GetPersonalBestAsync(int userId, string mapMd5)
    {
        return await _dbContext.Scores.SingleOrDefaultAsync(s =>
            s.User.Id == userId &&
            s.MapMd5 == mapMd5 &&
            s.Status == SubmissionStatus.Best);
    }

    public async Task<bool> ExistsAsync(string onlineChecksum) =>
        await _dbContext.Scores.AnyAsync(s => s.OnlineChecksum == onlineChecksum);
}