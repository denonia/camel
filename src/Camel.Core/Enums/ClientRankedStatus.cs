namespace Camel.Core.Enums;

public enum ClientRankedStatus
{
    NotSubmitted = -1,
    Pending = 0,
    UpdateAvailable = 1,
    Ranked = 2,
    Approved = 3,
    Qualified = 4,
    Loved = 5,
}

public enum RankedStatus
{
    Graveyard = -2,
    Wip = -1,
    Pending = 0,
    Ranked = 1,
    Approved = 2,
    Qualified = 3,
    Loved = 4
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
    
    public static ClientRankedStatus ToClientStatus(this RankedStatus status)
    {
        return status switch
        {
            RankedStatus.Graveyard => ClientRankedStatus.Pending,
            RankedStatus.Wip => ClientRankedStatus.Pending,
            RankedStatus.Pending => ClientRankedStatus.Pending,
            RankedStatus.Ranked => ClientRankedStatus.Ranked,
            RankedStatus.Approved => ClientRankedStatus.Approved,
            RankedStatus.Qualified => ClientRankedStatus.Qualified,
            RankedStatus.Loved => ClientRankedStatus.Loved,
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
