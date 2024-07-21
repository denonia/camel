using Camel.Bancho.Enums;
using Camel.Bancho.Packets.Multiplayer;

namespace Camel.Bancho.Packets.Server;

public readonly struct MatchJoinSuccessPacket : IPacket
{
    public PacketType Type => PacketType.ServerMatchJoinSuccess;

    public MatchState Match { get; }

    public MatchJoinSuccessPacket(MatchState match)
    {
        Match = match;
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        Match.WriteToStream(writer, true);
    }
}
