using Camel.Bancho.Enums.Multiplayer;
using Camel.Bancho.Packets.Payloads;
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

    public int FreeSlots => Slots.Count(s => (s.Status & SlotStatus.Open) != 0);

    public IEnumerable<UserSession> Players =>
        Slots.Where(s => (s.Status & SlotStatus.HasPlayer) != 0).Select(s => s.User!);

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
        Host.User.Id, Mode, WinCondition, TeamType, FreeMods, Slots.Select(s => s.Mods), Seed, true);

    private MatchSlot NextFreeSlot() => Slots.First(s => (s.Status & SlotStatus.Open) != 0);

    public bool Join(string? password, UserSession userSession)
    {
        if (FreeSlots <= 0 || Password != password)
            return false;

        var slot = NextFreeSlot();
        slot.User = userSession;
        slot.Status = SlotStatus.NotReady;
        
        EnqueueUpdates();
        return true;
    }

    public bool Leave(UserSession userSession)
    {
        var slot = Slots.SingleOrDefault(s => s.User == userSession);
        if (slot is null)
            return false;

        slot.Reset();
        EnqueueUpdates();
        return true;
    }
    
    public bool Ready(bool ready, UserSession userSession)
    {
        var slot = Slots.SingleOrDefault(s => s.User == userSession);
        if (slot is null)
            return false;

        slot.Status = ready ? SlotStatus.Ready : SlotStatus.NotReady;
        EnqueueUpdates();
        return true;
    }

    public bool ChangeSlot(int slotId, UserSession userSession)
    {
        if (slotId is >= 16 or < 0)
            return false;

        var slot = Slots[slotId];
        if ((slot.Status & SlotStatus.Open) == 0)
            return false;

        var oldSlot = Slots.SingleOrDefault(s => s.User == userSession);
        if (oldSlot is null)
            return false;

        Slots[slotId] = new MatchSlot(oldSlot);
        oldSlot.Reset();
        EnqueueUpdates();
        return true;
    }

    public bool LockSlot(int slotId, UserSession requester)
    {
        if (slotId is >= 16 or < 0 || requester != Host)
            return false;

        var slot = Slots[slotId];

        if ((slot.Status & SlotStatus.Locked) != 0)
            slot.Status = SlotStatus.Open;
        else if (slot.User != Host)
        {
            // TODO erm how do we notify them that they got kicked
            // and remove from channel
            slot.Reset();
            slot.Status = SlotStatus.Locked;
        }
        
        EnqueueUpdates();
        return true;
    }

    public void EnqueueUpdates()
    {
        foreach (var userSession in Players)
        {
            userSession.PacketQueue.WriteUpdateMatch(State);
        }
    }
}