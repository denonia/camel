using Camel.Bancho.Enums;
using Camel.Bancho.Packets.Multiplayer;

namespace Camel.Bancho.Packets.Server;

public readonly struct NewMatchPacket : IPacket
{
    public PacketType Type => PacketType.ServerNewMatch;

    public MatchState Match { get; }
    
    public NewMatchPacket(MatchState match)
    {
        Match = match;
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        Match.WriteToStream(writer);
    }
}