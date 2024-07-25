using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientLogout)]
public class LogoutHandler : IPacketHandler<EmptyPayload>
{
    private readonly IUserSessionService _sessionService;

    public LogoutHandler(IUserSessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    public Task HandleAsync(EmptyPayload payload, UserSession userSession)
    {
        if (DateTime.Now.Subtract(userSession.StartTime).Seconds > 1)
        {
            _sessionService.EndSession(userSession);

            foreach (var onlineUser in _sessionService.GetOnlineUsers())
            {
                onlineUser.PacketQueue.WriteUserLogout(userSession.User.Id);
            }
        }

        return Task.CompletedTask;
    }
}