using System.Buffers;
using System.Text;

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
}