using System.Globalization;

namespace Camel.Bancho.Dtos;

public readonly struct SearchSetResponse
{
    public string FileName { get; }
    public string Artist { get; }
    public string Title { get; }
    public string Creator { get; }
    public int Status { get; }
    public float Rating { get; }
    public DateTime LastUpdate { get; }
    public int MapsetId { get; }

    public int ThreadId { get; }
    public bool HasVideo { get; }
    public bool HasStoryboard { get; }
    public int FileSize { get; }
    public int FileSizeNoVideo { get; }

    public SearchSetResponse(string fileName, string artist, string title, string creator, int status,
        float rating, DateTime lastUpdate, int mapsetId, int threadId, bool hasVideo, bool hasStoryboard, int fileSize,
        int fileSizeNoVideo)
    {
        FileName = fileName;
        Artist = artist;
        Title = title;
        Creator = creator;
        Status = status;
        Rating = rating;
        LastUpdate = lastUpdate;
        MapsetId = mapsetId;
        ThreadId = threadId;
        HasVideo = hasVideo;
        HasStoryboard = hasStoryboard;
        FileSize = fileSize;
        FileSizeNoVideo = fileSizeNoVideo;
    }

    public override string ToString()
    {
        List<string> values =
        [
            FileName, Artist, Title, Creator, Status.ToString(),
            Rating.ToString("F1"), LastUpdate.ToString(CultureInfo.InvariantCulture), MapsetId.ToString(),
            ThreadId.ToString(), HasVideo ? "1" : "0", HasStoryboard ? "1" : "0", 
            FileSize.ToString(), FileSizeNoVideo.ToString()
        ];
        return string.Join('|', values);
    }
}