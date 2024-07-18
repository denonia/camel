using System.Text;
using Camel.Core.Dtos;

namespace Camel.Bancho.Dtos;

public class LeaderboardResponse
{
    public IList<InGameLeaderboardScore> Scores { get; }
    
    public LeaderboardResponse(IList<InGameLeaderboardScore> scores)
    {
        Scores = scores;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        // {ranked_status}|{serv_has_osz2}|{bid}|{bsid}|{len(scores)}|{fa_track_id}|{fa_license_text}
        // {offset}\n{beatmap_name}\n{rating}

        sb.Append($"2|false|123|123|{Scores.Count}|0|\n");
        sb.AppendFormat("0\nFull name\n10.0\n");
        // no personal best
        sb.Append("\n");

        foreach (var (score, place) in Scores.Select((s, i) => (s, i + 1)))
        {
            var fields = new object[]
            {
                score.Id, score.UserName, score.ScoreNum, score.MaxCombo, 
                score.Count50, score.Count100, score.Count300,
                score.CountMiss, score.CountKatu, score.CountGeki,
                score.Perfect, score.Mods, score.UserId, place,
                score.SetAt.Subtract(DateTime.UnixEpoch).TotalSeconds,
                1
            };
            sb.Append(string.Join('|', fields));
            sb.Append('\n');
        }
        
        return sb.ToString();
    }
}