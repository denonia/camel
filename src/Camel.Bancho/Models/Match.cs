using Camel.Bancho.Enums.Multiplayer;
using Camel.Bancho.Packets;
using Camel.Bancho.Packets.Payloads;
using Camel.Core.Enums;

namespace Camel.Bancho.Models;

public class Match
{
    public short Id { get; }
    public bool InProgress { get; private set; } = false;
    public bool PowerPlay { get; } = false;
    public Mods Mods { get; private set; } = Mods.NoMod;
    public string Name { get; private set; }
    public string? Password { get; private set; }
    public bool MatchHistoryPublic { get; }

    public string MapName { get; private set; }
    public int MapId { get; private set; }
    public string MapMd5 { get; private set; }

    public List<MatchSlot> Slots { get; private set; } = [];
    public UserSession Host { get; private set; }

    public GameMode Mode { get; }
    public WinCondition WinCondition { get; private set; } = WinCondition.Score;
    public TeamType TeamType { get; private set; } = TeamType.HeadToHead;
    public bool FreeMods { get; private set; } = false;
    public int Seed { get; }

    public int FreeSlots => Slots.Count(s => (s.Status & SlotStatus.Open) != 0);

    public IEnumerable<MatchSlot> PlayerSlots => Slots.Where(s => s.HasPlayer);
    public IEnumerable<UserSession> Players => PlayerSlots.Select(s => s.User!);

    public string ChatLink => $"[osump://{Id}/{Password} {Name}]";

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

    public MatchState State => new(this, true);
    public MatchState PublicState => new(this, false);

    private MatchSlot NextFreeSlot() => Slots.First(s => (s.Status & SlotStatus.Open) != 0);

    public bool ChangeSettings(MatchState newState, UserSession requester)
    {
        if (requester != Host)
            return false;

        Name = newState.Name;
        WinCondition = newState.WinCondition;

        if (newState.FreeMods != FreeMods)
        {
            FreeMods = newState.FreeMods;

            if (FreeMods)
            {
                foreach (var slot in PlayerSlots)
                    slot.Mods = Mods & ~Mods.SpeedChangingMods;
                Mods &= Mods.SpeedChangingMods;
            }
            else
            {
                var hostSlot = PlayerSlots.Single(s => s.User == Host);
                Mods &= Mods.SpeedChangingMods;
                Mods |= hostSlot.Mods;
                
                foreach (var slot in PlayerSlots)
                    slot.Mods = Mods.NoMod;
            }
        }

        // Changing the map
        if (newState.MapId == -1)
        {
            foreach (var slot in PlayerSlots)
                slot.Status = SlotStatus.NotReady;

            MapId = -1;
            MapMd5 = "";
            MapName = "";
        }
        else if (newState.MapId != MapId)
        {
            MapId = newState.MapId;
            MapMd5 = newState.MapMd5;
            MapName = newState.MapName;
        }

        if (newState.TeamType != TeamType)
        {
            TeamType = newState.TeamType;
            
            var defaultTeam =
                newState.TeamType is TeamType.TeamVs or TeamType.TagTeamVs
                    ? Team.Red
                    : Team.Neutral;

            foreach (var slot in PlayerSlots)
                slot.Team = defaultTeam;
        }

        EnqueueUpdates();

        return true;
    }

    public bool ChangePassword(MatchState newState, UserSession requester)
    {
        if (requester != Host)
            return false;

        Password = newState.Password;
        EnqueueUpdates();
        return true;
    }
    
    public bool ChangeMods(Mods newMods, UserSession requester)
    {
        if (!FreeMods)
        {
            if (requester != Host)
                return false;
            
            Mods = newMods;
            EnqueueUpdates();
            return true;
        }
        
        var slot = Slots.SingleOrDefault(s => s.User == requester);
        if (slot is null)
            return false;

        if (requester == Host)
            Mods = newMods & Mods.SpeedChangingMods;
        
        slot.Mods = newMods & ~Mods.SpeedChangingMods;
        
        EnqueueUpdates();
        return true;
    }

    public bool Join(string? password, UserSession requester)
    {
        if (FreeSlots <= 0 || Password != password)
            return false;

        var slot = NextFreeSlot();
        slot.User = requester;
        slot.Status = SlotStatus.NotReady;

        EnqueueUpdates();
        return true;
    }

    public bool Leave(UserSession requester)
    {
        var slot = Slots.SingleOrDefault(s => s.User == requester);
        if (slot is null)
            return false;

        if (requester == Host)
        {
            var nextUser = PlayerSlots.FirstOrDefault(s => s.User != requester);
            if (nextUser is not null)
            {
                Host = nextUser.User!;
                nextUser.User!.PacketQueue.WriteMatchTransferHost();
            }
        }

        slot.Reset();
        EnqueueUpdates();
        return true;
    }

    public bool Ready(bool ready, UserSession requester)
    {
        var slot = Slots.SingleOrDefault(s => s.User == requester);
        if (slot is null)
            return false;

        slot.Status = ready ? SlotStatus.Ready : SlotStatus.NotReady;
        EnqueueUpdates();
        return true;
    }
    
    public bool ChangeTeam(UserSession requester)
    {
        var slot = Slots.SingleOrDefault(s => s.User == requester);
        if (slot is null || slot.Team == Team.Neutral)
            return false;

        slot.Team = slot.Team == Team.Blue ? Team.Red : Team.Blue;
        EnqueueUpdates();
        return true;
    }
    
    public bool HasBeatmap(bool hasBeatmap, UserSession requester)
    {
        var slot = Slots.SingleOrDefault(s => s.User == requester);
        if (slot is null)
            return false;

        slot.Status = hasBeatmap ? SlotStatus.NotReady : SlotStatus.NoMap;
        EnqueueUpdates();
        return true;
    }

    public bool ChangeSlot(int slotId, UserSession requester)
    {
        if (slotId is >= 16 or < 0)
            return false;

        var slot = Slots[slotId];
        if ((slot.Status & SlotStatus.Open) == 0)
            return false;

        var oldSlot = Slots.SingleOrDefault(s => s.User == requester);
        if (oldSlot is null)
            return false;

        Slots[slotId] = new MatchSlot(oldSlot);
        oldSlot.Reset();
        EnqueueUpdates();
        return true;
    }

    public bool TransferHost(int targetSlotId, UserSession requester)
    {
        if (targetSlotId is >= 16 or < 0 || requester != Host)
            return false;

        var player = Slots[targetSlotId].User;
        if (player is null)
            return false;

        Host = player;
        Host.PacketQueue.WriteMatchTransferHost();
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
            slot.Status = SlotStatus.Locked;

        EnqueueUpdates();
        slot.Reset(SlotStatus.Locked);
        return true;
    }

    public bool Start(UserSession requester)
    {
        if (requester != Host)
            return false;

        InProgress = true;

        var mapHavers = PlayerSlots.Where(s => s.Status != SlotStatus.NoMap);
        foreach (var slot in mapHavers)
        {
            slot.Status = SlotStatus.Playing;
            slot.User!.PacketQueue.WriteMatchStart(State);
        }

        EnqueueUpdates();

        return true;
    }

    public bool LoadComplete(UserSession requester)
    {
        var slot = PlayerSlots.SingleOrDefault(s => s.User == requester);
        if (slot is null)
            return false;
        
        slot.Loaded = true;
        
        if (!PlayerSlots.Any(s => s is { Status: SlotStatus.Playing, Loaded: false }))
        {
            Enqueue(q => q.WriteMatchAllPlayersLoaded());
        }

        return true;
    }

    public bool SkipRequest(UserSession requester)
    {
        var slot = PlayerSlots.SingleOrDefault(s => s.User == requester);
        var slotId = Slots.FindIndex(s => s.User == requester);
        if (slot is null || slotId == -1)
            return false;
        
        slot.Skipped = true;
        Enqueue(q => q.WriteMatchPlayerSkipped(slotId));

        if (!PlayerSlots.Any(s => s is { Status: SlotStatus.Playing, Skipped: false }))
        {
            foreach (var player in Players)
            {
                player.PacketQueue.WriteMatchSkip();
            }
        }

        return true;
    }

    public bool UpdateScore(ScoreFrame scoreFrame, UserSession requester)
    {
        var slotId = Slots.FindIndex(s => s.User == requester);
        if (slotId == -1)
            return false;
        
        var sf = scoreFrame with
        {
            MultiSlotId = (byte)Slots.FindIndex(s => s.User == requester)
        };

        Enqueue(q => q.WriteMatchScoreUpdate(sf));

        return true;
    }

    public bool Complete(UserSession requester)
    {
        var slot = PlayerSlots.SingleOrDefault(s => s.User == requester);
        if (slot is null)
            return false;

        slot.Status = SlotStatus.Complete;

        if (PlayerSlots.Any(s => s.Status == SlotStatus.Playing))
            return true;

        // Everyone is done playing

        InProgress = false;

        foreach (var player in PlayerSlots.Where(s => s.Status == SlotStatus.Complete))
        {
            player.MapEnded();
            player.User!.PacketQueue.WriteMatchComplete();
        }

        EnqueueUpdates();

        return true;
    }

    public bool Fail(UserSession requester)
    {
        var slotIndex = Slots.FindIndex(s => s.User == requester);
        if (slotIndex == -1)
            return false;

        Enqueue(q => q.WriteMatchPlayerFailed(slotIndex));

        return true;
    }
    
    
    private delegate void PacketQueueAction(PacketQueue packetQueue);

    private void Enqueue(PacketQueueAction action)
    {
        foreach (var userSession in Players)
        {
            action(userSession.PacketQueue);
        }
    }

    private void EnqueueUpdates() => Enqueue(q => q.WriteUpdateMatch(State));
}