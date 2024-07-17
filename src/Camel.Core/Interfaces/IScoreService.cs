using Camel.Core.Entities;
using Camel.Core.ViewModels;

namespace Camel.Core.Interfaces;

public interface IScoreService
{
    public Task SubmitScoreAsync(string userName, Score score);
    public Task<IList<LeaderboardScore>> GetLeaderboardScoresAsync(string mapMd5);
    public Task<Score?> GetPersonalBestAsync(string userName, string mapMd5);
    public Task<bool> ExistsAsync(string onlineChecksum);
}