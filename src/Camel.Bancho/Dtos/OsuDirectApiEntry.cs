using System.Text.Json.Serialization;

namespace Camel.Bancho.Dtos;

public class ChildrenBeatmap
{
    [JsonPropertyName("BeatmapID")] public int BeatmapID { get; set; }

    [JsonPropertyName("ParentSetID")] public int ParentSetID { get; set; }

    [JsonPropertyName("DiffName")] public string DiffName { get; set; }

    [JsonPropertyName("FileMD5")] public string FileMD5 { get; set; }

    [JsonPropertyName("Mode")] public int Mode { get; set; }

    [JsonPropertyName("BPM")] public double BPM { get; set; }

    [JsonPropertyName("AR")] public double AR { get; set; }

    [JsonPropertyName("OD")] public double OD { get; set; }

    [JsonPropertyName("CS")] public double CS { get; set; }

    [JsonPropertyName("HP")] public double HP { get; set; }

    [JsonPropertyName("TotalLength")] public int TotalLength { get; set; }

    [JsonPropertyName("HitLength")] public int HitLength { get; set; }

    [JsonPropertyName("Playcount")] public int Playcount { get; set; }

    [JsonPropertyName("Passcount")] public int Passcount { get; set; }

    [JsonPropertyName("MaxCombo")] public int? MaxCombo { get; set; }

    [JsonPropertyName("DifficultyRating")] public double DifficultyRating { get; set; }
}

public class Genre
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }
}

public class Language
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }
}

public class OsuDirectApiEntry
{
    [JsonPropertyName("SetID")] public int SetID { get; set; }

    [JsonPropertyName("RankedStatus")] public int RankedStatus { get; set; }

    [JsonPropertyName("ChildrenBeatmaps")] public List<ChildrenBeatmap> ChildrenBeatmaps { get; set; }

    [JsonPropertyName("ApprovedDate")] public DateTime ApprovedDate { get; set; }

    [JsonPropertyName("LastUpdate")] public DateTime LastUpdate { get; set; }

    [JsonPropertyName("LastChecked")] public DateTime LastChecked { get; set; }

    [JsonPropertyName("Artist")] public string Artist { get; set; }

    [JsonPropertyName("Title")] public string Title { get; set; }

    [JsonPropertyName("Creator")] public string Creator { get; set; }

    [JsonPropertyName("CreatorID")] public int CreatorID { get; set; }

    [JsonPropertyName("Source")] public string Source { get; set; }

    [JsonPropertyName("Tags")] public string Tags { get; set; }

    [JsonPropertyName("HasVideo")] public bool HasVideo { get; set; }

    [JsonPropertyName("Genre")] public Genre Genre { get; set; }

    [JsonPropertyName("Language")] public Language Language { get; set; }

    [JsonPropertyName("Favourites")] public int Favourites { get; set; }

    [JsonPropertyName("StarRating")] public double StarRating { get; set; }
}