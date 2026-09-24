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

    public async Task OnGetAsync()
    {
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

        SeriesItems = ApplyFilter(seriesItems, StatusFilter)
            .OrderBy(GetAllListPriority)
            .ThenByDescending(series => series.UpdatedAt)
            .ToList();
    }

    public static IEnumerable<SeriesItem> ApplyFilter(IEnumerable<SeriesItem> seriesItems, string statusFilter)
    {
        return statusFilter.ToLowerInvariant() switch
        {
            "to read" => seriesItems.Where(series => !series.IsDropped && series.Titles.Any(title => title.State == SeriesTitleState.Released)),
            "up to date" => seriesItems.Where(series => series.GetDerivedStatus() == SeriesStatus.UpToDate),
            "completed" => seriesItems.Where(series => series.GetDerivedStatus() == SeriesStatus.Completed),
            "dropped" => seriesItems.Where(series => series.IsDropped),
            _ => seriesItems
        };
    }

    private static int GetAllListPriority(SeriesItem series)
    {
        if (series.IsDropped)
        {
            return 4;
        }

        if (series.Titles.Any(title => title.State == SeriesTitleState.Released))
        {
            return 0;
        }

        return series.GetDerivedStatus() switch
        {
            SeriesStatus.UpToDate => 1,
            SeriesStatus.Completed => 3,
            _ => 2
        };
    }
}
