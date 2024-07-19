
using System.Text;
using Camel.Bancho.Enums;
using Microsoft.Toolkit.HighPerformance;

namespace Camel.Bancho.Packets;

public class PacketStream : IPacketStream, IDisposable, IAsyncDisposable
{
    private readonly Stream _stream;
    private readonly PacketBinaryWriter _binaryWriter;

    public PacketStream(Stream stream)
    {
        _stream = stream;
        _binaryWriter = new PacketBinaryWriter(stream);
    }
    
    public bool AtEnd => _stream.Position >= _stream.Length;

    public IEnumerable<Packet> ReadAll()
    {
        while (!AtEnd)
        {
            yield return Read();
        }
    }

    public void Write(IPacket packet)
    {
        // don't know the packet body length beforehand
        // so we just return back and write it lmao
        _stream.Write((short)packet.Type);
        _stream.WriteByte(0);
        
        var lengthPos = _stream.Position;
        _stream.Write((int)0);
        
        packet.WriteToStream(_binaryWriter);
        var endPos = _stream.Position;
        
        var dataLength = _stream.Position - lengthPos - 4;
        _stream.Position = lengthPos;
        _stream.Write((int)dataLength);
        _stream.Position = endPos;
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
        // _stream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        // await _stream.DisposeAsync();
    }
}