using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers.Chat;

[PacketHandler(PacketType.ClientSendPrivateMessage)]
public class SendPrivateMessageHandler : IPacketHandler<Message>
{
    private readonly IUserSessionService _sessionService;
    private readonly ILogger<SendPrivateMessageHandler> _logger;

    public SendPrivateMessageHandler(IUserSessionService sessionService, ILogger<SendPrivateMessageHandler> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
    }

    public async Task HandleAsync(Message message, UserSession user)
    {
        var recipient = _sessionService.GetOnlineUsers().SingleOrDefault(s => s.Username == message.Recipient);
        if (recipient is null)
        {
            _logger.LogInformation("{} tried to send {} a message but the recipient is offline: {}", 
                user.Username, message.Recipient, message.Text);
            return;
        }
        
        recipient.PacketQueue.WriteSendMessage(user.Username, message.Text, message.Recipient, user.User.Id);
        _logger.LogInformation("{} -> {}: {}", user.Username, message.Recipient, message.Text);
    }
}