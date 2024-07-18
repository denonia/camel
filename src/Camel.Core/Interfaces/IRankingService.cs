namespace Camel.Core.Interfaces;

public interface IRankingService
{
    int GetUserGlobalRank(int userId);
    Task FetchRanksAsync();
}