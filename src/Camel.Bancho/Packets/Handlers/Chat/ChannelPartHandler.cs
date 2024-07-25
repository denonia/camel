using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Chat;

[PacketHandler(PacketType.ClientChannelPart)]
public class ChannelPartHandler : IPacketHandler<string>
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChannelPartHandler> _logger;

    public ChannelPartHandler(IChatService chatService, ILogger<ChannelPartHandler> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }
    
    public async Task HandleAsync(string channelName, UserSession userSession)
    {
        if (_chatService.LeaveChannel(channelName, userSession))
        {
            _logger.LogDebug("{} has left {}", userSession.Username, channelName);
        }
        else
        {
            _logger.LogInformation("{} tried to leave {} but failed", userSession.Username, channelName);
        }
    }
}