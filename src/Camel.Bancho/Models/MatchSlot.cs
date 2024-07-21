using Camel.Bancho.Enums.Multiplayer;
using Camel.Core.Enums;

namespace Camel.Bancho.Models;

public class MatchSlot
{
    public UserSession? User { get; set; }
    public SlotStatus Status { get; set; }
    public Team Team { get; set; }
    public Mods Mods { get; set; } = Mods.NoMod;

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

    public void Reset()
    {
        User = null;
        Status = SlotStatus.Open;
        Team = Team.Neutral;
        Mods = Mods.NoMod;
    }
}