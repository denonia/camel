using Camel.Bancho.Packets.Multiplayer;

namespace Camel.Bancho.Packets.Client;

public readonly struct CreateMatchPacket
{
    public MatchState Match { get; }

    public CreateMatchPacket(MatchState match)
    {
        Match = match;
    }

    public static CreateMatchPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new CreateMatchPacket(MatchState.ReadFromStream(reader));
    }
}