using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Series_Tracker.Data;
using Series_Tracker.Models;

namespace Series_Tracker.Pages;

public class IndexModel : PageModel
{
    private readonly SeriesTrackerDbContext _dbContext;

    public IndexModel(SeriesTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty(SupportsGet = true)]
    public string StatusFilter { get; set; } = "All";

    public IList<SeriesItem> SeriesItems { get; private set; } = new List<SeriesItem>();

    public DateOnly Today { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

    public async Task OnGetAsync()
    {
        Today = DateOnly.FromDateTime(DateTime.Now);
        var seriesItems = await _dbContext.Series
            .AsNoTracking()
            .Include(series => series.Titles)
            .OrderByDescending(series => series.UpdatedAt)
            .ThenByDescending(series => series.Id)
            .ToListAsync();

        foreach (var series in seriesItems)
        {
            series.Titles = series.Titles.OrderBy(title => title.Position).ToList();
        }

        SeriesItems = ApplyFilter(seriesItems, StatusFilter, Today)
            .OrderBy(series => GetAllListPriority(series, Today))
            .ThenByDescending(series => series.UpdatedAt)
            .ToList();
    }

    public static IEnumerable<SeriesItem> ApplyFilter(IEnumerable<SeriesItem> seriesItems, string statusFilter, DateOnly? today = null)
    {
        var currentDate = today ?? DateOnly.FromDateTime(DateTime.Now);
        return statusFilter.ToLowerInvariant() switch
        {
            "to read" => seriesItems.Where(series => !series.IsDropped && series.Titles.Any(title => !title.IsRead && title.GetAvailability(currentDate) == TitleAvailability.Available)),
            "up to date" => seriesItems.Where(series => series.GetDerivedStatus(currentDate) == SeriesStatus.UpToDate),
            "completed" => seriesItems.Where(series => series.GetDerivedStatus(currentDate) == SeriesStatus.Completed),
            "dropped" => seriesItems.Where(series => series.IsDropped),
            _ => seriesItems
        };
    }

    private static int GetAllListPriority(SeriesItem series, DateOnly today)
    {
        if (series.IsDropped)
        {
            return 4;
        }

        if (series.Titles.Any(title => !title.IsRead && title.GetAvailability(today) == TitleAvailability.Available))
        {
            return 0;
        }

        return series.GetDerivedStatus(today) switch
        {
            SeriesStatus.UpToDate => 1,
            SeriesStatus.Completed => 3,
            _ => 2
        };
    }
}
