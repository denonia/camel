using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientCreateMatch)]
public class CreateMatchHandler : IPacketHandler<CreateMatchPacket>
{
    private readonly IMultiplayerService _multiplayerService;

    public CreateMatchHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public async Task HandleAsync(CreateMatchPacket packet, UserSession userSession)
    {
        _multiplayerService.CreateMatch(packet.Match, userSession);
    }
}