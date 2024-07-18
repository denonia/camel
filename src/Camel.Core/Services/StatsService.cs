using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Camel.Core.Services;

public class StatsService : IStatsService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IScoreService _scoreService;

    public StatsService(ApplicationDbContext dbContext, IScoreService scoreService)
    {
        _dbContext = dbContext;
        _scoreService = scoreService;
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

    public async Task UpdateStatsAfterSubmissionAsync(int userId, Score score, Score? personalBest)
    {
        var stats = await GetUserStatsAsync(userId, score.Mode);
        stats.TotalScore += score.ScoreNum;
        stats.TotalHits += score.Count300 + score.Count100 + score.Count50;
        stats.Plays++;

        if (score.MaxCombo > stats.MaxCombo)
            stats.MaxCombo = score.MaxCombo;
        
        if (score.Status == SubmissionStatus.Best)
        {
            var rankedScoreToAdd = score.ScoreNum;
            if (personalBest != null)
                rankedScoreToAdd -= personalBest.ScoreNum;

            stats.RankedScore += rankedScoreToAdd;
            
            var bestScores = await _scoreService.GetUserBestScoresAsync(score.UserId, score.Mode);
            var weightedAccuracy = bestScores
                .Select((s, i) => Math.Pow(0.95, i) * s.Accuracy)
                .Sum();
            var bonusAccuracy = 100 / (20 * (1 - Math.Pow(0.95, bestScores.Count)));
            stats.Accuracy = (float)(weightedAccuracy * bonusAccuracy / 100.0);

            var weightedPp = bestScores
                .Select((s, i) => Math.Pow(0.95, i) * s.Pp)
                .Sum();
            var bonusPp = 416.667 * (1 - Math.Pow(0.9994, bestScores.Count));
            stats.Pp = (short)(weightedPp + bonusPp);
        }
    }
}