using Camel.Bancho.Enums;
using Camel.Core.Enums;
using osu.Game.IO.Legacy;

namespace Camel.Bancho.Packets.v1.Server;

public readonly struct UserStatsPacket : IWritePacket
{
    public int Id { get; }
    public string UserName { get; }
    public long RankedScore { get; }
    public double Accuracy { get; }
    public int Plays { get; }
    public long TotalScore { get; }
    public int Rank { get; }
    public string AvatarFileName { get; }

    // int?
    public ClientAction Action { get; }
    public string InfoText { get; }
    public string BeatmapMd5 { get; }
    public Mods Mods { get; }
    
    public int Timezone { get; }
    public string Location { get; }

    public UserStatsPacket(int id, string userName, long rankedScore, double accuracy, int plays, long totalScore, int rank, string avatarFileName, ClientAction action, string infoText, string beatmapMd5, Mods mods, int timezone, string location)
    {
        Id = id;
        UserName = userName;
        RankedScore = rankedScore;
        Accuracy = accuracy;
        Plays = plays;
        TotalScore = totalScore;
        Rank = rank;
        AvatarFileName = avatarFileName;
        Action = action;
        InfoText = infoText;
        BeatmapMd5 = beatmapMd5;
        Mods = mods;
        Timezone = timezone;
        Location = location;
    }

    public void WriteToStream(IPacketStream stream)
    {
        using var ms = new MemoryStream();

        using var serializationWriter = new SerializationWriter(ms, true);
        serializationWriter.Write(Id);
        serializationWriter.Write(UserName);
        serializationWriter.Write(RankedScore);
        serializationWriter.Write(Accuracy);
        serializationWriter.Write(Plays);
        serializationWriter.Write(TotalScore);
        serializationWriter.Write(Rank);
        serializationWriter.Write(AvatarFileName);
        
        serializationWriter.Write((byte) Action);
        if (Action != ClientAction.Unknown)
        {
            serializationWriter.Write(InfoText);
            serializationWriter.Write(BeatmapMd5);
            serializationWriter.Write((ushort) Mods);
        }
        
        serializationWriter.Write((byte) (Timezone + 24));
        serializationWriter.Write(Location);
        
        var packet = new Packet((PacketType)12, ms.ToArray());
        stream.Write(packet);
    }
}