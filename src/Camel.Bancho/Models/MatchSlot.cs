using Camel.Bancho.Enums.Multiplayer;
using Camel.Core.Enums;

namespace Camel.Bancho.Models;

public class MatchSlot
{
    public UserSession? User { get; set; }
    public SlotStatus Status { get; set; }
    public Team Team { get; set; }
    public Mods Mods { get; set; } = Mods.NoMod;

    public bool Loaded { get; set; }
    public bool Skipped { get; set; }

    public bool HasPlayer => User != null;

    public MatchSlot(UserSession? user, SlotStatus status, Team team)
    {
        User = user;
        Status = status;
        Team = team;
    }

    public MatchSlot(MatchSlot other)
    {
        User = other.User;
        Status = other.Status;
        Team = other.Team;
        Mods = other.Mods;
    }

    public void Reset(SlotStatus status = SlotStatus.Open)
    {
        User = null;
        Status = status;
        Team = Team.Neutral;
        Mods = Mods.NoMod;
    }

    public void MapEnded()
    {
        Status = SlotStatus.NotReady;
        Loaded = false;
        Skipped = false;
    }
}