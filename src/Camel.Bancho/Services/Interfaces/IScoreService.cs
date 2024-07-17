using Camel.Bancho.ViewModels;
using Camel.Core.Entities;

namespace Camel.Bancho.Services.Interfaces;

public interface IScoreService
{
    public Task SubmitScoreAsync(string userName, Score score);
    public Task<IList<LeaderboardScore>> GetLeaderboardScoresAsync(string mapMd5);
    public Task<Score?> GetPersonalBestAsync(string userName, string mapMd5);
    public Task<bool> ExistsAsync(string onlineChecksum);
}