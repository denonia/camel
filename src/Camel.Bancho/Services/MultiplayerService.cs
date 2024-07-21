using Camel.Bancho.Models;
using Camel.Bancho.Packets.Multiplayer;
using Camel.Bancho.Packets.Server;
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
            string.IsNullOrEmpty(password) || password.EndsWith("//private");

        if (!matchHistoryPublic)
            password = initialState.Password![..^9];

        // TODO fix reader returning "" strings
        if (string.IsNullOrEmpty(password))
            password = null;

        var matchId = ++_lastMatchId;
        var match = new Match(matchId, initialState.Name, password,
            matchHistoryPublic, initialState.MapName, initialState.MapId, initialState.MapMd5, host, initialState.Mode);

        _chatService.JoinMultiplayerChannel(match, host);

        var successPacket = new MatchJoinSuccessPacket(match.State);
        host.PacketQueue.WritePacket(successPacket);
        host.Match = match;

        _activeMatches.Add(match);
        return match;
    }

    public bool LeaveMatch(UserSession user)
    {
        return true;
    }
}