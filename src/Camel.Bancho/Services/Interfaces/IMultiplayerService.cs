using Camel.Bancho.Models;
using Camel.Bancho.Packets.Multiplayer;

namespace Camel.Bancho.Services.Interfaces;

public interface IMultiplayerService
{
    IEnumerable<Match> ActiveMatches();
    Match CreateMatch(MatchState initialState, UserSession host);
    bool JoinMatch(int matchId, string? password, UserSession user);
    bool LeaveMatch(UserSession user);
}