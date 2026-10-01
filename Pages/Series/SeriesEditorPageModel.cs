using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Series_Tracker.Models;

namespace Series_Tracker.Pages.Series;

public abstract class SeriesEditorPageModel : PageModel
{
    [BindProperty]
    public SeriesFormModel Form { get; set; } = new();

    public string? Message { get; protected set; }

    public DateOnly Today { get; } = DateOnly.FromDateTime(DateTime.Now);

    public abstract bool IsEditMode { get; }

    public static string? ValidateSeriesDraft(SeriesFormModel form, DateOnly? today = null)
    {
        if (string.IsNullOrWhiteSpace(form.Title))
        {
            return "Series title is required.";
        }

        if (string.IsNullOrWhiteSpace(form.Author))
        {
            return "Author is required.";
        }

        if (form.PlannedLength < 1)
        {
            return "Planned series length must be at least 1.";
        }

        if (form.Titles.Count > form.PlannedLength)
        {
            return "Known titles cannot exceed the planned series length.";
        }

        var blankIndex = form.Titles.FindIndex(title => string.IsNullOrWhiteSpace(title.Title));
        if (blankIndex >= 0)
        {
            return $"Title {blankIndex + 1} needs a name.";
        }

        var currentDate = today ?? DateOnly.FromDateTime(DateTime.Now);
        var invalidReadIndex = form.Titles.FindIndex(title => title.IsRead &&
            (title.ReleaseDate is null || title.ReleaseDate > currentDate));
        if (invalidReadIndex >= 0)
        {
            return $"Title {invalidReadIndex + 1} needs a release date on or before today to be Read.";
        }

        return null;
    }

    protected static List<SeriesTitle> CreateTitles(SeriesFormModel form)
    {
        return form.Titles
            .Select((title, position) => new SeriesTitle
            {
                Title = title.Title.Trim(),
                Position = position,
                ReleaseDate = title.ReleaseDate,
                IsRead = title.IsRead
            })
            .ToList();
    }

    protected static SeriesFormModel CreateFormModel(SeriesItem series)
    {
        return new SeriesFormModel
        {
            Id = series.Id,
            Title = series.Title,
            Author = series.Author,
            PlannedLength = series.PlannedLength,
            IsDropped = series.IsDropped,
            Titles = series.Titles
                .OrderBy(title => title.Position)
                .Select(title => new SeriesTitleDraft
                {
                    Id = title.Id,
                    Title = title.Title,
                    ReleaseDate = title.ReleaseDate,
                    IsRead = title.IsRead
                })
                .ToList()
        };
    }
}

public class SeriesFormModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public int PlannedLength { get; set; } = 1;

    public bool IsDropped { get; set; }

    public List<SeriesTitleDraft> Titles { get; set; } = [];
}

public class SeriesTitleDraft
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateOnly? ReleaseDate { get; set; }

    public bool IsRead { get; set; }
}