using Camel.Web.Dtos;

namespace Camel.Web.Services.Interfaces;

public interface IScoreService
{
    public Task<IList<LeaderboardScore>> GetLeaderboardScoresAsync(int beatmapId);
}