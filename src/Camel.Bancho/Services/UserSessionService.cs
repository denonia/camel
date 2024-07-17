using Camel.Bancho.Models;
using Camel.Bancho.Packets;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Services;

public class UserSessionService : IUserSessionService
{
    private readonly Dictionary<string, UserSession> _activeSessions = new();

    public void AddSession(string accessToken, UserSession userSession)
    {
        _activeSessions[accessToken] = userSession;
    }

    public UserSession? GetSession(string accessToken)
    {
        return _activeSessions.GetValueOrDefault(accessToken);
    }

    public UserSession? GetSessionFromApi(string userName, string passwordMd5)
    {
        return GetOnlineUsers()
            .SingleOrDefault(u => u.Username == userName && u.PasswordMd5 == passwordMd5);
    }

    public IEnumerable<UserSession> GetOnlineUsers()
    {
        return _activeSessions.Values;
    }

    public void WriteGlobalPacket(IWritePacket packet, Func<UserSession, bool>? predicate = null)
    {
        var targets = predicate == null ? GetOnlineUsers() 
            : GetOnlineUsers().Where(predicate);
        
        foreach (var session in targets)
        {
            session.PacketQueue.WritePacket(packet);
        }
    }
}