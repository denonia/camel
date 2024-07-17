using Camel.Bancho.Enums;
using Camel.Core.Enums;

namespace Camel.Bancho.Packets.Client;

public readonly struct ChangeActionPacket
{
    public ClientAction Action { get; }
    public string InfoText { get; }
    public string MapMd5 { get; }
    public int Mods { get; }
    public GameMode Mode { get; }
    
    public ChangeActionPacket(ClientAction action, string infoText, string mapMd5, int mods, GameMode mode)
    {
        Action = action;
        InfoText = infoText;
        MapMd5 = mapMd5;
        Mods = mods;
        Mode = mode;
    }
    
    public static ChangeActionPacket ReadFromStream(Stream stream)
    {
        return new ChangeActionPacket(
            (ClientAction)stream.ReadByte(),
            stream.ReadBanchoString(),
            stream.ReadBanchoString(),
            stream.ReadInt32(),
            (GameMode)stream.ReadByte());
    }
}