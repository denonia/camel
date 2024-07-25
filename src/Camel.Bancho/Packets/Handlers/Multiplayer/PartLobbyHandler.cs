using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientPartLobby)]
public class LeaveLobbyHandler : IPacketHandler<EmptyPayload>
{
    private readonly IMultiplayerService _multiplayerService;

    public LeaveLobbyHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public Task HandleAsync(EmptyPayload payload, UserSession userSession)
    {
        _multiplayerService.LeaveLobby(userSession);
        return Task.CompletedTask;
    }
}