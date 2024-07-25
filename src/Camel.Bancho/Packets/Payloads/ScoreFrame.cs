namespace Camel.Bancho.Packets.Payloads;

public readonly struct ScoreFrame : IPacketPayload
{
    public int Time { get; }
    public byte MultiSlotId { get; init; }
    public ushort Count300 { get; }
    public ushort Count100 { get; }
    public ushort Count50 { get; }
    public ushort CountGeki { get; }
    public ushort CountKatu { get; }
    public ushort CountMiss { get; }
    public int TotalScore { get; }
    public ushort MaxCombo { get; }
    public ushort CurrentCombo { get; }
    public bool Perfect { get; }
    public byte CurrentHp { get; }
    public byte TagByte { get; }
    public bool ScoreV2 { get; }
    public double? ComboPortion { get; }
    public double? BonusPortion { get; }

    public ScoreFrame(int time, byte multiSlotId,
        ushort count300, ushort count100, ushort count50, ushort countGeki, ushort countKatu, ushort countMiss,
        int totalScore, ushort maxCombo, ushort currentCombo, bool perfect, byte currentHp,
        byte tagByte, bool scoreV2, double? comboPortion, double? bonusPortion)
    {
        Time = time;
        MultiSlotId = multiSlotId;
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
        writer.Write(MultiSlotId);
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