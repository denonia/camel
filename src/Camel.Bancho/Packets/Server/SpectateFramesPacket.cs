using Camel.Bancho.Enums;
using Camel.Bancho.Packets.Client.Spectating;

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
        writer.Write((ushort)15);
        writer.Write((byte)0);

        writer.Write(FrameBundle.Length);
        FrameBundle.WriteToStream(writer);
    }
}
    