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

    public int PlannedLength { get; set; } = 2;

    public bool IsDropped { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int GetKnownTitleCount()
    {
        return Titles.Count;
    }

    public int GetReleasedCount(DateOnly? today = null)
    {
        var currentDate = today ?? DateOnly.FromDateTime(DateTime.Now);
        return Titles.Count(title => title.GetAvailability(currentDate) == TitleAvailability.Available);
    }

    public int GetReadCount()
    {
        return Titles.Count(title => title.IsRead);
    }

    public int GetUnannouncedCount()
    {
        return Math.Max(0, PlannedLength - GetKnownTitleCount());
    }

    public SeriesStatus GetDerivedStatus(DateOnly? today = null)
    {
        var currentDate = today ?? DateOnly.FromDateTime(DateTime.Now);
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

        if (Titles.All(title => title.IsRead || title.GetAvailability(currentDate) != TitleAvailability.Available))
        {
            return SeriesStatus.UpToDate;
        }

        return SeriesStatus.Reading;
    }

    public SeriesCompletionState GetDerivedCompletionState(DateOnly? today = null)
    {
        return PlannedLength > 0 && GetReleasedCount(today) == PlannedLength
            ? SeriesCompletionState.Completed
            : SeriesCompletionState.Ongoing;
    }

    public string GetDashboardProgressText()
    {
        return $"{GetReadCount()} / {PlannedLength} read";
    }

    public string GetDashboardSecondaryText(DateOnly? today = null)
    {
        var currentDate = today ?? DateOnly.FromDateTime(DateTime.Now);
        if (IsDropped)
        {
            return "Dropped";
        }

        var nextReleased = Titles
            .Where(title => !title.IsRead && title.GetAvailability(currentDate) == TitleAvailability.Available)
            .OrderBy(title => title.Position)
            .FirstOrDefault();
        if (nextReleased is not null)
        {
            return $"Next: {nextReleased.Title}";
        }

        var nextUpcoming = Titles
            .Where(title => title.GetAvailability(currentDate) == TitleAvailability.Upcoming)
            .OrderBy(title => title.Position)
            .FirstOrDefault();
        if (nextUpcoming is not null)
        {
            return $"Upcoming: {nextUpcoming.Title}";
        }

        var nextUnknown = Titles
            .Where(title => title.GetAvailability(currentDate) == TitleAvailability.Unknown)
            .OrderBy(title => title.Position)
            .FirstOrDefault();
        if (nextUnknown is not null)
        {
            return $"Date unknown: {nextUnknown.Title}";
        }

        if (GetDerivedStatus(currentDate) == SeriesStatus.Completed)
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
