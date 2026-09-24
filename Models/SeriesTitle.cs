namespace Series_Tracker.Models;

public enum SeriesTitleState
{
    Upcoming = 0,
    Released = 1,
    Read = 2
}

public class SeriesTitle
{
    public int Id { get; set; }

    public int SeriesItemId { get; set; }

    public SeriesItem SeriesItem { get; set; } = null!;

    public string Title { get; set; } = string.Empty;

    public int Position { get; set; }

    public SeriesTitleState State { get; set; }
}