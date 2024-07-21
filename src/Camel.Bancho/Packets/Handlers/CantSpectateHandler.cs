using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientCantSpectate)]
public class CantSpectateHandler : IPacketHandler<CantSpectatePacket>
{
    private readonly ILogger<CantSpectateHandler> _logger;

    public CantSpectateHandler(ILogger<CantSpectateHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task HandleAsync(CantSpectatePacket packet, UserSession userSession)
    {
        var target = userSession.Spectating;
        if (target is null)
        {
            _logger.LogWarning("{} tried to send CantSpectate while there's no target", userSession.Username);
            return;
        }

        var cantSpectatePacket = new Server.SpectatorCantSpectatePacket(userSession.User.Id);
        target.PacketQueue.WritePacket(cantSpectatePacket);
        foreach (var spectator in target.Spectators)
        {
            spectator.PacketQueue.WritePacket(cantSpectatePacket);
        }
    }
}