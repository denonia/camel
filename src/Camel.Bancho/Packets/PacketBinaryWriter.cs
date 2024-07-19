using System.Buffers;
using System.Text;
using Microsoft.Toolkit.HighPerformance;

namespace Camel.Bancho.Packets;

public class PacketBinaryWriter : BinaryWriter
{
    public PacketBinaryWriter(Stream output) : base(output)
    {
    }

    public override void Write(string? value)
    {
        OutStream.WriteByte(0x0b);
        
        if (value == null)
        {
            OutStream.WriteLEB128Unsigned(0);
        }
        else
        {
            var arr = ArrayPool<byte>.Shared.Rent(value.Length * 3);
            var bytes = Encoding.UTF8.GetBytes(value.AsSpan(), arr.AsSpan());
            OutStream.WriteLEB128Unsigned((ulong)value.Length);
            OutStream.Write(arr, 0, bytes);
            ArrayPool<byte>.Shared.Return(arr);
        }
    }

    public void Write(IPacket packet)
    {
        // don't know the packet body length beforehand
        // so we just return back and write it lmao
        OutStream.Write((short)packet.Type);
        OutStream.WriteByte(0);
        
        var lengthPos = OutStream.Position;
        OutStream.Write((int)0);
        
        packet.WriteToStream(this);
        var endPos = OutStream.Position;
        
        var dataLength = OutStream.Position - lengthPos - 4;
        OutStream.Position = lengthPos;
        OutStream.Write((int)dataLength);
        OutStream.Position = endPos;
    }
}