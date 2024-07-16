using Camel.Bancho.Packets.Client;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientSendPublicMessage)]
public class SendPublicMessageHandler : IPacketHandler
{
    private readonly ILogger<SendPublicMessageHandler> _logger;

    public SendPublicMessageHandler(ILogger<SendPublicMessageHandler> logger)
    {
        _logger = logger;
    }

    public void Handle(Stream stream)
    {
        var packet = SendPublicMessagePacket.ReadFromStream(stream);

        _logger.LogInformation("Message in {}: {}", packet.Recipient, packet.Text);
    }
}