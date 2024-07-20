using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Client.Spectating;

public readonly struct ReplayFrameBundle
{
    public uint Length { get; }
    public int Extra { get; }
    public ushort FrameCount { get; }
    public IEnumerable<ReplayFrame> ReplayFrames { get; }
    public ReplayAction Action { get; }
    public ScoreFrame ScoreFrame { get; }
    public ushort Sequence { get; }

    public ReplayFrameBundle(uint length, int extra, ushort frameCount, IEnumerable<ReplayFrame> replayFrames,
        ReplayAction action, ScoreFrame scoreFrame, ushort sequence)
    {
        Length = length;
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
        var length = (uint)reader.BaseStream.Length;
        var extra = reader.ReadInt32();
        var frameCount = reader.ReadUInt16();
        var frames = Enumerable.Range(0, frameCount).Select(_ => ReplayFrame.ReadFromStream(reader));
        var action = (ReplayAction)reader.ReadByte();
        var scoreFrame = ScoreFrame.ReadFromStream(reader);
        var sequence = reader.ReadUInt16();
        return new ReplayFrameBundle(length, extra, frameCount, frames, action, scoreFrame, sequence);
    }
}