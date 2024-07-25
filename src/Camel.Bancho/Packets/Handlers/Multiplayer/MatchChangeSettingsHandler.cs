using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchChangeSettings)]
public class MatchChangeSettingsHandler : IPacketHandler<MatchState>
{
    private readonly IMultiplayerService _multiplayerService;

    public MatchChangeSettingsHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public Task HandleAsync(MatchState state, UserSession userSession)
    {
        _multiplayerService.ChangeSettings(state, userSession);
        return Task.CompletedTask;
    }
}