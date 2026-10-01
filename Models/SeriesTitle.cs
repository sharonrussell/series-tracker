namespace Series_Tracker.Models;

public enum TitleAvailability
{
    Unknown,
    Upcoming,
    Available
}

public class SeriesTitle
{
    public int Id { get; set; }

    public int SeriesItemId { get; set; }

    public SeriesItem SeriesItem { get; set; } = null!;

    public string Title { get; set; } = string.Empty;

    public int Position { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public bool IsRead { get; set; }

    public TitleAvailability GetAvailability(DateOnly today)
    {
        if (ReleaseDate is null)
        {
            return TitleAvailability.Unknown;
        }

        return ReleaseDate <= today ? TitleAvailability.Available : TitleAvailability.Upcoming;
    }
}