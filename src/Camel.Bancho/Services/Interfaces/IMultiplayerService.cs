using Camel.Bancho.Models;
using Camel.Bancho.Packets.Payloads;
using Camel.Core.Enums;

namespace Camel.Bancho.Services.Interfaces;

public interface IMultiplayerService
{
    void JoinLobby(UserSession user);
    void LeaveLobby(UserSession user);
    
    IEnumerable<Match> ActiveMatches();
    Match? ActiveMatch(UserSession user);
    Match CreateMatch(MatchState initialState, UserSession host);
    bool JoinMatch(int matchId, string? password, UserSession user);
    bool LeaveMatch(UserSession user);
    
    bool ChangeSettings(MatchState state, UserSession host);
    bool ChangePassword(MatchState state, UserSession host);
    bool ChangeMods(Mods mods, UserSession host);
    
    bool Ready(bool ready, UserSession user);
    bool ChangeTeam(UserSession user);
    bool HasBeatmap(bool hasBeatmap, UserSession user);
    bool LockSlot(int slotId, UserSession user);
    bool ChangeSlot(int slotId, UserSession user);
    bool TransferHost(int targetId, UserSession user);
    bool Start(UserSession user);
    bool LoadComplete(UserSession user);
    bool SkipRequest(UserSession user);
    bool UpdateScore(ScoreFrame scoreFrame, UserSession user);
    bool Complete(UserSession user);
    bool Fail(UserSession user);
}