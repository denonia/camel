using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Client;

namespace Camel.Bancho.Packets.Handlers;

[PacketHandler(PacketType.ClientSpectateFrames)]
public class SpectateFramesHandler : IPacketHandler<SpectateFramesPacket>
{
    public async Task HandleAsync(SpectateFramesPacket packet, UserSession userSession)
    {
        var framesPacket = new Server.SpectateFramesPacket(packet.FrameBundle);

        foreach (var spectator in userSession.Spectators)
        {
            spectator.PacketQueue.WritePacket(framesPacket);
        }
    }
}