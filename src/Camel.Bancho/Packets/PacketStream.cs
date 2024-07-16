
using System.Text;

namespace Camel.Bancho.Packets;

public class PacketStream : IDisposable, IAsyncDisposable
{
    private readonly Stream _stream;
    private readonly BinaryWriter _binaryWriter;

    public PacketStream(Stream stream)
    {
        _stream = stream;
        _binaryWriter = new BinaryWriter(_stream);
    }

    public void Write(Packet packet)
    {
        _binaryWriter.Write((short)packet.Type);
        _binaryWriter.Write((byte)0);
        _binaryWriter.Write((int)packet.Data.Length);
        _binaryWriter.Write(packet.Data);
    }

    public void WriteNotification(string text)
    {
        var ms = new MemoryStream();
        using var w = new BinaryWriter(ms);
        
        w.Write((byte)0x0b);
        var str = Encoding.UTF8.GetBytes(text);
        ms.WriteLEB128Unsigned((ulong)text.Length);
        w.Write(str);
        
        var packet = new Packet
        {
            Type = PacketType.ServerNotification,
            Data = ms.ToArray()
        };
        Write(packet);
    }

    public void WriteUserId(int id)
    {
        var packet = new Packet
        {
            Type = PacketType.ServerUserId,
            Data = BitConverter.GetBytes(id)
        };
        Write(packet);
    }

    public void Dispose()
    {
        _binaryWriter.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _binaryWriter.DisposeAsync();
    }
}