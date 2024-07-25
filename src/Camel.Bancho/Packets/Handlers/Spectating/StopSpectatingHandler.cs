using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Spectating;

[PacketHandler(PacketType.ClientStopSpectating)]
public class StopSpectatingHandler : IPacketHandler<EmptyPayload>
{
    private readonly IChatService _chatService;
    private readonly ILogger<StopSpectatingHandler> _logger;

    public StopSpectatingHandler(IChatService chatService, ILogger<StopSpectatingHandler> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }
    
    public async Task HandleAsync(EmptyPayload payload, UserSession userSession)
    {
        var target = userSession.Spectating;
        if (target is null)
        {
            _logger.LogWarning("{} tried to stop spectating while there's no target", userSession.Username);
            return;
        }

        target.Spectators.Remove(userSession);
        userSession.Spectating = null;
        
        target.PacketQueue.WriteSpectatorLeft(userSession.User.Id);
        foreach (var spectator in target.Spectators)
        {
            spectator.PacketQueue.WriteFellowSpectatorLeft(userSession.User.Id);
        }
        
        _chatService.LeaveSpectatorChannel(target, userSession);
    }
}