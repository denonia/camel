using System.Diagnostics;
using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Spectating;

public readonly struct ReplayFrameBundle
{
    public int Extra { get; }
    public ushort FrameCount { get; }
    public IEnumerable<ReplayFrame> ReplayFrames { get; }
    public ReplayAction Action { get; }
    public ScoreFrame ScoreFrame { get; }
    public ushort Sequence { get; }

    public ReplayFrameBundle(int extra, ushort frameCount, IEnumerable<ReplayFrame> replayFrames,
        ReplayAction action, ScoreFrame scoreFrame, ushort sequence)
    {
        Extra = extra;
        FrameCount = frameCount;
        ReplayFrames = replayFrames;
        Action = action;
        ScoreFrame = scoreFrame;
        Sequence = sequence;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(Extra);
        writer.Write(FrameCount);
        foreach (var frame in ReplayFrames)
            frame.WriteToStream(writer);
        writer.Write((byte)Action);
        ScoreFrame.WriteToStream(writer);
        writer.Write(Sequence);
    }

    public static ReplayFrameBundle ReadFromStream(PacketBinaryReader reader)
    {
        var extra = reader.ReadInt32();
        var frameCount = reader.ReadUInt16();
        var frames = Enumerable.Range(0, frameCount).Select(_ => ReplayFrame.ReadFromStream(reader)).ToList();
        var action = (ReplayAction)reader.ReadByte();
        var scoreFrame = ScoreFrame.ReadFromStream(reader);
        var sequence = reader.ReadUInt16();

        Debug.Assert(reader.BaseStream.Length == reader.BaseStream.Position);

        return new ReplayFrameBundle(extra, frameCount, frames, action, scoreFrame, sequence);
    }
}