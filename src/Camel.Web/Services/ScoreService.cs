using Camel.Core.Data;
using Camel.Core.Enums;
using Camel.Web.Dtos;
using Camel.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Camel.Web.Services;

public class ScoreService : IScoreService
{
    private readonly ApplicationDbContext _dbContext;

    public ScoreService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IList<LeaderboardScore>> GetLeaderboardScoresAsync(int beatmapId)
    {
        return await _dbContext.Database.SqlQuery<LeaderboardScore>(
            $"""
             SELECT s.id, s.count100, s.count300, s.count50, 
             s.count_geki, s.count_katu, s.count_miss, 
             s.max_combo, s.mods, s.perfect, 
             s.score_num, s.set_at, s.user_id, 
             s.grade, s.accuracy, s.pp, u.user_name
             FROM scores s 
             INNER JOIN users u ON u.id = s.user_id 
             INNER JOIN beatmaps b ON b.md5 = s.map_md5
             WHERE b.id = {beatmapId} AND s.status = {SubmissionStatus.Best} 
             ORDER BY s.score_num DESC LIMIT 50
             """
        ).ToListAsync();
    }
    
    public async Task<IList<ProfileScore>> GetUserBestScoresAsync(int userId, GameMode mode)
    {
        return await _dbContext.Database.SqlQuery<ProfileScore>(
            $"""
             SELECT s.id, s.count100, s.count300, s.count50, 
             s.count_geki, s.count_katu, s.count_miss, 
             s.max_combo, s.mods, s.perfect, 
             s.score_num, s.set_at,
             s.grade, s.accuracy, s.pp,
             b.artist, b.title, b.version, b.id AS beatmap_id
             FROM scores s 
             INNER JOIN users u ON u.id = s.user_id 
             INNER JOIN beatmaps b ON b.md5 = s.map_md5
             WHERE s.user_id = {userId} AND s.status = {SubmissionStatus.Best} AND s.mode = {mode}
             ORDER BY s.pp DESC LIMIT 50
             """
        ).ToListAsync();
    }
}