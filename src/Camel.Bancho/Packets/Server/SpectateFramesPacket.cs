using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct SpectateFramesPacket : IPacket
{
    public PacketType Type => PacketType.ServerSpectateFrames;

    public byte[] Data { get; }

    public SpectateFramesPacket(byte[] data)
    {
        Data = data;
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write((ushort)15);
        writer.Write((byte)0);
        writer.Write((uint)Data.Length);
        writer.Write(Data);
    }
}