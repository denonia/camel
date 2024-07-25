using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientJoinMatch)]
public class JoinMatchHandler : IPacketHandler<MatchJoin>
{
    private readonly IMultiplayerService _multiplayerService;

    public JoinMatchHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public Task HandleAsync(MatchJoin packet, UserSession userSession)
    {
        if (!_multiplayerService.JoinMatch(packet.MatchId, packet.Password, userSession))
        {
            userSession.PacketQueue.WriteMatchJoinFail();
        }

        return Task.CompletedTask;
    }
}