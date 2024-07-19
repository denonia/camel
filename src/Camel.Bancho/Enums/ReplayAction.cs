namespace Camel.Bancho.Enums;

public enum ReplayAction : byte
{
    Standard = 0,
    NewSong = 1,
    Skip = 2,
    Completion = 3,
    Fail = 4,
    Pause = 5,
    Unpause = 6,
    SongSelect = 7,
    WatchingOther = 8
}