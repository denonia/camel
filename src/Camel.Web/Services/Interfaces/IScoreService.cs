using Camel.Core.Enums;
using Camel.Web.Dtos;

namespace Camel.Web.Services.Interfaces;

public interface IScoreService
{
    Task<IList<LeaderboardScore>> GetLeaderboardScoresAsync(int beatmapId);
    Task<IList<ProfileScore>> GetUserBestScoresAsync(int userId, GameMode mode);
}