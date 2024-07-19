using System.Text;

namespace Camel.Bancho.Packets;

public class PacketBinaryReader : BinaryReader
{
    public PacketBinaryReader(Stream input) : base(input)
    {
    }

    public PacketBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
    {
    }

    public PacketBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
    {
    }

    public override string ReadString()
    {
        BaseStream.ReadByte();
        var length = BaseStream.ReadLEB128Unsigned();
        var bytes = new byte[length];
        BaseStream.Read(bytes, 0, (int)length);
        return Encoding.UTF8.GetString(bytes.AsSpan());
    }
}