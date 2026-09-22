namespace Series_Tracker.Models;

public enum SeriesStatus
{
    Reading = 0,
    Completed = 1,
    Dropped = 2
}

public enum SeriesCompletionState
{
    Ongoing = 0,
    Completed = 1
}

public class SeriesItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public int CurrentProgress { get; set; }

    public int TotalProgress { get; set; }

    public SeriesStatus Status { get; set; } = SeriesStatus.Reading;

    public SeriesCompletionState CompletionState { get; set; } = SeriesCompletionState.Ongoing;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int GetProgressPercent()
    {
        if (TotalProgress <= 0)
        {
            return 0;
        }

        return (int)Math.Round((double)CurrentProgress / TotalProgress * 100);
    }
}
