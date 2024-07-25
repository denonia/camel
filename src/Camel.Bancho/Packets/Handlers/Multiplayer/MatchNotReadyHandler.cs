using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchNotReady)]
public class MatchNotReadyHandler : IPacketHandler<EmptyPayload>
{
    private readonly IMultiplayerService _multiplayerService;

    public MatchNotReadyHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public Task HandleAsync(EmptyPayload payload, UserSession userSession)
    {
        _multiplayerService.Ready(false, userSession);
        return Task.CompletedTask;
    }
}