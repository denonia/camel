using System.Text;
using Camel.Core.Dtos;
using Camel.Core.Entities;
using Camel.Core.Enums;

namespace Camel.Bancho.Dtos;

public class LeaderboardResponse
{
    public Beatmap Beatmap { get; }
    public Score? PersonalBest { get; }
    public string UserName { get; }
    public IList<InGameLeaderboardScore> Scores { get; }
    
    public LeaderboardResponse(Beatmap beatmap, Score? personalBest, string userName, IList<InGameLeaderboardScore> scores)
    {
        Beatmap = beatmap;
        PersonalBest = personalBest;
        UserName = userName;
        Scores = scores;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        // TODO: status and rating
        sb.Append($"{ClientRankedStatus.Ranked}|false|{Beatmap.Id}|{Beatmap.MapsetId}|{Scores.Count}|0|\n");
        sb.Append($"0\n{Beatmap.Artist} - {Beatmap.Title} [{Beatmap.Version}]\n10.0\n");
        
        if (PersonalBest is not null)
        {
            var fields = new object[]
            {
                // TODO: WHAT'S THE PLACE
                PersonalBest.Id, UserName, PersonalBest.ScoreNum, PersonalBest.MaxCombo, 
                PersonalBest.Count50, PersonalBest.Count100, PersonalBest.Count300,
                PersonalBest.CountMiss, PersonalBest.CountKatu, PersonalBest.CountGeki,
                PersonalBest.Perfect, PersonalBest.Mods, PersonalBest.UserId, 1,
                PersonalBest.SetAt.Subtract(DateTime.UnixEpoch).TotalSeconds,
                1
            };
            sb.Append(string.Join('|', fields));
        }
        
        sb.Append('\n');

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