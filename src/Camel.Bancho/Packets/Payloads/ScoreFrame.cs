namespace Camel.Bancho.Packets.Payloads;

public readonly struct ScoreFrame
{
    private int Time { get; }
    private byte Id { get; }
    private ushort Count300 { get; }
    private ushort Count100 { get; }
    private ushort Count50 { get; }
    private ushort CountGeki { get; }
    private ushort CountKatu { get; }
    private ushort CountMiss { get; }
    private int TotalScore { get; }
    private ushort MaxCombo { get; }
    private ushort CurrentCombo { get; }
    private bool Perfect { get; }
    private byte CurrentHp { get; }
    private byte TagByte { get; }
    private bool ScoreV2 { get; }
    private double? ComboPortion { get; }
    private double? BonusPortion { get; }

    public ScoreFrame(int time, byte id,
        ushort count300, ushort count100, ushort count50, ushort countGeki, ushort countKatu, ushort countMiss,
        int totalScore, ushort maxCombo, ushort currentCombo, bool perfect, byte currentHp,
        byte tagByte, bool scoreV2, double? comboPortion, double? bonusPortion)
    {
        Time = time;
        Id = id;
        Count300 = count300;
        Count100 = count100;
        Count50 = count50;
        CountGeki = countGeki;
        CountKatu = countKatu;
        CountMiss = countMiss;
        TotalScore = totalScore;
        MaxCombo = maxCombo;
        CurrentCombo = currentCombo;
        Perfect = perfect;
        CurrentHp = currentHp;
        TagByte = tagByte;
        ScoreV2 = scoreV2;
        ComboPortion = comboPortion;
        BonusPortion = bonusPortion;
    }

    public void WriteToStream(PacketBinaryWriter writer)
    {
        writer.Write(Time);
        writer.Write(Id);
        writer.Write(Count300);
        writer.Write(Count100);
        writer.Write(Count50);
        writer.Write(CountGeki);
        writer.Write(CountKatu);
        writer.Write(CountMiss);
        writer.Write(TotalScore);
        writer.Write(MaxCombo);
        writer.Write(CurrentCombo);
        writer.Write(Perfect);
        writer.Write(CurrentHp);
        writer.Write(TagByte);
        writer.Write(ScoreV2);
        
        if (ScoreV2)
        {
            writer.Write((double)ComboPortion!);
            writer.Write((double)BonusPortion!);
        }
    }

    public static ScoreFrame ReadFromStream(PacketBinaryReader reader)
    {
        var time = reader.ReadInt32();
        var id = reader.ReadByte();
        var count300 = reader.ReadUInt16();
        var count100 = reader.ReadUInt16();
        var count50 = reader.ReadUInt16();
        var countGeki = reader.ReadUInt16();
        var countKatu = reader.ReadUInt16();
        var countMiss = reader.ReadUInt16();
        var totalScore = reader.ReadInt32();
        var maxCombo = reader.ReadUInt16();
        var currentCombo = reader.ReadUInt16();
        var perfect = reader.ReadBoolean();
        var currentHp = reader.ReadByte();
        var tagByte = reader.ReadByte();
        var scoreV2 = reader.ReadBoolean();
        double? comboPortion = scoreV2 ? reader.ReadDouble() : null;
        double? bonusPortion = scoreV2 ? reader.ReadDouble() : null;
        return new ScoreFrame(time, id, count300, count100, count50, countGeki, countKatu, countMiss,
            totalScore, maxCombo, currentCombo, perfect, currentHp, tagByte, scoreV2, comboPortion, bonusPortion);
    }
}