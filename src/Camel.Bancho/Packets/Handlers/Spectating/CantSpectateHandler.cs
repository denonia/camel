using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;

namespace Camel.Bancho.Packets.Handlers.Spectating;

[PacketHandler(PacketType.ClientCantSpectate)]
public class CantSpectateHandler : IPacketHandler<EmptyPayload>
{
    private readonly ILogger<CantSpectateHandler> _logger;

    public CantSpectateHandler(ILogger<CantSpectateHandler> logger)
    {
        _logger = logger;
    }
    
    public Task HandleAsync(EmptyPayload packet, UserSession userSession)
    {
        var target = userSession.Spectating;
        if (target is null)
        {
            _logger.LogWarning("{} tried to send CantSpectate while there's no target", userSession.Username);
            return Task.CompletedTask;
        }

        target.PacketQueue.WriteSpectatorCantSpectate(userSession.User.Id);
        foreach (var spectator in target.Spectators)
        {
            spectator.PacketQueue.WriteSpectatorCantSpectate(userSession.User.Id);
        }

        return Task.CompletedTask;
    }
}