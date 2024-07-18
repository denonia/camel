namespace Camel.Bancho.Services.Interfaces;

public interface ICacheService
{
    void AddUnsubmittedMap(string md5);
    bool IsInUnsubmittedCache(string md5);
}