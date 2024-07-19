using Camel.Bancho.Enums;
using Camel.Bancho.Packets.Client.Spectating;

namespace Camel.Bancho.Packets.Client;

public readonly struct SpectateFramesPacket
{
    public int Extra { get; }
    public ushort FrameCount { get; }
    public IEnumerable<ReplayFrame> ReplayFrames { get; }
    public ReplayAction Action { get; }
    public ScoreFrame ScoreFrame { get; }
    public ushort Sequence { get; }

    public SpectateFramesPacket(int extra, ushort frameCount, IEnumerable<ReplayFrame> replayFrames,
        ReplayAction action, ScoreFrame scoreFrame, ushort sequence)
    {
        Extra = extra;
        FrameCount = frameCount;
        ReplayFrames = replayFrames;
        Action = action;
        ScoreFrame = scoreFrame;
        Sequence = sequence;
    }

    public static SpectateFramesPacket ReadFromStream(PacketBinaryReader reader)
    {
        var extra = reader.ReadInt32();
        var frameCount = reader.ReadUInt16();
        var frames = Enumerable.Range(0, frameCount).Select(_ => ReplayFrame.ReadFromStream(reader));
        var action = (ReplayAction)reader.ReadByte();
        var scoreFrame = ScoreFrame.ReadFromStream(reader);
        var sequence = reader.ReadUInt16();
        return new SpectateFramesPacket(extra, frameCount, frames, action, scoreFrame, sequence);
    }
}