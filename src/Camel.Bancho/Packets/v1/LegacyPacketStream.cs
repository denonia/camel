using System.IO.Compression;
using Camel.Bancho.Enums;
using osu.Game.IO.Legacy;

namespace Camel.Bancho.Packets.v1;

public class LegacyPacketStream : IPacketStream
{
    private readonly Stream _stream;

    public LegacyPacketStream(Stream stream)
    {
        _stream = stream;
    }
    
    public bool AtEnd => _stream.Position >= _stream.Length;

    public IEnumerable<Packet> ReadAll()
    {
        while (!AtEnd)
        {
            yield return Read();
        }
    }
    
    public void Write(Packet packet)
    {
        var writer = new SerializationWriter(_stream, true);
        writer.Write((ushort)packet.Type);

        using var compressedStream = new MemoryStream();
        
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
        {
            zipStream.Write(packet.Data, 0, packet.Data.Length);
        }
        var arr = compressedStream.ToArray();
        writer.Write((uint)arr.Length);
        // HAHHAHAHHAHAHAHAHHAHAHHA
        _stream.Write(arr);
    }

    public Packet Read()
    {
        var reader = new SerializationReader(_stream);
        var type = (PacketType) reader.ReadUInt16();
        var length = reader.ReadUInt32();
        var data = reader.ReadBytes((int)length);
        return new Packet(type, data);
    }
}