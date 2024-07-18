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
        userSession.PacketQueue.WriteUserStats(userSession, _rankingService.GetUserGlobalRank(userSession.User.Id));
    }
}