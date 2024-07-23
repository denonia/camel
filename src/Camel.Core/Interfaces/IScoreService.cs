using Camel.Core.Dtos;
using Camel.Core.Entities;
using Camel.Core.Enums;

namespace Camel.Core.Interfaces;

public interface IScoreService
{
    public Task<Score?> FindScoreAsync(int scoreId);
    public Task<Score?> SubmitScoreAsync(int userId, Score score);
    public Task<IList<InGameLeaderboardScore>> GetLeaderboardScoresAsync(string mapMd5);
    public Task<IList<ScorePpAcc>> GetUserBestScoresAsync(int userId, GameMode mode);
    public Task<Score?> GetPersonalBestAsync(int userId, string mapMd5);
    public Task<bool> ExistsAsync(string onlineChecksum);
}