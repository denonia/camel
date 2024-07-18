using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Services;

public class CacheService : ICacheService
{
    private readonly List<string> _unsubmittedMaps = [];

    public void AddUnsubmittedMap(string md5) => _unsubmittedMaps.Add(md5);
    public bool IsInUnsubmittedCache(string md5) => _unsubmittedMaps.Contains(md5);
}