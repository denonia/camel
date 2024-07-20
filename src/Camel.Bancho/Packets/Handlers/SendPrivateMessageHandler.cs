using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientSendPrivateMessage)]
public class SendPrivateMessageHandler : IPacketHandler<SendPrivateMessagePacket>
{
    private readonly IUserSessionService _sessionService;
    private readonly ILogger<SendPrivateMessageHandler> _logger;

    public SendPrivateMessageHandler(IUserSessionService sessionService, ILogger<SendPrivateMessageHandler> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
    }

    public async Task HandleAsync(SendPrivateMessagePacket packet, UserSession user)
    {
        var recipient = _sessionService.GetOnlineUsers().SingleOrDefault(s => s.Username == packet.Recipient);
        if (recipient is null)
        {
            _logger.LogInformation("{} tried to send {} a message but the recipient is offline: {}", 
                user.Username, packet.Recipient, packet.Text);
            return;
        }
        
        recipient.PacketQueue.WriteSendMessage(user.Username, packet.Text, packet.Recipient, user.User.Id);
        _logger.LogInformation("{} -> {}: {}", user.Username, packet.Recipient, packet.Text);
    }
}