using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Services;

public class MultiplayerService : IMultiplayerService
{
    private readonly IChatService _chatService;
    private readonly List<Match> _activeMatches = [];
    private short _lastMatchId;

    private readonly List<UserSession> _lobbyParticipants = [];

    public MultiplayerService(IChatService chatService)
    {
        _chatService = chatService;
    }

    public void JoinLobby(UserSession user)
    {
        _lobbyParticipants.Add(user);
        _chatService.JoinChannel("#lobby", user);

        foreach (var match in ActiveMatches())
        {
            user.PacketQueue.WriteNewMatch(match.PublicState);
        }
    }

    public void LeaveLobby(UserSession user)
    {
        _lobbyParticipants.Remove(user);
    }

    private void UpdateLobby(Match match)
    {
        foreach (var user in _lobbyParticipants)
        {
            user.PacketQueue.WriteUpdateMatch(match.PublicState);
        }
    }

    public IEnumerable<Match> ActiveMatches() => _activeMatches;

    public int? ActiveMatchId(UserSession user) => ActiveMatch(user)?.Id;

    private Match? ActiveMatch(UserSession user) => _activeMatches.SingleOrDefault(m => m.Players.Contains(user));

    private delegate bool MatchAction(Match match);

    private bool ActiveMatchDo(UserSession user, MatchAction action)
    {
        var match = ActiveMatch(user);
        var result = match is not null && action(match);

        if (result)
            UpdateLobby(match!);

        return result;
    }

    public Match CreateMatch(MatchState initialState, UserSession host)
    {
        // Client adds "//private" to the end of the password
        // if the "Make match history publicly viewable" box is unticked

        var password = initialState.Password;

        var matchHistoryPublic =
            string.IsNullOrEmpty(password) || !password.EndsWith("//private");

        if (!matchHistoryPublic)
            password = initialState.Password![..^9];

        // TODO fix reader returning "" strings
        if (string.IsNullOrEmpty(password))
            password = null;

        var matchId = ++_lastMatchId;
        var match = new Match(matchId, initialState.Name, password,
            matchHistoryPublic, initialState.MapName, initialState.MapId, initialState.MapMd5, host, initialState.Mode);

        _chatService.JoinMultiplayerChannel(match, host);

        host.PacketQueue.WriteMatchJoinSuccess(match.State);

        _activeMatches.Add(match);
        UpdateLobby(match);
        return match;
    }

    public bool JoinMatch(int matchId, string? password, UserSession user)
    {
        if (string.IsNullOrEmpty(password))
            password = null;

        var match = _activeMatches.SingleOrDefault(m => m.Id == matchId);
        if (match is null || !match.Join(password, user))
            return false;

        user.PacketQueue.WriteMatchJoinSuccess(match.State);

        _chatService.JoinMultiplayerChannel(match, user);
        UpdateLobby(match);
        return true;
    }

    public bool LeaveMatch(UserSession user)
    {
        var match = ActiveMatch(user);
        if (match is null)
            return false;

        match.Leave(user);

        _chatService.LeaveMultiplayerChannel(match, user);

        if (match.Slots.All(s => s.User == null))
        {
            foreach (var lobbyParticipant in _lobbyParticipants)
            {
                lobbyParticipant.PacketQueue.WriteDisposeMatch(match.Id);
            }

            _activeMatches.Remove(match);
        }

        UpdateLobby(match);

        return true;
    }

    public bool Ready(bool ready, UserSession user) => ActiveMatch(user)?.Ready(ready, user) ?? false;
    public bool LockSlot(int slotId, UserSession user) => ActiveMatchDo(user, m => m.LockSlot(slotId, user));
    public bool ChangeSlot(int slotId, UserSession user) => ActiveMatchDo(user, m => m.ChangeSlot(slotId, user));
    public bool Start(UserSession user) => ActiveMatchDo(user, m => m.Start(user));
}