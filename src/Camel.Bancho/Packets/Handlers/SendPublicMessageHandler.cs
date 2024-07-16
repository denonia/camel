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

    public void Handle(SendPublicMessagePacket packet, UserContext userContext)
    {
        _logger.LogInformation("[{}] {}: {}", packet.Recipient, userContext.Username, packet.Text);
        
        userContext.PacketWriter.WriteNotification(packet.Text);
    }
}