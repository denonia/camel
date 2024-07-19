namespace Camel.Bancho.Packets.Client.Spectating;

public readonly struct ReplayFrame
{
    public byte ButtonState { get; }
    public byte TaikoByte { get; }
    public float X { get; }
    public float Y { get; }
    public int Time { get; }

    public ReplayFrame(byte buttonState, byte taikoByte, float x, float y, int time)
    {
        ButtonState = buttonState;
        TaikoByte = taikoByte;
        X = x;
        Y = y;
        Time = time;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(ButtonState);
        writer.Write(TaikoByte);
        writer.Write(X);
        writer.Write(Y);
        writer.Write(Time);
    }

    public static ReplayFrame ReadFromStream(PacketBinaryReader reader)
    {
        return new ReplayFrame(reader.ReadByte(), reader.ReadByte(),
            reader.ReadSingle(), reader.ReadSingle(),
            reader.ReadInt32());
    }
}