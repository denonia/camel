using Camel.Bancho.Enums;
using Camel.Bancho.Enums.Multiplayer;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Packets.Multiplayer;
using Camel.Bancho.Packets.Server;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Enums;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientJoinLobby)]
public class JoinLobbyHandler : IPacketHandler<JoinLobbyPacket>
{
    private readonly IMultiplayerService _multiplayerService;

    public JoinLobbyHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public async Task HandleAsync(JoinLobbyPacket packet, UserSession userSession)
    {
        foreach (var match in _multiplayerService.ActiveMatches())
        {
            var newMatchPacket = new NewMatchPacket(match.State);
            userSession.PacketQueue.WritePacket(newMatchPacket);
        }
    }
}