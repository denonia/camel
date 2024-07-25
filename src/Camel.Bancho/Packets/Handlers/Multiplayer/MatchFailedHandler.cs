using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchFailed)]
public class MatchFailedHandler : IPacketHandler<EmptyPayload>
{
    private readonly IMultiplayerService _multiplayerService;

    public MatchFailedHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public Task HandleAsync(EmptyPayload payload, UserSession userSession)
    {
        _multiplayerService.Fail(userSession);
        return Task.CompletedTask;
    }
}