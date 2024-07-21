using Camel.Bancho.Enums.Multiplayer;
using Camel.Bancho.Packets.Multiplayer;
using Camel.Core.Enums;

namespace Camel.Bancho.Models;

public class Match
{
    public short Id { get; }
    public bool InProgress { get; set; } = false;
    public bool PowerPlay { get; } = false;
    public Mods Mods { get; set; } = Mods.NoMod;
    public string Name { get; set; }
    public string? Password { get; }
    public bool MatchHistoryPublic { get; }

    public string MapName { get; }
    public int MapId { get; }
    public string MapMd5 { get; }

    public List<MatchSlot> Slots { get; set; } = [];
    public UserSession Host { get; set; }

    public GameMode Mode { get; }
    public WinCondition WinCondition { get; set; } = WinCondition.Score;
    public TeamType TeamType { get; set; } = TeamType.HeadToHead;
    public bool FreeMods { get; } = false;
    public int Seed { get; }

    public Match(short id, string name, string? password, bool matchHistoryPublic, string mapName, int mapId,
        string mapMd5, UserSession host, GameMode mode)
    {
        Id = id;
        Name = name;
        Password = password;
        MatchHistoryPublic = matchHistoryPublic;
        MapName = mapName;
        MapId = mapId;
        MapMd5 = mapMd5;
        Host = host;
        Mode = mode;

        Seed = Random.Shared.Next();

        Slots.Add(new MatchSlot(host, SlotStatus.NotReady, Team.Neutral));

        foreach (var _ in Enumerable.Range(0, 6))
            Slots.Add(new MatchSlot(null, SlotStatus.Open, Team.Neutral));
        foreach (var _ in Enumerable.Range(0, 9))
            Slots.Add(new MatchSlot(null, SlotStatus.Locked, Team.Neutral));
    }

    public MatchState State => new MatchState(Id, InProgress, PowerPlay, Mods, Name, Password, MapName, MapId, MapMd5,
        Slots.Select(s => s.Status), Slots.Select(s => s.Team),
        Slots.Where(s => s.User is not null).Select(s => s.User!.User.Id),
        Host.User.Id, Mode, WinCondition, TeamType, FreeMods, Slots.Select(s => s.Mods), Seed);
}