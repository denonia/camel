namespace Camel.Bancho.Packets.Payloads;

public readonly struct MatchJoin
{
    public int MatchId { get; }
    public string Password { get; }
    
    public MatchJoin(int matchId, string password)
    {
        MatchId = matchId;
        Password = password;
    }

    public static MatchJoin ReadFromStream(PacketBinaryReader reader)
    {
        return new MatchJoin(reader.ReadInt32(), reader.ReadString());
    }
}