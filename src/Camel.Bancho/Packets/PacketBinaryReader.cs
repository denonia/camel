using System.Text;
using Camel.Bancho.Enums;

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
        BaseStream.ReadExactly(bytes, 0, (int)length);
        return Encoding.UTF8.GetString(bytes.AsSpan());
    }

    public int[] ReadInt32Array()
    {
        var length = ReadInt16();

        var result = new int[length];
        for (var i = 0; i < length; i++)
            result[i] = ReadInt32();

        return result;
    }
    
    public Packet ReadPacket()
    {
        var type = (PacketType) ReadInt16();
        ReadByte();
        var length = ReadInt32();
        var data = ReadBytes(length);
        return new Packet(type, data);
    }
}