using Camel.Core.Dtos;
using Camel.Core.Entities;
using Camel.Core.Enums;

namespace Camel.Core.Interfaces;

public interface IScoreService
{
    public Task<Score?> SubmitScoreAsync(string userName, Score score);
    public Task<IList<InGameLeaderboardScore>> GetLeaderboardScoresAsync(string mapMd5);
    public Task<IList<ScorePpAcc>> GetUserBestScoresAsync(int userId, GameMode mode);
    public Task<Score?> GetPersonalBestAsync(string userName, string mapMd5);
    public Task<bool> ExistsAsync(string onlineChecksum);
}