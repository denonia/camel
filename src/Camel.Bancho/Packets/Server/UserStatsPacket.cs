using Camel.Bancho.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct UserStatsPacket : IWritePacket
{
    public int Id { get; }
    public byte Action { get; }
    public string InfoText { get; }
    public string MapMd5 { get; }
    public int Mods { get; }
    public GameMode Mode { get; }
    public int MapId { get; }
    public long RankedScore { get; }
    public float Accuracy { get; }
    public int Plays { get; }
    public long TotalScore { get; }
    public int Rank { get; }
    public short Pp { get; }

    public UserStatsPacket(int id, byte action, string infoText, string mapMd5, int mods, GameMode mode, int mapId,
        long rankedScore, float accuracy, int plays, long totalScore, int rank, short pp)
    {
        Id = id;
        Action = action;
        InfoText = infoText;
        MapMd5 = mapMd5;
        Mods = mods;
        Mode = mode;
        MapId = mapId;
        RankedScore = rankedScore;
        Accuracy = accuracy;
        Plays = plays;
        TotalScore = totalScore;
        Rank = rank;
        Pp = pp;
    }

    public void WriteToStream(IPacketStream stream)
    {
        using var ms = new MemoryStream();
        
        ms.Write(BitConverter.GetBytes(Id));
        ms.WriteByte(Action);
        ms.WriteBanchoString(InfoText);
        ms.WriteBanchoString(MapMd5);
        ms.Write(BitConverter.GetBytes(Mods));
        ms.WriteByte((byte)Mode);
        ms.Write(BitConverter.GetBytes(MapId));
        ms.Write(BitConverter.GetBytes(RankedScore));
        ms.Write(BitConverter.GetBytes(Accuracy));
        ms.Write(BitConverter.GetBytes(Plays));
        ms.Write(BitConverter.GetBytes(TotalScore));
        ms.Write(BitConverter.GetBytes(Rank));
        ms.Write(BitConverter.GetBytes(Pp));
        
        var packet = new Packet(PacketType.ServerUserStats, ms.ToArray());
        stream.Write(packet);
    }
}