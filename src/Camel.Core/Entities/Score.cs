using Camel.Core.Enums;

namespace Camel.Core.Entities;

public class Score
{
    public int Id { get; set; }
    public string MapMd5 { get; set; }
    public int ScoreNum { get; set; }
    public float Pp { get; set; }
    public float Accuracy { get; set; }
    public int MaxCombo { get; set; }
    public int Mods { get; set; }
    public int Count300 { get; set; }
    public int Count100 { get; set; }
    public int Count50 { get; set; }
    public int CountMiss { get; set; }
    public int CountGeki { get; set; }
    public int CountKatu { get; set; }
    public Grade Grade { get; set; }
    public SubmissionStatus Status { get; set; }
    public GameMode Mode { get; set; }
    public DateTime SetAt { get; set; }
    public int TimeElapsed { get; set; }
    public int ClientFlags { get; set; }
    public bool Perfect { get; set; }
    public string OnlineChecksum { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }

    public static Score FromSubmission(string[] data)
    {
        var result = new Score
        {
            MapMd5 = data[0],
            OnlineChecksum = data[2],
            Count300 = Convert.ToInt32(data[3]),
            Count100 = Convert.ToInt32(data[4]),
            Count50 = Convert.ToInt32(data[5]),
            CountGeki = Convert.ToInt32(data[6]),
            CountKatu = Convert.ToInt32(data[7]),
            CountMiss = Convert.ToInt32(data[8]),
            ScoreNum = Convert.ToInt32(data[9]),
            MaxCombo = Convert.ToInt32(data[10]),
            Perfect = data[11] == "True",
            Grade = Enum.Parse<Grade>(data[12].ToUpper()),
            Mods = Convert.ToInt32(data[13]),
            //passed
            Status = data[14] == "True" ? SubmissionStatus.Submitted : SubmissionStatus.Failed,
            Mode = (GameMode)Convert.ToInt32(data[15]),
            SetAt = DateTime.ParseExact(data[16], "yyMMddHHmmss", null).ToUniversalTime()
            // client flags
            
        };

        return result;
    }
}