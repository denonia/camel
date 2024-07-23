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
    private readonly IRankingService _rankingService;

    public StatsService(ApplicationDbContext dbContext, IScoreService scoreService, IRankingService rankingService)
    {
        _dbContext = dbContext;
        _scoreService = scoreService;
        _rankingService = rankingService;
    }

    public async Task<IEnumerable<Stats>> GetUserStatsAsync(int userId)
    {
        var stats = await _dbContext.Stats
            .AsTracking()
            .Where(s => s.UserId == userId)
            .ToListAsync();

        if (stats.Count < 4)
        {
            var modesToCreate = Enum.GetValues<GameMode>()
                .Except(stats.Select(s => s.Mode));
            foreach (var mode in modesToCreate)
            {
                var modeStats = new Stats(userId, mode);
                _dbContext.Stats.Add(modeStats);
                stats.Add(modeStats);
            }

            await _dbContext.SaveChangesAsync();
        }

        return stats;
    }

    public async Task UpdateStatsAfterSubmissionAsync(int userId, Stats stats, Score score, Score? personalBest)
    {
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

        if (score.Status == SubmissionStatus.Best)
            await _rankingService.UpdateUserRankAsync(userId, score.Mode, stats.Pp, stats.RankedScore);

        _dbContext.Entry(stats).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
}