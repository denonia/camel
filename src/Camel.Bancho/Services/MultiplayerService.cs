using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Bancho.Services.Interfaces;

namespace Camel.Bancho.Services;

public class MultiplayerService : IMultiplayerService
{
    private readonly IChatService _chatService;
    private readonly List<Match> _activeMatches = [];
    private short _lastMatchId;

    public MultiplayerService(IChatService chatService)
    {
        _chatService = chatService;
    }

    public IEnumerable<Match> ActiveMatches() => _activeMatches;

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
        host.Match = match;

        _activeMatches.Add(match);
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
        user.Match = match;

        _chatService.JoinMultiplayerChannel(match, user);
        return true;
    }

    public bool LeaveMatch(UserSession user)
    {
        var match = user.Match;
        if (match is null)
            return false;

        match.Leave(user);

        _chatService.LeaveMultiplayerChannel(match, user);

        return true;
    }
}