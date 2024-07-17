using Camel.Bancho.Enums;
using Camel.Core.Enums;

namespace Camel.Bancho.Models;

public readonly struct UserStatus
{
    public ClientAction Action { get; } = ClientAction.Idle;
    public string InfoText { get; }
    public string MapMd5 { get; }
    public int Mods { get; }
    public GameMode Mode { get; } = GameMode.Standard;
    public int MapId { get; }

    public UserStatus(ClientAction action, string infoText, string mapMd5, int mods, GameMode mode, int mapId)
    {
        Action = action;
        InfoText = infoText;
        MapMd5 = mapMd5;
        Mods = mods;
        Mode = mode;
        MapId = mapId;
    }
}