using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Spectating;

[PacketHandler(PacketType.ClientStartSpectating)]
public class StartSpectatingHandler : IPacketHandler<int>
{
    private readonly IUserSessionService _userSessionService;
    private readonly IChatService _chatService;
    private readonly ILogger _logger;

    public StartSpectatingHandler(IUserSessionService userSessionService, 
        IChatService chatService,
        ILogger<StartSpectatingHandler> logger)
    {
        _userSessionService = userSessionService;
        _chatService = chatService;
        _logger = logger;
    }

    public async Task HandleAsync(int targetUserId, UserSession userSession)
    {
        var target = _userSessionService.GetOnlineUsers().SingleOrDefault(s => s.User.Id == targetUserId);
        if (target == null)
        {
            _logger.LogWarning("{} tried to spectate {} but session wasn't found", userSession.Username, targetUserId);
            return;
        }

        userSession.Spectating?.Spectators.Remove(userSession);
        
        target.PacketQueue.WriteSpectatorJoined(userSession.User.Id);

        foreach (var spectator in target.Spectators)
        {
            spectator.PacketQueue.WriteFellowSpectatorJoined(userSession.User.Id);
            userSession.PacketQueue.WriteFellowSpectatorJoined(spectator.User.Id);
        }

        _chatService.JoinSpectatorChannel(target, userSession);

        target.Spectators.Add(userSession);
        userSession.Spectating = target;
    }
}