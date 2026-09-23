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

    [BindProperty(SupportsGet = true)]
    public int? EditId { get; set; }

    [BindProperty]
    public SeriesFormModel Form { get; set; } = new();

    public IList<SeriesItem> SeriesItems { get; private set; } = new List<SeriesItem>();

    public string? Message { get; private set; }

    public IReadOnlyList<string> StatusOptions { get; } = Enum.GetNames<SeriesStatus>();

    public bool IsEditorOpen => ShowAddDraft || EditId.HasValue;

    public bool IsEditMode => EditId.HasValue;

    public async Task OnGetAsync()
    {
        await LoadSeriesAsync();

        if (EditId.HasValue)
        {
            var series = await _dbContext.Series.AsNoTracking().FirstOrDefaultAsync(item => item.Id == EditId.Value);
            if (series is null)
            {
                EditId = null;
                Message = "That series could not be found.";
                return;
            }

            Form = CreateFormModel(series);
        }
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var validationMessage = ValidateSeriesDraft(Form.Title, Form.Author, Form.TotalProgress, Form.CurrentReleasedCount, Form.CurrentProgress, Form.CompletionState);
        if (!string.IsNullOrWhiteSpace(validationMessage))
        {
            Message = validationMessage;
            ShowAddDraft = Form.Id == 0;
            EditId = Form.Id == 0 ? null : Form.Id;
            await LoadSeriesAsync();
            return Page();
        }

        if (Form.Id == 0)
        {
            var series = new SeriesItem
            {
                Title = Form.Title.Trim(),
                Author = Form.Author.Trim(),
                Status = SeriesStatus.Reading,
                CompletionState = Form.CompletionState,
                TotalProgress = Math.Max(1, Form.TotalProgress),
                CurrentReleasedCount = Math.Clamp(Form.CurrentReleasedCount, 0, Math.Max(1, Form.TotalProgress)),
                CurrentProgress = Math.Clamp(Form.CurrentProgress, 0, Math.Clamp(Form.CurrentReleasedCount, 0, Math.Max(1, Form.TotalProgress))),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (series.CompletionState == SeriesCompletionState.Completed && series.CurrentProgress == series.TotalProgress)
            {
                series.Status = SeriesStatus.Completed;
            }

            _dbContext.Series.Add(series);
        }
        else
        {
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
        }

        await _dbContext.SaveChangesAsync();
        return RedirectToPage(new { StatusFilter });
    }

    public static string? ValidateSeriesDraft(string title, string author, int totalProgress, int currentReleasedCount, int currentProgress, SeriesCompletionState completionState)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return "Series title is required.";
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            return "Author is required.";
        }

        if (!Enum.IsDefined(completionState))
        {
            return "Series completion state must be ongoing or completed.";
        }

        if (totalProgress < 1)
        {
            return "Series length must be at least 1.";
        }

        if (currentReleasedCount < 0 || currentReleasedCount > totalProgress)
        {
            return "Released so far must be between 0 and the series length.";
        }

        if (currentProgress < 0 || currentProgress > currentReleasedCount)
        {
            return "Books read so far must be between 0 and the released count.";
        }

        return null;
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        var validationMessage = ValidateSeriesDraft(Form.Title, Form.Author, Form.TotalProgress, Form.CurrentReleasedCount, Form.CurrentProgress, Form.CompletionState);
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
            Author = Form.Author.Trim(),
            Status = Form.Status,
            CompletionState = Form.CompletionState,
            TotalProgress = Math.Max(1, Form.TotalProgress),
            CurrentReleasedCount = Math.Clamp(Form.CurrentReleasedCount, 0, Math.Max(1, Form.TotalProgress)),
            CurrentProgress = Math.Clamp(Form.CurrentProgress, 0, Math.Clamp(Form.CurrentReleasedCount, 0, Math.Max(1, Form.TotalProgress))),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (series.Status == SeriesStatus.Completed)
        {
            series.CurrentReleasedCount = series.TotalProgress;
            series.CurrentProgress = series.TotalProgress;
        }

        if (series.CompletionState == SeriesCompletionState.Completed && series.CurrentProgress == series.TotalProgress)
        {
            series.Status = SeriesStatus.Completed;
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
            series.CurrentProgress = Math.Min(series.CurrentReleasedCount, series.CurrentProgress + incrementBy);
        }

        if (status.HasValue)
        {
            series.Status = status.Value;

            if (status.Value == SeriesStatus.Completed && series.TotalProgress > 0)
            {
                series.CurrentReleasedCount = series.TotalProgress;
                series.CurrentProgress = series.TotalProgress;
            }
        }

        if (series.CompletionState == SeriesCompletionState.Completed &&
            series.TotalProgress > 0 &&
            series.CurrentProgress == series.TotalProgress)
        {
            series.Status = SeriesStatus.Completed;
        }

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

    private static SeriesFormModel CreateFormModel(SeriesItem series)
    {
        return new SeriesFormModel
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
    }

    public class SeriesFormModel
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
