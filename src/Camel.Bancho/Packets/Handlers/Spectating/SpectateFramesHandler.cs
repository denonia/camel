using Camel.Bancho.Enums;
using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;

namespace Camel.Bancho.Packets.Handlers.Spectating;

[PacketHandler(PacketType.ClientSpectateFrames)]
public class SpectateFramesHandler : IPacketHandler<ReplayFrameBundle>
{
    public Task HandleAsync(ReplayFrameBundle frameBundle, UserSession userSession)
    {
        foreach (var spectator in userSession.Spectators)
        {
            spectator.PacketQueue.WriteSpectateFrames(frameBundle);
        }

        return Task.CompletedTask;
    }
}