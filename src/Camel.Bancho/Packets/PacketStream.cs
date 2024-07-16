
using System.Text;
using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets;

public class PacketStream : IPacketStream, IDisposable, IAsyncDisposable
{
    private readonly Stream _stream;

    public PacketStream(Stream stream)
    {
        _stream = stream;
    }
    
    public bool AtEnd => _stream.Position >= _stream.Length;

    public void Write(Packet packet)
    {
        _stream.Write((short)packet.Type);
        _stream.WriteByte(0);
        _stream.Write(packet.Data.Length);
        _stream.Write(packet.Data);
    }

    public Packet Read()
    {
        using var br = new BinaryReader(_stream, Encoding.Default, true);
        var type = (PacketType) br.ReadInt16();
        br.ReadByte();
        var length = br.ReadInt32();
        var data = br.ReadBytes(length);
        return new Packet(type, data);
    }

    public void Dispose()
    {
        _stream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _stream.DisposeAsync();
    }
}