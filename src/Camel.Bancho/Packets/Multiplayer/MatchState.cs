using Camel.Bancho.Enums.Multiplayer;
using Camel.Core.Enums;

namespace Camel.Bancho.Packets.Multiplayer;

public readonly struct MatchState
{
    public short Id { get; }
    public bool InProgress { get; }
    public bool PowerPlay { get; }
    public Mods Mods { get; }
    public string Name { get; }
    public string? Password { get; }
    public string MapName { get; }
    public int MapId { get; }
    public string MapMd5 { get; }
    public IEnumerable<SlotStatus> SlotStatuses { get; }
    public IEnumerable<Team> SlotTeams { get; }
    public IEnumerable<int> PlayerIds { get; }
    public int HostId { get; }
    public GameMode Mode { get; }
    public WinCondition WinCondition { get; }
    public TeamType TeamType { get; }
    public bool FreeMods { get; }
    public IEnumerable<Mods> SlotMods { get; }
    public int Seed { get; }

    public MatchState(short id, bool inProgress, bool powerPlay, Mods mods, string name, string? password,
        string mapName, int mapId, string mapMd5,
        IEnumerable<SlotStatus> slotStatuses, IEnumerable<Team> slotTeams, IEnumerable<int> playerIds, int hostId,
        GameMode mode, WinCondition winCondition, TeamType teamType, bool freeMods, IEnumerable<Mods> slotMods,
        int seed)
    {
        Id = id;
        InProgress = inProgress;
        PowerPlay = powerPlay;
        Mods = mods;
        Name = name;
        Password = password;
        MapName = mapName;
        MapId = mapId;
        MapMd5 = mapMd5;
        SlotStatuses = slotStatuses;
        SlotTeams = slotTeams;
        PlayerIds = playerIds;
        HostId = hostId;
        Mode = mode;
        WinCondition = winCondition;
        TeamType = teamType;
        FreeMods = freeMods;
        SlotMods = slotMods;
        Seed = seed;
    }

    public void WriteToStream(PacketBinaryWriter writer, bool writePassword)
    {
        writer.Write(Id);
        writer.Write(InProgress);
        writer.Write(PowerPlay);
        writer.Write((int)Mods);
        writer.Write(Name);

        if (Password is null)
            writer.Write((byte)0);
        else if (writePassword)
            writer.Write(Password);
        else
            writer.Write("");

        writer.Write(MapName);
        writer.Write(MapId);
        writer.Write(MapMd5);

        foreach (var slotStatus in SlotStatuses)
            writer.Write((byte)slotStatus);
        foreach (var slotTeam in SlotTeams)
            writer.Write((byte)slotTeam);

        foreach (var playerId in PlayerIds)
            writer.Write(playerId);

        writer.Write(HostId);
        writer.Write((byte)Mode);
        writer.Write((byte)WinCondition);
        writer.Write((byte)TeamType);
        writer.Write(FreeMods);

        if (FreeMods)
        {
            foreach (var slotMods in SlotMods)
                writer.Write((int)slotMods);
        }

        writer.Write(Seed);
    }

    public static MatchState ReadFromStream(PacketBinaryReader reader)
    {
        var id = reader.ReadInt16();
        var inProgress = reader.ReadBoolean();
        var powerPlay = reader.ReadBoolean();
        var mods = (Mods)reader.ReadInt32();
        var name = reader.ReadString();

        // TODO this should go to the ReadString method
        string? password;
        if (reader.PeekChar() == 0)
        {
            password = null;
            reader.ReadByte();
        }
        else
            password = reader.ReadString();

        var mapName = reader.ReadString();
        var mapId = reader.ReadInt32();
        var mapMd5 = reader.ReadString();

        var slotStatuses = Enumerable.Range(0, 16).Select(_ => (SlotStatus)reader.ReadByte()).ToList();
        var slotTeams = Enumerable.Range(0, 16).Select(_ => (Team)reader.ReadByte()).ToList();

        var playerCount = slotStatuses.Count(s => (s & SlotStatus.HasPlayer) == SlotStatus.HasPlayer);
        var slotIds = Enumerable.Range(0, playerCount).Select(_ => reader.ReadInt32()).ToList();

        var hostId = reader.ReadInt32();
        var mode = (GameMode)reader.ReadByte();
        var winCondition = (WinCondition)reader.ReadByte();
        var teamType = (TeamType)reader.ReadByte();
        var freeMods = reader.ReadBoolean();

        var slotMods = freeMods
            ? Enumerable.Range(0, 16).Select(_ => (Mods)reader.ReadInt32()).ToList()
            : Enumerable.Empty<Mods>();

        var seed = reader.ReadInt32();

        return new MatchState(id, inProgress, powerPlay, mods, name, password, mapName, mapId, mapMd5, slotStatuses,
            slotTeams, slotIds, hostId, mode, winCondition, teamType, freeMods, slotMods, seed);
    }
}