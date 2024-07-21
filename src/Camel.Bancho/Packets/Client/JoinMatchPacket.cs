namespace Camel.Bancho.Packets.Client;

public readonly struct JoinMatchPacket
{
    public int MatchId { get; }
    public string Password { get; }
    
    public JoinMatchPacket(int matchId, string password)
    {
        MatchId = matchId;
        Password = password;
    }

    public static JoinMatchPacket ReadFromStream(PacketBinaryReader reader)
    {
        return new JoinMatchPacket(reader.ReadInt32(), reader.ReadString());
    }
}