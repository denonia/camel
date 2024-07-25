using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientJoinLobby)]
public class JoinLobbyHandler : IPacketHandler<EmptyPayload>
{
    private readonly IMultiplayerService _multiplayerService;

    public JoinLobbyHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public async Task HandleAsync(EmptyPayload payload, UserSession userSession)
    {
        foreach (var match in _multiplayerService.ActiveMatches())
        {
            userSession.PacketQueue.WriteNewMatch(match.State);
        }
    }
}