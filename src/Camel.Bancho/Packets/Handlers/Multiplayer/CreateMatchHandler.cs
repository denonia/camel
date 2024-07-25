using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientCreateMatch)]
public class CreateMatchHandler : IPacketHandler<MatchState>
{
    private readonly IMultiplayerService _multiplayerService;

    public CreateMatchHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public Task HandleAsync(MatchState matchState, UserSession userSession)
    {
        _multiplayerService.CreateMatch(matchState, userSession);
        return Task.CompletedTask;
    }
}