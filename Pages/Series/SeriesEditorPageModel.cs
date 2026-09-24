using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Series_Tracker.Models;

namespace Series_Tracker.Pages.Series;

public abstract class SeriesEditorPageModel : PageModel
{
    [BindProperty]
    public SeriesFormModel Form { get; set; } = new();

    public string? Message { get; protected set; }

    public abstract bool IsEditMode { get; }

    public static string? ValidateSeriesDraft(SeriesFormModel form)
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

        if (form.Titles.Any(title => !Enum.IsDefined(title.State)))
        {
            return "Each title must be Upcoming, Released, or Read.";
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
                State = title.State
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
                    State = title.State
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

    public SeriesTitleState State { get; set; }
}