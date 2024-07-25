using System.Text.Json.Serialization;

namespace Camel.Bancho.Dtos;

public class ChildrenBeatmap
{
    [JsonPropertyName("BeatmapID")] public required int BeatmapID { get; set; }

    [JsonPropertyName("ParentSetID")] public required int ParentSetID { get; set; }

    [JsonPropertyName("DiffName")] public required string DiffName { get; set; }

    [JsonPropertyName("FileMD5")] public required string FileMD5 { get; set; }

    [JsonPropertyName("Mode")] public required int Mode { get; set; }

    [JsonPropertyName("BPM")] public required double BPM { get; set; }

    [JsonPropertyName("AR")] public required double AR { get; set; }

    [JsonPropertyName("OD")] public required double OD { get; set; }

    [JsonPropertyName("CS")] public required double CS { get; set; }

    [JsonPropertyName("HP")] public required double HP { get; set; }

    [JsonPropertyName("TotalLength")] public required int TotalLength { get; set; }

    [JsonPropertyName("HitLength")] public required int HitLength { get; set; }

    [JsonPropertyName("Playcount")] public required int Playcount { get; set; }

    [JsonPropertyName("Passcount")] public required int Passcount { get; set; }

    [JsonPropertyName("MaxCombo")] public required int? MaxCombo { get; set; }

    [JsonPropertyName("DifficultyRating")] public required double DifficultyRating { get; set; }
}

public class Genre
{
    [JsonPropertyName("id")] public required int Id { get; set; }

    [JsonPropertyName("name")] public required string Name { get; set; }
}

public class Language
{
    [JsonPropertyName("id")] public required int Id { get; set; }

    [JsonPropertyName("name")] public required string Name { get; set; }
}

public class OsuDirectApiEntry
{
    [JsonPropertyName("SetID")] public required int SetID { get; set; }

    [JsonPropertyName("RankedStatus")] public required int RankedStatus { get; set; }

    [JsonPropertyName("ChildrenBeatmaps")] public required List<ChildrenBeatmap> ChildrenBeatmaps { get; set; }

    [JsonPropertyName("ApprovedDate")] public required DateTime ApprovedDate { get; set; }

    [JsonPropertyName("LastUpdate")] public required DateTime LastUpdate { get; set; }

    [JsonPropertyName("LastChecked")] public required DateTime LastChecked { get; set; }

    [JsonPropertyName("Artist")] public required string Artist { get; set; }

    [JsonPropertyName("Title")] public required string Title { get; set; }

    [JsonPropertyName("Creator")] public required string Creator { get; set; }

    [JsonPropertyName("CreatorID")] public required int CreatorID { get; set; }

    [JsonPropertyName("Source")] public required string Source { get; set; }

    [JsonPropertyName("Tags")] public required string Tags { get; set; }

    [JsonPropertyName("HasVideo")] public required bool HasVideo { get; set; }

    [JsonPropertyName("Genre")] public required Genre Genre { get; set; }

    [JsonPropertyName("Language")] public required Language Language { get; set; }

    [JsonPropertyName("Favourites")] public required int Favourites { get; set; }

    [JsonPropertyName("StarRating")] public required double StarRating { get; set; }
}