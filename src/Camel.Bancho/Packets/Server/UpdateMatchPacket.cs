using Camel.Bancho.Enums;
using Camel.Bancho.Packets.Multiplayer;

namespace Camel.Bancho.Packets.Server;

public readonly struct UpdateMatchPacket : IPacket
{
    public PacketType Type => PacketType.ServerUpdateMatch;

    public MatchState Match { get; }
    
    public UpdateMatchPacket(MatchState match)
    {
        Match = match;
    }
    
    public void WriteToStream(PacketBinaryWriter writer)
    {
        Match.WriteToStream(writer, true);
    }
}