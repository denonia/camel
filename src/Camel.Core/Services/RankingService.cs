using Camel.Core.Data;
using Camel.Core.Enums;
using Camel.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Camel.Core.Services;

public class RankingService : IRankingService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly Dictionary<int, int> _userIdRanks = new();

    public RankingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public int GetUserGlobalRank(int userId)
    {
        return _userIdRanks.GetValueOrDefault(userId, 0);
    }
    
    private class UserIdRank
    {
        public int UserId { get; set; }
        public int Rank { get; set; }
    }
    
    public async Task FetchRanksAsync()
    {
        // TODO come up with something better
        // and add other mods idc now

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var results = await dbContext.Database.SqlQuery<UserIdRank>(
            $"""
             SELECT user_id, RANK() OVER (ORDER BY pp DESC) rank
             FROM stats
             WHERE mode = {GameMode.Standard}
             """
        ).ToListAsync();
        
        _userIdRanks.Clear();
        
        foreach (var r in results)
        {
            _userIdRanks[r.UserId] = r.Rank;
        }
    }
}