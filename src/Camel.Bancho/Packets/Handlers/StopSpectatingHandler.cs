﻿using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Packets.Server;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientStopSpectating)]
public class StopSpectatingHandler : IPacketHandler<StopSpectatingPacket>
{
    private readonly IChatService _chatService;
    private readonly ILogger<StopSpectatingHandler> _logger;

    public StopSpectatingHandler(IChatService chatService, ILogger<StopSpectatingHandler> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }
    
    public async Task HandleAsync(StopSpectatingPacket packet, UserSession userSession)
    {
        var target = userSession.Spectating;
        if (target is null)
        {
            _logger.LogWarning("{} tried to stop spectating while there's no target", userSession.Username);
            return;
        }

        target.Spectators.Remove(userSession);
        userSession.Spectating = null;
        
        target.PacketQueue.WritePacket(new SpectatorLeftPacket(userSession.User.Id));
        var leftPacket = new FellowSpectatorLeftPacket(userSession.User.Id);
        foreach (var spectator in target.Spectators)
        {
            spectator.PacketQueue.WritePacket(leftPacket);
        }
        
        _chatService.LeaveSpectatorChannel(target, userSession);
    }
}