using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;

namespace Camel.Bancho.Services.Interfaces;

public interface IMultiplayerService
{
    void JoinLobby(UserSession user);
    void LeaveLobby(UserSession user);
    
    IEnumerable<Match> ActiveMatches();
    int? ActiveMatchId(UserSession user);
    Match CreateMatch(MatchState initialState, UserSession host);
    bool JoinMatch(int matchId, string? password, UserSession user);
    bool LeaveMatch(UserSession user);
    
    bool Ready(bool ready, UserSession user);
    bool LockSlot(int slotId, UserSession user);
    bool ChangeSlot(int slotId, UserSession user);
    bool Start(UserSession user);
}