﻿using Camel.Bancho.Models;
using Camel.Bancho.Packets;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Entities;

namespace Camel.Bancho.Services;

public class UserSessionService : IUserSessionService
{
    private readonly Dictionary<string, UserSession> _activeSessions = new();

    public void AddSession(string accessToken, UserSession userSession)
    {
        EndSession(userSession);
        
        _activeSessions[accessToken] = userSession;
    }

    public void EndSession(UserSession userSession)
    {
        var existing = _activeSessions
            .SingleOrDefault(s => s.Value.Username == userSession.Username);

        if (!existing.Equals(default(KeyValuePair<string, UserSession>)))
        {
            _activeSessions.Remove(existing.Key);
            
            // TODO same as StopSpectatingHandler
            // move this somewhere else
            var session = existing.Value;
            if (session.Spectating is not null)
            {
                var target = session.Spectating;
                target.Spectators.Remove(userSession);
                target.PacketQueue.WriteSpectatorLeft(userSession.User.Id);
                foreach (var spectator in target.Spectators)
                {
                    spectator.PacketQueue.WriteFellowSpectatorLeft(userSession.User.Id);
                }
            }
        }
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
}