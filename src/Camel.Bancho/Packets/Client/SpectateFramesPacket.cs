using System.Buffers;
using Camel.Bancho.Enums;
using Camel.Bancho.Packets.Client.Spectating;

namespace Camel.Bancho.Packets.Client;

public readonly struct SpectateFramesPacket
{
    public ReplayFrameBundle FrameBundle { get; }

    public SpectateFramesPacket(ReplayFrameBundle frameBundle)
    {
        FrameBundle = frameBundle;
    }

    public static SpectateFramesPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new SpectateFramesPacket(ReplayFrameBundle.ReadFromStream(reader));
    }
}