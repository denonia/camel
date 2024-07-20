using Camel.Bancho.Enums;
using Camel.Bancho.Packets.Spectating;

namespace Camel.Bancho.Packets.Server;

public readonly struct SpectateFramesPacket : IPacket
{
    public PacketType Type => PacketType.ServerSpectateFrames;

    public ReplayFrameBundle FrameBundle { get; }

    public SpectateFramesPacket(ReplayFrameBundle frameBundle)
    {
        FrameBundle = frameBundle;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        FrameBundle.WriteToStream(writer);
    }
}
    