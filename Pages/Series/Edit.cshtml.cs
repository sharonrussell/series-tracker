using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Series_Tracker.Data;

namespace Series_Tracker.Pages.Series;

public class EditModel : PageModel
{
    private readonly SeriesTrackerDbContext _dbContext;

    public EditModel(SeriesTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var series = await _dbContext.Series.FindAsync(id);
        if (series is null)
        {
            return NotFound();
        }

        return RedirectToPage("/Index", new { EditId = series.Id });
    }
}
