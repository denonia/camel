using Camel.Bancho.Enums;
using Camel.Bancho.Packets.Multiplayer;

namespace Camel.Bancho.Packets.Server;

public readonly struct MatchJoinFailPacket : IPacket
{
    public PacketType Type => PacketType.ServerMatchJoinFail;

    public void WriteToStream(PacketBinaryWriter writer)
    {
    }
}
