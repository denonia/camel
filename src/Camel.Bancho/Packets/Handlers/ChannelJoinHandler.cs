using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Packets.Server;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientChannelJoin)]
public class ChannelJoinHandler : IPacketHandler<ChannelJoinPacket>
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChannelJoinHandler> _logger;

    public ChannelJoinHandler(IChatService chatService, ILogger<ChannelJoinHandler> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }
    
    public async Task HandleAsync(ChannelJoinPacket packet, UserSession userSession)
    {
        if (_chatService.JoinChannel(packet.ChannelName, userSession))
        {
            _logger.LogDebug("{} has joined {}", userSession.Username, packet.ChannelName);
            userSession.PacketQueue.WritePacket(new ChannelJoinSuccessPacket(packet.ChannelName));

            var channelInfo = _chatService.GetChannel(packet.ChannelName)!;
            userSession.PacketQueue.WriteChannelInfo(channelInfo.Name, channelInfo.Topic, channelInfo.ParticipantCount);
        }
        else
        {
            _logger.LogInformation("{} tried to join {} but failed", userSession.Username, packet.ChannelName);
        }
    }
}