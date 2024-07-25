using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Enums;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchChangeMods)]
public class MatchChangeModsHandler : IPacketHandler<Mods>
{
    private readonly IMultiplayerService _multiplayerService;

    public MatchChangeModsHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public async Task HandleAsync(Mods newMods, UserSession userSession)
    {
        _multiplayerService.ChangeMods(newMods, userSession);
    }
}