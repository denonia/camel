using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Chat;

[PacketHandler(PacketType.ClientSendPublicMessage)]
public class SendPublicMessageHandler : IPacketHandler<Message>
{
    private readonly IChatService _chatService;
    private readonly ILogger<SendPublicMessageHandler> _logger;

    public SendPublicMessageHandler(IChatService chatService, ILogger<SendPublicMessageHandler> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    public Task HandleAsync(Message message, UserSession user)
    {
        if (_chatService.SendMessage(message.Recipient, message.Text, user))
        {
            _logger.LogInformation("[{}] {}: {}", message.Recipient, user.Username, message.Text);
        }
        else
        {
            _logger.LogInformation("{} unsuccessfully tried to send a message to {} : {}", 
                user.Username, message.Recipient, message.Text);
        }

        return Task.CompletedTask;
    }
}