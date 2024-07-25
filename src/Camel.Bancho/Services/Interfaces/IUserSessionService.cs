using Camel.Bancho.Models;

namespace Camel.Bancho.Services.Interfaces;

public interface IUserSessionService
{
    void AddSession(string accessToken, UserSession userSession);
    void EndSession(UserSession userSession);
    UserSession? GetSession(string accessToken);
    UserSession? GetSessionFromApi(string userName, string passwordMd5);
    IEnumerable<UserSession> GetOnlineUsers();
}