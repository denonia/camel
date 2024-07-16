using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct RestartPacket : IWritePacket
{
    public int DelayMs { get; }
    
    public RestartPacket(int delayMs)
    {
        DelayMs = delayMs;
    }

    public void WriteToStream(IPacketStream stream)
    {
        var packet = new Packet(PacketType.ServerRestart, BitConverter.GetBytes(DelayMs));
        stream.Write(packet);
    }
}