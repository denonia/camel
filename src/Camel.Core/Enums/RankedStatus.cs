namespace Camel.Core.Enums;

public enum RankedStatus
{
    NotSubmitted = -1,
    Pending = 0,
    UpdateAvailable = 1,
    Ranked = 2,
    Approved = 3,
    Qualified = 4,
    Loved = 5,
}

public static class RankedStatusMethods
{
    public static RankedStatus FromOsuDirect(this int status)
    {
        return status switch
        {
            0 => RankedStatus.Ranked,
            2 => RankedStatus.Pending,
            3 => RankedStatus.Qualified,
            // 4 => Any,
            5 => RankedStatus.Pending,
            7 => RankedStatus.Ranked, // played before
            8 => RankedStatus.Loved,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
    
    public static int ToOsuApiStatus(this RankedStatus status)
    {
        return status switch
        {
            RankedStatus.Pending => 0,
            RankedStatus.Ranked => 1,
            RankedStatus.Approved => 2,
            RankedStatus.Qualified => 3,
            RankedStatus.Loved => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}
