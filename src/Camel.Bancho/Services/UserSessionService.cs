using Camel.Bancho.Models;
using Camel.Bancho.Packets;

namespace Camel.Bancho.Services;

public class UserSessionService
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