using Camel.Core.Entities;

namespace Camel.Bancho.Services.Interfaces;

public interface IScoreService
{
    public Task SubmitScoreAsync(string userName, Score score);
    public Task<IList<Score>> GetMapScoresAsync(string mapMd5);
}