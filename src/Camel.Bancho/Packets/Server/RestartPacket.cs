using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct RestartPacket : IPacket
{
    public PacketType Type => PacketType.ServerRestart;
    
    public int DelayMs { get; }
    
    public RestartPacket(int delayMs)
    {
        DelayMs = delayMs;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(DelayMs);
    }
}