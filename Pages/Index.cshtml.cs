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

    [BindProperty(SupportsGet = true)]
    public bool ShowAddDraft { get; set; }

    [BindProperty]
    public SeriesFormModel Form { get; set; } = new();

    public IList<SeriesItem> SeriesItems { get; private set; } = new List<SeriesItem>();

    public string? Message { get; private set; }

    public IReadOnlyList<string> StatusOptions { get; } = Enum.GetNames<SeriesStatus>();

    public async Task OnGetAsync()
    {
        await LoadSeriesAsync();
    }

    public static string? ValidateSeriesDraft(string title, int totalProgress, int currentProgress)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return "Series title is required.";
        }

        if (totalProgress < 1)
        {
            return "Series length must be at least 1.";
        }

        if (currentProgress < 0 || currentProgress > totalProgress)
        {
            return "Books read so far must be between 0 and the series length.";
        }

        return null;
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        var validationMessage = ValidateSeriesDraft(Form.Title, Form.TotalProgress, Form.CurrentProgress);
        if (!string.IsNullOrWhiteSpace(validationMessage))
        {
            Message = validationMessage;
            ShowAddDraft = true;
            await LoadSeriesAsync();
            return Page();
        }

        var series = new SeriesItem
        {
            Title = Form.Title.Trim(),
            Notes = string.IsNullOrWhiteSpace(Form.Notes) ? null : Form.Notes.Trim(),
            Status = Form.Status,
            TotalProgress = Math.Max(1, Form.TotalProgress),
            CurrentProgress = Math.Clamp(Form.CurrentProgress, 0, Math.Max(1, Form.TotalProgress)),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (series.Status == SeriesStatus.Completed)
        {
            series.CurrentProgress = series.TotalProgress;
        }

        _dbContext.Series.Add(series);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage(new { StatusFilter });
    }

    public async Task<IActionResult> OnPostUpdateAsync(int id, int incrementBy = 0, SeriesStatus? status = null)
    {
        var series = await _dbContext.Series.FindAsync(id);
        if (series is null)
        {
            return NotFound();
        }

        if (incrementBy > 0)
        {
            series.CurrentProgress += incrementBy;
        }

        if (status.HasValue)
        {
            series.Status = status.Value;

            if (status.Value == SeriesStatus.Completed && series.TotalProgress > 0)
            {
                series.CurrentProgress = series.TotalProgress;
            }
        }

        series.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return RedirectToPage(new { StatusFilter });
    }

    public async Task<IActionResult> OnPostArchiveAsync(int id)
    {
        var series = await _dbContext.Series.FindAsync(id);
        if (series is null)
        {
            return NotFound();
        }

        series.Status = SeriesStatus.Archived;
        series.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return RedirectToPage(new { StatusFilter });
    }

    private async Task LoadSeriesAsync()
    {
        IQueryable<SeriesItem> query = _dbContext.Series.AsNoTracking().OrderByDescending(s => s.UpdatedAt);

        if (!string.Equals(StatusFilter, "All", StringComparison.OrdinalIgnoreCase) &&
            Enum.TryParse<SeriesStatus>(StatusFilter, true, out var parsedStatus))
        {
            query = query.Where(s => s.Status == parsedStatus);
        }

        SeriesItems = await query.ToListAsync();
    }

    public class SeriesFormModel
    {
        public string Title { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public int CurrentProgress { get; set; }

        public int TotalProgress { get; set; } = 1;

        public SeriesStatus Status { get; set; } = SeriesStatus.Active;
    }
}
