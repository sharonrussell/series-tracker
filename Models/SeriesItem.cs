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

    public int CurrentProgress { get; set; }

    public int CurrentReleasedCount { get; set; }

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

    public string GetProgressFraction()
    {
        if (TotalProgress <= 0)
        {
            return "0/0";
        }

        return $"{CurrentProgress}/{TotalProgress}";
    }

    public int GetAvailableToReadCount()
    {
        return Math.Max(0, CurrentReleasedCount - CurrentProgress);
    }

    public bool IsUpToDate()
    {
        return GetAvailableToReadCount() == 0 && CurrentReleasedCount < TotalProgress;
    }

    public bool IsProgressComplete()
    {
        return TotalProgress > 0 && CurrentProgress >= TotalProgress;
    }

    public SeriesCompletionState GetDisplayCompletionState()
    {
        return IsProgressComplete() ? SeriesCompletionState.Completed : CompletionState;
    }
}
