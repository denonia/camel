using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Multiplayer;

[PacketHandler(PacketType.ClientMatchHasBeatmap)]
public class MatchHasBeatmapHandler : IPacketHandler<EmptyPayload>
{
    private readonly IMultiplayerService _multiplayerService;

    public MatchHasBeatmapHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public Task HandleAsync(EmptyPayload packet, UserSession userSession)
    {
        _multiplayerService.HasBeatmap(true, userSession);
        return Task.CompletedTask;
    }
}