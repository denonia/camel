using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchScoreUpdate)]
public class MatchScoreUpdateHandler : IPacketHandler<ScoreFrame>
{
    private readonly IMultiplayerService _multiplayerService;

    public MatchScoreUpdateHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public async Task HandleAsync(ScoreFrame scoreFrame, UserSession userSession)
    {
        _multiplayerService.UpdateScore(scoreFrame, userSession);
    }
}