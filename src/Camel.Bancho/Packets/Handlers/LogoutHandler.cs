using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Packets.Server;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientLogout)]
public class LogoutHandler : IPacketHandler<LogoutPacket>
{
    private readonly IUserSessionService _sessionService;

    public LogoutHandler(IUserSessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    public async Task HandleAsync(LogoutPacket packet, UserSession userSession)
    {
        if (DateTime.Now.Subtract(userSession.StartTime).Seconds > 1)
        {
            _sessionService.EndSession(userSession);

            var logoutPacket = new UserLogoutPacket(userSession.User.Id);
            foreach (var onlineUser in _sessionService.GetOnlineUsers())
            {
                onlineUser.PacketQueue.WritePacket(logoutPacket);
            }
        }
    }
}