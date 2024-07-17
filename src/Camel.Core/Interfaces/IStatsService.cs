using Camel.Core.Entities;
using Camel.Core.Enums;

namespace Camel.Core.Interfaces;

public interface IStatsService
{
    public Task<Stats> GetUserStatsAsync(int userId, GameMode mode);
}