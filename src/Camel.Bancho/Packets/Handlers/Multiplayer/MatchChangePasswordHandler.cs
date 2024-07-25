using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchChangePassword)]
public class MatchChangePasswordHandler : IPacketHandler<MatchState>
{
    private readonly IMultiplayerService _multiplayerService;

    public MatchChangePasswordHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public async Task HandleAsync(MatchState state, UserSession userSession)
    {
        _multiplayerService.ChangePassword(state, userSession);
    }
}