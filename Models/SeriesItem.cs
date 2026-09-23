namespace Series_Tracker.Models;

public enum SeriesStatus
{
    NotStarted = 0,
    Reading = 1,
    UpToDate = 2,
    Completed = 3,
    Dropped = 4
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

    public string GetDashboardProgressText()
    {
        return GetProgressFraction();
    }

    public SeriesStatus GetDerivedStatus()
    {
        if (Status == SeriesStatus.Dropped)
        {
            return SeriesStatus.Dropped;
        }

        if (CurrentProgress <= 0)
        {
            return SeriesStatus.NotStarted;
        }

        if (TotalProgress > 0 && CurrentProgress >= TotalProgress && GetDerivedCompletionState() == SeriesCompletionState.Completed)
        {
            return SeriesStatus.Completed;
        }

        if (CurrentReleasedCount > 0 && CurrentProgress >= CurrentReleasedCount && CurrentReleasedCount < TotalProgress)
        {
            return SeriesStatus.UpToDate;
        }

        return SeriesStatus.Reading;
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

    public SeriesCompletionState GetDerivedCompletionState()
    {
        return TotalProgress > 0 && CurrentReleasedCount >= TotalProgress ? SeriesCompletionState.Completed : SeriesCompletionState.Ongoing;
    }

    public SeriesCompletionState GetDisplayCompletionState()
    {
        return GetDerivedCompletionState();
    }
}
