using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchStart)]
public class MatchStartHandler : IPacketHandler<EmptyPayload>
{
    private readonly IMultiplayerService _multiplayerService;

    public MatchStartHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public Task HandleAsync(EmptyPayload payload, UserSession userSession)
    {
        _multiplayerService.Start(userSession);
        return Task.CompletedTask;
    }
}