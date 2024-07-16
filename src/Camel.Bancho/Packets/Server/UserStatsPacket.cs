using Camel.Bancho.Enums;
using Camel.Core.Enums;

namespace Camel.Bancho.Packets.Server;

public readonly struct UserStatsPacket : IWritePacket
{
    public int Id { get; }
    public ClientAction Action { get; }
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

    public UserStatsPacket(int id, ClientAction action, string infoText, string mapMd5, int mods, GameMode mode, int mapId,
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
        
        ms.Write(Id);
        ms.WriteByte((byte)Action);
        ms.WriteBanchoString(InfoText);
        ms.WriteBanchoString(MapMd5);
        ms.Write(Mods);
        ms.WriteByte((byte)Mode);
        ms.Write(MapId);
        ms.Write(RankedScore);
        ms.Write(Accuracy);
        ms.Write(Plays);
        ms.Write(TotalScore);
        ms.Write(Rank);
        ms.Write(Pp);
        
        var packet = new Packet(PacketType.ServerUserStats, ms.ToArray());
        stream.Write(packet);
    }
}