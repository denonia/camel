using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Client;

public readonly struct StartSpectatingPacket
{
    public int TargetUserId { get; }

    public StartSpectatingPacket(int targetUserId)
    {
        TargetUserId = targetUserId;
    }

    public static StartSpectatingPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new StartSpectatingPacket(reader.ReadInt32());
    }
}