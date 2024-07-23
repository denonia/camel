namespace Camel.Core.Interfaces;

public interface IReplayService
{
    Task SaveReplayAsync(int scoreId, Stream stream);
    Task<Stream?> GetReplayAsync(int scoreId);
}