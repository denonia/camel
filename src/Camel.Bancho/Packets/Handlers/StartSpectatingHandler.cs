using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Packets.Server;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientStartSpectating)]
public class StartSpectatingHandler : IPacketHandler<StartSpectatingPacket>
{
    private readonly IUserSessionService _userSessionService;
    private readonly ILogger _logger;

    public StartSpectatingHandler(IUserSessionService userSessionService, ILogger<StartSpectatingHandler> logger)
    {
        _userSessionService = userSessionService;
        _logger = logger;
    }

    public async Task HandleAsync(StartSpectatingPacket packet, UserSession userSession)
    {
        // TODO chat channel

        var target = _userSessionService.GetOnlineUsers().SingleOrDefault(s => s.User.Id == packet.TargetUserId);
        if (target == null)
        {
            _logger.LogWarning("{} tried to spectate {} but session wasn't found", userSession.Username, packet.TargetUserId);
            return;
        }

        userSession.Spectating?.Spectators.Remove(userSession);
        
        target.PacketQueue.WritePacket(new SpectatorJoinedPacket(userSession.User.Id));

        var joinPacket = new FellowSpectatorJoinedPacket(userSession.User.Id);
        foreach (var spectator in target.Spectators)
        {
            spectator.PacketQueue.WritePacket(joinPacket);
            userSession.PacketQueue.WritePacket(new FellowSpectatorJoinedPacket(spectator.User.Id));
        }
        
        target.Spectators.Add(userSession);
        userSession.Spectating = target;
    }
}