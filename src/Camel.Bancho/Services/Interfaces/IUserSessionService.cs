using Camel.Bancho.Models;
using Camel.Bancho.Packets;

namespace Camel.Bancho.Services.Interfaces;

public interface IUserSessionService
{
    void AddSession(string accessToken, UserSession userSession);
    UserSession? GetSession(string accessToken);
    UserSession? GetSessionFromApi(string userName, string passwordMd5);
    IEnumerable<UserSession> GetOnlineUsers();
    void WriteGlobalPacket(IPacket packet, Func<UserSession, bool>? predicate = null);
}