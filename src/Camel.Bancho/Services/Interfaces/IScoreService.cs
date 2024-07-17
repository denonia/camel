using Camel.Core.Entities;
using Camel.Core.ViewModels;

namespace Camel.Bancho.Services.Interfaces;

public interface IScoreService
{
    public Task SubmitScoreAsync(string userName, Score score);
    public Task<IList<LeaderboardScore>> GetLeaderboardScoresAsync(string mapMd5);
    public Task<IList<LeaderboardScore>> GetLeaderboardScoresAsync(int beatmapId);
    public Task<Score?> GetPersonalBestAsync(string userName, string mapMd5);
    public Task<bool> ExistsAsync(string onlineChecksum);
}