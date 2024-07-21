using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Packets.Server;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientJoinMatch)]
public class JoinMatchHandler : IPacketHandler<JoinMatchPacket>
{
    private readonly IMultiplayerService _multiplayerService;

    public JoinMatchHandler(IMultiplayerService multiplayerService)
    {
        _multiplayerService = multiplayerService;
    }
    
    public async Task HandleAsync(JoinMatchPacket packet, UserSession userSession)
    {
        if (!_multiplayerService.JoinMatch(packet.MatchId, packet.Password, userSession))
        {
            var failPacket = new MatchJoinFailPacket();
            userSession.PacketQueue.WritePacket(failPacket);
        }
    }
}