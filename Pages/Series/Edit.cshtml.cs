using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Series_Tracker.Data;

namespace Series_Tracker.Pages.Series;

public class EditModel : SeriesEditorPageModel
{
    private readonly SeriesTrackerDbContext _dbContext;

    public EditModel(SeriesTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override bool IsEditMode => true;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var series = await _dbContext.Series
            .AsNoTracking()
            .Include(item => item.Titles)
            .FirstOrDefaultAsync(item => item.Id == id);
        if (series is null)
        {
            return NotFound();
        }

        Form = CreateFormModel(series);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        Form.Id = id;
        Message = ValidateSeriesDraft(Form);
        if (Message is not null)
        {
            return Page();
        }

        var series = await _dbContext.Series
            .Include(item => item.Titles)
            .FirstOrDefaultAsync(item => item.Id == id);
        if (series is null)
        {
            return NotFound();
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        _dbContext.SeriesTitles.RemoveRange(series.Titles);
        await _dbContext.SaveChangesAsync();

        series.Title = Form.Title.Trim();
        series.Author = Form.Author.Trim();
        series.PlannedLength = Form.PlannedLength;
        series.IsDropped = Form.IsDropped;
        series.Titles = CreateTitles(Form);
        series.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var series = await _dbContext.Series.FindAsync(id);
        if (series is null)
        {
            return NotFound();
        }

        _dbContext.Series.Remove(series);
        await _dbContext.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}
