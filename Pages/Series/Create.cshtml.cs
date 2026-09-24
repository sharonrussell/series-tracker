using Microsoft.AspNetCore.Mvc;
using Series_Tracker.Data;
using Series_Tracker.Models;

namespace Series_Tracker.Pages.Series;

public class CreateModel : SeriesEditorPageModel
{
    private readonly SeriesTrackerDbContext _dbContext;

    public CreateModel(SeriesTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override bool IsEditMode => false;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Message = ValidateSeriesDraft(Form);
        if (Message is not null)
        {
            return Page();
        }

        var series = new SeriesItem
        {
            Title = Form.Title.Trim(),
            Author = Form.Author.Trim(),
            PlannedLength = Form.PlannedLength,
            Titles = CreateTitles(Form),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Series.Add(series);
        await _dbContext.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}