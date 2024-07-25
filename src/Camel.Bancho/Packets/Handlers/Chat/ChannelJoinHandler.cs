using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Chat;

[PacketHandler(PacketType.ClientChannelJoin)]
public class ChannelJoinHandler : IPacketHandler<string>
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChannelJoinHandler> _logger;

    public ChannelJoinHandler(IChatService chatService, ILogger<ChannelJoinHandler> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }
    
    public async Task HandleAsync(string channelName, UserSession userSession)
    {
        if (_chatService.JoinChannel(channelName, userSession))
        {
            _logger.LogDebug("{} has joined {}", userSession.Username, channelName);
        }
        else
        {
            _logger.LogInformation("{} tried to join {} but failed", userSession.Username, channelName);
        }
    }
}