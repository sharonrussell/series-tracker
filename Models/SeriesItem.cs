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

    public ICollection<SeriesTitle> Titles { get; set; } = new List<SeriesTitle>();

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public int PlannedLength { get; set; } = 1;

    public bool IsDropped { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int GetKnownTitleCount()
    {
        return Titles.Count;
    }

    public int GetReleasedCount()
    {
        return Titles.Count(title => title.State is SeriesTitleState.Released or SeriesTitleState.Read);
    }

    public int GetReadCount()
    {
        return Titles.Count(title => title.State == SeriesTitleState.Read);
    }

    public int GetUnannouncedCount()
    {
        return Math.Max(0, PlannedLength - GetKnownTitleCount());
    }

    public SeriesStatus GetDerivedStatus()
    {
        if (IsDropped)
        {
            return SeriesStatus.Dropped;
        }

        var readCount = GetReadCount();
        if (readCount == 0)
        {
            return SeriesStatus.NotStarted;
        }

        if (PlannedLength > 0 && readCount == PlannedLength)
        {
            return SeriesStatus.Completed;
        }

        if (Titles.All(title => title.State != SeriesTitleState.Released))
        {
            return SeriesStatus.UpToDate;
        }

        return SeriesStatus.Reading;
    }

    public SeriesCompletionState GetDerivedCompletionState()
    {
        return PlannedLength > 0 && GetReleasedCount() == PlannedLength
            ? SeriesCompletionState.Completed
            : SeriesCompletionState.Ongoing;
    }

    public string GetDashboardProgressText()
    {
        return $"{GetReadCount()} / {PlannedLength} read";
    }

    public string GetDashboardSecondaryText()
    {
        if (IsDropped)
        {
            return "Dropped";
        }

        var nextReleased = Titles
            .Where(title => title.State == SeriesTitleState.Released)
            .OrderBy(title => title.Position)
            .FirstOrDefault();
        if (nextReleased is not null)
        {
            return $"Next: {nextReleased.Title}";
        }

        var nextUpcoming = Titles
            .Where(title => title.State == SeriesTitleState.Upcoming)
            .OrderBy(title => title.Position)
            .FirstOrDefault();
        if (nextUpcoming is not null)
        {
            return $"Upcoming: {nextUpcoming.Title}";
        }

        if (GetDerivedStatus() == SeriesStatus.Completed)
        {
            return "Completed";
        }

        if (Titles.Count == 0)
        {
            return "No titles announced";
        }

        return "Up to date";
    }
}
