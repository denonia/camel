using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Packets.Server;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientChannelPart)]
public class ChannelPartHandler : IPacketHandler<ChannelPartPacket>
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChannelPartHandler> _logger;

    public ChannelPartHandler(IChatService chatService, ILogger<ChannelPartHandler> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }
    
    public async Task HandleAsync(ChannelPartPacket packet, UserSession userSession)
    {
        if (_chatService.LeaveChannel(packet.ChannelName, userSession))
        {
            _logger.LogDebug("{} has left {}", userSession.Username, packet.ChannelName);
            userSession.PacketQueue.WritePacket(new ChannelJoinSuccessPacket(packet.ChannelName));
        }
        else
        {
            _logger.LogInformation("{} tried to leave {} but failed", userSession.Username, packet.ChannelName);
        }
    }
}