using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Core.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientRequestStatusUpdate)]
public class RequestStatusUpdateHandler : IPacketHandler<RequestStatusUpdatePacket>
{
    private readonly IRankingService _rankingService;

    public RequestStatusUpdateHandler(IRankingService rankingService)
    {
        _rankingService = rankingService;
    }
    
    public async Task HandleAsync(RequestStatusUpdatePacket packet, UserSession userSession)
    {
        var rank = await _rankingService.GetGlobalRankPpAsync(userSession.User.Id, userSession.Status.Mode);
            
        userSession.PacketQueue.WriteUserStats(userSession, rank);
    }
}