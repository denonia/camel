using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientSendPublicMessage)]
public class SendPublicMessageHandler : IPacketHandler<SendPublicMessagePacket>
{
    private readonly IChatService _chatService;
    private readonly ILogger<SendPublicMessageHandler> _logger;

    public SendPublicMessageHandler(IChatService chatService, ILogger<SendPublicMessageHandler> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    public async Task HandleAsync(SendPublicMessagePacket packet, UserSession user)
    {
        if (_chatService.SendMessage(packet.Recipient, packet.Text, user))
        {
            _logger.LogInformation("[{}] {}: {}", packet.Recipient, user.Username, packet.Text);
        }
        else
        {
            _logger.LogInformation("{} unsuccessfully tried to send a message to {} : {}", 
                user.Username, packet.Recipient, packet.Text);
        }
    }
}