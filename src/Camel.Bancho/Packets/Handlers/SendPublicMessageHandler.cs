using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientSendPublicMessage)]
public class SendPublicMessageHandler : IPacketHandler<SendPublicMessagePacket>
{
    private readonly ILogger<SendPublicMessageHandler> _logger;

    public SendPublicMessageHandler(ILogger<SendPublicMessageHandler> logger)
    {
        _logger = logger;
    }

    public void Handle(SendPublicMessagePacket packet, UserSession user)
    {
        _logger.LogInformation("[{}] {}: {}", packet.Recipient, user.Username, packet.Text);
        
        user.PacketQueue.WriteNotification($"{user.Username}: {packet.Text}");
    }
}