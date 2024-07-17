using Camel.Bancho.Models;
using Camel.Bancho.Packets;

namespace Camel.Bancho.Services.Interfaces;

public interface IUserSessionService
{
    void AddSession(string accessToken, UserSession userSession);
    UserSession? GetSession(string accessToken);
    IEnumerable<UserSession> GetOnlineUsers();
    void WriteGlobalPacket(IWritePacket packet, Func<UserSession, bool>? predicate = null);
}