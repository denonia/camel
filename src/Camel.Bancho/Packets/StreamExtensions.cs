using System.Text;

namespace Camel.Bancho.Packets;

public static class StreamExtensions
{
    public static void WriteBanchoString(this Stream stream, string str)
    {
        stream.WriteByte(0x0b);
        stream.WriteLEB128Unsigned((ulong)str.Length);
        stream.Write(Encoding.UTF8.GetBytes(str));
    }

    public static string ReadBanchoString(this Stream stream)
    {
        stream.ReadByte();
        var length = stream.ReadLEB128Unsigned();
        var bytes = new byte[length];
        stream.Read(bytes, 0, (int)length);
        return Encoding.UTF8.GetString(bytes);
    }
    
    public static int ReadInt16(this Stream stream)
    {
        var bytes = new byte[2];
        stream.Read(bytes, 0, 2);
        return BitConverter.ToInt16(bytes);
    }

    public static int ReadInt32(this Stream stream)
    {
        var bytes = new byte[4];
        stream.Read(bytes, 0, 4);
        return BitConverter.ToInt32(bytes);
    }
    
    public static void Write(this Stream stream, short num) =>
        stream.Write(BitConverter.GetBytes(num));

    public static void Write(this Stream stream, int num) =>
        stream.Write(BitConverter.GetBytes(num));
    
    public static void Write(this Stream stream, float num) =>
        stream.Write(BitConverter.GetBytes(num));
    
    public static void Write(this Stream stream, long num) =>
        stream.Write(BitConverter.GetBytes(num));
}