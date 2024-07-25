using Camel.Bancho.Enums;
using Camel.Core.Enums;

namespace Camel.Bancho.Packets.Payloads;

public readonly struct ChangeAction
{
    public ClientAction Action { get; }
    public string InfoText { get; }
    public string MapMd5 { get; }
    public int Mods { get; }
    public GameMode Mode { get; }
    public int BeatmapId { get; }
    
    public ChangeAction(ClientAction action, string infoText, string mapMd5, int mods, GameMode mode, int beatmapId)
    {
        Action = action;
        InfoText = infoText;
        MapMd5 = mapMd5;
        Mods = mods;
        Mode = mode;
        BeatmapId = beatmapId;
    }
    
    public static ChangeAction ReadFromStream(PacketBinaryReader reader)
    {
        return new ChangeAction(
            (ClientAction)reader.ReadByte(),
            reader.ReadString(),
            reader.ReadString(),
            reader.ReadInt32(),
            (GameMode)reader.ReadByte(),
            reader.ReadInt32());
    }
}