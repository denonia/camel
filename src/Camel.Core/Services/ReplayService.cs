using Camel.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Camel.Core.Services;

public class ReplayService : IReplayService
{
    private readonly IConfiguration _configuration;

    public ReplayService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private string GetReplayPath(int scoreId)
    {
        var dataDir = _configuration.GetRequiredSection("DATA_PATH").Value;
        return Path.Combine(Path.GetFullPath(dataDir), "osr", $"{scoreId}.osr");
    }
    
    public async Task SaveReplayAsync(int scoreId, Stream stream)
    {
        var path = GetReplayPath(scoreId);
        await using var fs = new FileStream(path, FileMode.Create);
        await stream.CopyToAsync(fs);
    }

    public async Task<Stream?> GetReplayAsync(int scoreId)
    {
        var path = GetReplayPath(scoreId);
        return File.Exists(path) ? new FileStream(path, FileMode.Open) : null;
    }
}