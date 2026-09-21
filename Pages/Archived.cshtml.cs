using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Series_Tracker.Data;
using Series_Tracker.Models;

namespace Series_Tracker.Pages;

public class ArchivedModel : PageModel
{
    private readonly SeriesTrackerDbContext _dbContext;

    public ArchivedModel(SeriesTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IList<SeriesItem> SeriesItems { get; private set; } = new List<SeriesItem>();

    public async Task OnGetAsync()
    {
        SeriesItems = await _dbContext.Series
            .AsNoTracking()
            .Where(s => s.Status == SeriesStatus.Archived)
            .OrderByDescending(s => s.UpdatedAt)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostRestoreAsync(int id)
    {
        var series = await _dbContext.Series.FindAsync(id);
        if (series is null)
        {
            return NotFound();
        }

        series.Status = SeriesStatus.Active;
        series.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return RedirectToPage();
    }
}
