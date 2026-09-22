using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Series_Tracker.Data;
using Series_Tracker.Models;

namespace Series_Tracker.Pages.Series;

public class EditModel : PageModel
{
    private readonly SeriesTrackerDbContext _dbContext;

    public EditModel(SeriesTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public EditFormModel Form { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var series = await _dbContext.Series.FindAsync(id);
        if (series is null)
        {
            return NotFound();
        }

        Form = new EditFormModel
        {
            Id = series.Id,
            Title = series.Title,
            Author = series.Author,
            CurrentProgress = series.CurrentProgress,
            CurrentReleasedCount = series.CurrentReleasedCount,
            TotalProgress = series.TotalProgress,
            Status = series.Status,
            CompletionState = series.CompletionState
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Form.Title))
        {
            ModelState.AddModelError(nameof(Form.Title), "Title is required.");
            return Page();
        }

        if (string.IsNullOrWhiteSpace(Form.Author))
        {
            ModelState.AddModelError(nameof(Form.Author), "Author is required.");
            return Page();
        }

        if (!Enum.IsDefined(Form.CompletionState))
        {
            ModelState.AddModelError(nameof(Form.CompletionState), "Series completion state must be ongoing or completed.");
            return Page();
        }

        if (Form.TotalProgress < 1)
        {
            ModelState.AddModelError(nameof(Form.TotalProgress), "Series length must be at least 1.");
            return Page();
        }

        if (Form.CurrentReleasedCount < 0 || Form.CurrentReleasedCount > Form.TotalProgress)
        {
            ModelState.AddModelError(nameof(Form.CurrentReleasedCount), "Released so far must be between 0 and the series length.");
            return Page();
        }

        if (Form.CurrentProgress < 0 || Form.CurrentProgress > Form.CurrentReleasedCount)
        {
            ModelState.AddModelError(nameof(Form.CurrentProgress), "Current progress must be between 0 and the released count.");
            return Page();
        }

        var series = await _dbContext.Series.FindAsync(Form.Id);
        if (series is null)
        {
            return NotFound();
        }

        series.Title = Form.Title.Trim();
        series.Author = Form.Author.Trim();
        series.TotalProgress = Math.Max(1, Form.TotalProgress);
        series.CurrentReleasedCount = Math.Clamp(Form.CurrentReleasedCount, 0, series.TotalProgress);
        series.CurrentProgress = Math.Clamp(Form.CurrentProgress, 0, series.CurrentReleasedCount);
        series.Status = Form.Status;
        series.CompletionState = Form.CompletionState;
        if (series.Status == SeriesStatus.Completed)
        {
            series.CurrentReleasedCount = series.TotalProgress;
            series.CurrentProgress = series.TotalProgress;
        }

        if (series.CompletionState == SeriesCompletionState.Completed && series.CurrentProgress == series.TotalProgress)
        {
            series.Status = SeriesStatus.Completed;
        }

        series.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return RedirectToPage("/Index");
    }

    public class EditFormModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public int CurrentProgress { get; set; }

        public int CurrentReleasedCount { get; set; }

        public int TotalProgress { get; set; } = 1;

        public SeriesStatus Status { get; set; } = SeriesStatus.Reading;

        public SeriesCompletionState CompletionState { get; set; } = SeriesCompletionState.Ongoing;
    }
}
