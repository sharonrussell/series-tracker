using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Series_Tracker.Data;
using Series_Tracker.Models;
using Series_Tracker.Pages;
using Series_Tracker.Pages.Series;

namespace SeriesTracker.Tests;

public class SeriesTrackerTests
{
    [Fact]
    public void TitleAvailability_UsesReleaseDateBoundary()
    {
        var today = new DateOnly(2026, 10, 1);
        var title = new SeriesTitle();

        Assert.Equal(TitleAvailability.Unknown, title.GetAvailability(today));
        title.ReleaseDate = today.AddDays(1);
        Assert.Equal(TitleAvailability.Upcoming, title.GetAvailability(today));
        title.ReleaseDate = today;
        Assert.Equal(TitleAvailability.Available, title.GetAvailability(today));
        title.ReleaseDate = today.AddDays(-1);
        Assert.Equal(TitleAvailability.Available, title.GetAvailability(today));
    }

    [Fact]
    public void SeriesAvailability_RollsOverWithoutEditingTitles()
    {
        var today = new DateOnly(2026, 10, 1);
        var series = new SeriesItem
        {
            PlannedLength = 3,
            Titles =
            [
                new SeriesTitle { Title = "Finished", Position = 0, ReleaseDate = today.AddDays(-1), IsRead = true },
                new SeriesTitle { Title = "Next", Position = 1, ReleaseDate = today.AddDays(1) },
                new SeriesTitle { Title = "Unknown", Position = 2 }
            ]
        };

        Assert.Equal(1, series.GetReleasedCount(today));
        Assert.Equal("Upcoming: Next", series.GetDashboardSecondaryText(today));
        Assert.Equal(SeriesStatus.UpToDate, series.GetDerivedStatus(today));
        Assert.Equal(2, series.GetReleasedCount(today.AddDays(1)));
        Assert.Equal("Next: Next", series.GetDashboardSecondaryText(today.AddDays(1)));
        Assert.Equal(SeriesStatus.Reading, series.GetDerivedStatus(today.AddDays(1)));
        series.Titles.Single(title => title.Title == "Next").IsRead = true;
        Assert.Equal("Date unknown: Unknown", series.GetDashboardSecondaryText(today.AddDays(1)));
    }

    [Fact]
    public void DerivedCounts_UseDatesAndReadValuesAndPlannedLength()
    {
        var series = CreateSeries(6,
            Read("Read"),
            Available("Released"),
            Upcoming("Upcoming"));

        Assert.Equal(3, series.GetKnownTitleCount());
        Assert.Equal(2, series.GetReleasedCount());
        Assert.Equal(1, series.GetReadCount());
        Assert.Equal(3, series.GetUnannouncedCount());
        Assert.Equal(SeriesCompletionState.Ongoing, series.GetDerivedCompletionState());
        Assert.Equal("1 / 6 read", series.GetDashboardProgressText());
    }

    [Theory]
    [InlineData(false, 3, "", SeriesStatus.NotStarted)]
    [InlineData(false, 3, "RL", SeriesStatus.Reading)]
    [InlineData(false, 3, "RU", SeriesStatus.UpToDate)]
    [InlineData(false, 2, "RR", SeriesStatus.Completed)]
    [InlineData(true, 3, "RR", SeriesStatus.Dropped)]
    public void GetDerivedStatus_UsesTitleAvailabilityPrecedence(bool dropped, int plannedLength, string states, SeriesStatus expected)
    {
        var titles = states.Select((state, index) => state switch
        {
            'R' => Read($"Book {index + 1}"),
            'L' => Available($"Book {index + 1}"),
            _ => Upcoming($"Book {index + 1}")
        }).ToArray();
        var series = CreateSeries(plannedLength, titles);
        series.IsDropped = dropped;

        Assert.Equal(expected, series.GetDerivedStatus());
    }

    [Fact]
    public void DashboardSecondaryText_UsesActionableTitleOrderAndFallbacks()
    {
        var series = CreateSeries(5,
            Read("Already read"),
            Upcoming("Later release"),
            Available("Read next"));

        Assert.Equal("Next: Read next", series.GetDashboardSecondaryText());

        series.Titles.Single(title => title.Title == "Read next").IsRead = true;
        Assert.Equal("Upcoming: Later release", series.GetDashboardSecondaryText());

        var laterRelease = series.Titles.Single(title => title.Title == "Later release");
        laterRelease.ReleaseDate = DateOnly.FromDateTime(DateTime.Now);
        laterRelease.IsRead = true;
        Assert.Equal("Up to date", series.GetDashboardSecondaryText());

        series.PlannedLength = 3;
        Assert.Equal("Completed", series.GetDashboardSecondaryText());

        series.IsDropped = true;
        Assert.Equal("Dropped", series.GetDashboardSecondaryText());
    }

    [Fact]
    public void DashboardSecondaryText_ReportsNoTitlesAnnounced()
    {
        Assert.Equal("No titles announced", CreateSeries(4).GetDashboardSecondaryText());
    }

    [Fact]
    public void ValidateSeriesDraft_RejectsBlankTitleAndExcessKnownTitles()
    {
        var blankTitle = ValidForm();
        blankTitle.Titles.Add(new SeriesTitleDraft());
        Assert.Equal("Title 1 needs a name.", SeriesEditorPageModel.ValidateSeriesDraft(blankTitle));

        var tooMany = ValidForm();
        tooMany.PlannedLength = 1;
        tooMany.Titles =
        [
            new SeriesTitleDraft { Title = "One" },
            new SeriesTitleDraft { Title = "Two" }
        ];
        Assert.Equal("Known titles cannot exceed the planned series length.", SeriesEditorPageModel.ValidateSeriesDraft(tooMany));
    }

    [Fact]
    public void ValidateSeriesDraft_AcceptsValidOrderedTitles()
    {
        var form = ValidForm();
        form.Titles =
        [
            new SeriesTitleDraft { Title = "One", ReleaseDate = new DateOnly(2026, 10, 1), IsRead = true },
            new SeriesTitleDraft { Title = "Two" }
        ];

        Assert.Null(SeriesEditorPageModel.ValidateSeriesDraft(form, new DateOnly(2026, 10, 1)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("2026-10-02")]
    public void ValidateSeriesDraft_RejectsReadWithoutAvailableDate(string? releaseDate)
    {
        var form = ValidForm();
        form.Titles.Add(new SeriesTitleDraft
        {
            Title = "Not available",
            IsRead = true,
            ReleaseDate = releaseDate is null ? null : DateOnly.Parse(releaseDate, System.Globalization.CultureInfo.InvariantCulture)
        });

        Assert.Equal("Title 1 needs a release date on or before today to be Read.",
            SeriesEditorPageModel.ValidateSeriesDraft(form, new DateOnly(2026, 10, 1)));
        Assert.True(form.Titles[0].IsRead);
    }

    [Fact]
    public void EfModel_ConfiguresRequiredCascadeAndUniquePosition()
    {
        using var connection = new SqliteConnection("Data Source=:memory:");
        var dbContext = CreateDbContext(connection);
        var entity = dbContext.Model.FindEntityType(typeof(SeriesTitle))!;

        Assert.False(entity.FindProperty(nameof(SeriesTitle.Title))!.IsNullable);
        Assert.Equal(200, entity.FindProperty(nameof(SeriesTitle.Title))!.GetMaxLength());
        Assert.True(entity.FindProperty(nameof(SeriesTitle.ReleaseDate))!.IsNullable);
        Assert.False(entity.FindProperty(nameof(SeriesTitle.IsRead))!.IsNullable);
        Assert.Null(entity.FindProperty("State"));
        Assert.Equal(DeleteBehavior.Cascade, entity.GetForeignKeys().Single().DeleteBehavior);
        Assert.True(entity.GetIndexes().Single(index => index.Properties.Select(property => property.Name)
            .SequenceEqual([nameof(SeriesTitle.SeriesItemId), nameof(SeriesTitle.Position)])).IsUnique);
    }

    [Fact]
    public async Task Persistence_LoadsTitlesInOrderAndCascadeDeletesThem()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = CreateSeries(2,
            Upcoming("Second"),
            Read("First"));
        series.Titles.ElementAt(0).Position = 1;
        series.Titles.ElementAt(1).Position = 0;
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();

        var loaded = await dbContext.Series.AsNoTracking().Include(item => item.Titles).SingleAsync();
        Assert.Equal(["First", "Second"], loaded.Titles.OrderBy(title => title.Position).Select(title => title.Title));
        Assert.True(loaded.Titles.Single(title => title.Title == "First").IsRead);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now).AddDays(1), loaded.Titles.Single(title => title.Title == "Second").ReleaseDate);

        dbContext.Series.Remove(series);
        await dbContext.SaveChangesAsync();
        Assert.Empty(await dbContext.SeriesTitles.ToListAsync());
    }

    [Fact]
    public async Task Persistence_RejectsDuplicatePositionsWithinSeries()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = CreateSeries(2,
            Read("One"),
            Read("Two"));
        series.Titles.ElementAt(1).Position = 0;
        dbContext.Series.Add(series);

        await Assert.ThrowsAsync<DbUpdateException>(() => dbContext.SaveChangesAsync());
    }

    [Fact]
    public async Task DatabaseInitializer_ResetsLegacySchemaAndPreservesCompatibleData()
    {
        var databasePath = Path.Combine(Path.GetTempPath(), $"series-tracker-{Guid.NewGuid():N}.db");
        try
        {
            await using (var legacyConnection = new SqliteConnection($"Data Source={databasePath}"))
            {
                await legacyConnection.OpenAsync();
                var command = legacyConnection.CreateCommand();
                command.CommandText = "CREATE TABLE Series (Id INTEGER PRIMARY KEY, Title TEXT NOT NULL); INSERT INTO Series VALUES (1, 'Legacy');";
                await command.ExecuteNonQueryAsync();
            }

            await using (var resetContext = CreateDbContext(databasePath))
            {
                await DatabaseInitializer.InitializeAsync(resetContext);
                Assert.Empty(await resetContext.Series.ToListAsync());
                resetContext.Series.Add(CreateSeries(1, Read("Fresh")));
                await resetContext.SaveChangesAsync();
            }

            await using (var retainedContext = CreateDbContext(databasePath))
            {
                await DatabaseInitializer.InitializeAsync(retainedContext);
                Assert.Equal("Series", (await retainedContext.Series.Include(series => series.Titles).SingleAsync()).Title);
            }
        }
        finally
        {
            File.Delete(databasePath);
        }
    }

    [Fact]
    public async Task DatabaseInitializer_ResetsOldTitleStates()
    {
        var databasePath = Path.Combine(Path.GetTempPath(), $"series-tracker-{Guid.NewGuid():N}.db");
        try
        {
            await using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE Series (Id INTEGER PRIMARY KEY, Title TEXT NOT NULL, PlannedLength INTEGER NOT NULL, IsDropped INTEGER NOT NULL); " +
                    "CREATE TABLE SeriesTitles (Id INTEGER PRIMARY KEY, SeriesItemId INTEGER NOT NULL, Title TEXT NOT NULL, Position INTEGER NOT NULL, State TEXT NOT NULL); " +
                    "INSERT INTO Series VALUES (1, 'Old', 1, 0); " +
                    "INSERT INTO SeriesTitles VALUES (1, 1, 'Old book', 0, 'Released');";
                await command.ExecuteNonQueryAsync();
            }

            await using var dbContext = CreateDbContext(databasePath);
            await DatabaseInitializer.InitializeAsync(dbContext);
            Assert.Empty(await dbContext.Series.ToListAsync());
            Assert.NotNull(dbContext.Model.FindEntityType(typeof(SeriesTitle))!.FindProperty(nameof(SeriesTitle.ReleaseDate)));
            dbContext.Series.Add(CreateSeries(1, Read("Fresh")));
            await dbContext.SaveChangesAsync();
        }
        finally
        {
            File.Delete(databasePath);
        }
    }

    [Fact]
    public async Task CreatePage_SavesOrderedTitles()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var page = new CreateModel(dbContext)
        {
            Form = ValidForm()
        };
        page.Form.Titles =
        [
            new SeriesTitleDraft { Title = "First", ReleaseDate = DateOnly.FromDateTime(DateTime.Now), IsRead = true },
            new SeriesTitleDraft { Title = "Second", ReleaseDate = DateOnly.FromDateTime(DateTime.Now) }
        ];

        var result = await page.OnPostAsync();

        Assert.IsType<RedirectToPageResult>(result);
        var saved = await dbContext.Series.Include(series => series.Titles).SingleAsync();
        Assert.Equal(["First", "Second"], saved.Titles.OrderBy(title => title.Position).Select(title => title.Title));
        Assert.True(saved.Titles.Single(title => title.Title == "First").IsRead);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), saved.Titles.Single(title => title.Title == "Second").ReleaseDate);
    }

    [Fact]
    public async Task CreatePage_PreservesInvalidDraft()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var page = new CreateModel(dbContext) { Form = ValidForm() };
        page.Form.Titles.Add(new SeriesTitleDraft());

        var result = await page.OnPostAsync();

        Assert.IsType<PageResult>(result);
        Assert.Single(page.Form.Titles);
        Assert.Empty(await dbContext.Series.ToListAsync());
    }

    [Fact]
    public async Task CreatePage_PreservesInvalidReadAndReleaseDate()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var page = new CreateModel(dbContext) { Form = ValidForm() };
        page.Form.Titles.Add(new SeriesTitleDraft
        {
            Title = "Later",
            ReleaseDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1),
            IsRead = true
        });

        Assert.IsType<PageResult>(await page.OnPostAsync());
        Assert.Contains("release date", page.Message, StringComparison.Ordinal);
        Assert.True(page.Form.Titles[0].IsRead);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now).AddDays(1), page.Form.Titles[0].ReleaseDate);
        Assert.Empty(await dbContext.Series.ToListAsync());
    }

    [Fact]
    public async Task CreatePage_RejectsUnparseableReleaseDate()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var page = new CreateModel(dbContext) { Form = ValidForm() };
        page.Form.Titles.Add(new SeriesTitleDraft { Title = "Invalid date" });
        page.ModelState.AddModelError("Form.Titles[0].ReleaseDate", "Invalid release date.");

        Assert.IsType<PageResult>(await page.OnPostAsync());
        Assert.Single(page.Form.Titles);
        Assert.Empty(await dbContext.Series.ToListAsync());
    }

    [Fact]
    public async Task EditPage_LoadsAndReplacesOrderedTitles()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = CreateSeries(3, Available("Old"));
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var page = new EditModel(dbContext);

        Assert.IsType<PageResult>(await page.OnGetAsync(series.Id));
        Assert.Equal("Old", page.Form.Titles.Single().Title);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), page.Form.Titles.Single().ReleaseDate);

        page.Form.Titles =
        [
            new SeriesTitleDraft { Title = "New first", ReleaseDate = DateOnly.FromDateTime(DateTime.Now), IsRead = true },
            new SeriesTitleDraft { Title = "New second" }
        ];
        page.Form.IsDropped = true;
        var result = await page.OnPostAsync(series.Id);

        Assert.IsType<RedirectToPageResult>(result);
        var updated = await dbContext.Series.AsNoTracking().Include(item => item.Titles).SingleAsync();
        Assert.True(updated.IsDropped);
        Assert.Equal(["New first", "New second"], updated.Titles.OrderBy(title => title.Position).Select(title => title.Title));
        Assert.True(updated.Titles.Single(title => title.Title == "New first").IsRead);
        Assert.Null(updated.Titles.Single(title => title.Title == "New second").ReleaseDate);
    }

    [Fact]
    public async Task EditPage_DeleteRemovesSeries()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = CreateSeries(1, Read("One"));
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();

        var result = await new EditModel(dbContext).OnPostDeleteAsync(series.Id);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.Empty(await dbContext.Series.ToListAsync());
    }

    [Fact]
    public async Task Dashboard_FiltersAndOrdersDerivedSeries()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var next = CreateSeries(3, Read("Read"), Available("Next"));
        next.Title = "Has next";
        var current = CreateSeries(3, Read("Read"), Upcoming("Soon"));
        current.Title = "Up to date";
        var complete = CreateSeries(1, Read("Done"));
        complete.Title = "Complete";
        var dropped = CreateSeries(2, Read("Read"));
        dropped.Title = "Dropped";
        dropped.IsDropped = true;
        dbContext.Series.AddRange(next, current, complete, dropped);
        await dbContext.SaveChangesAsync();

        var allPage = new IndexModel(dbContext);
        await allPage.OnGetAsync();
        Assert.Equal(["Has next", "Up to date", "Complete", "Dropped"], allPage.SeriesItems.Select(series => series.Title));

        var toReadPage = new IndexModel(dbContext) { StatusFilter = "To read" };
        await toReadPage.OnGetAsync();
        Assert.Equal("Has next", Assert.Single(toReadPage.SeriesItems).Title);

        var completedPage = new IndexModel(dbContext) { StatusFilter = "Completed" };
        await completedPage.OnGetAsync();
        Assert.Equal("Complete", Assert.Single(completedPage.SeriesItems).Title);
    }

    [Theory]
    [InlineData("All", 4)]
    [InlineData("To read", 1)]
    [InlineData("Up to date", 1)]
    [InlineData("Completed", 1)]
    [InlineData("Dropped", 1)]
    public void Dashboard_FilterMatchesDerivedCriteria(string filter, int expectedCount)
    {
        var toRead = CreateSeries(3, Read("Read"), Available("Next"));
        var upToDate = CreateSeries(3, Read("Read"), Upcoming("Soon"));
        var complete = CreateSeries(1, Read("Done"));
        var dropped = CreateSeries(2, Read("Read"));
        dropped.IsDropped = true;

        var result = IndexModel.ApplyFilter([toRead, upToDate, complete, dropped], filter).ToList();

        Assert.Equal(expectedCount, result.Count);
    }

    [Fact]
    public void Dashboard_ToReadFilterChangesOnReleaseDay()
    {
        var today = new DateOnly(2026, 10, 1);
        var series = CreateSeries(2, ("Dated", today.AddDays(1), false), ("Unknown", null, false));

        Assert.Empty(IndexModel.ApplyFilter([series], "To read", today));
        Assert.Single(IndexModel.ApplyFilter([series], "To read", today.AddDays(1)));
        Assert.Equal("Next: Dated", series.GetDashboardSecondaryText(today.AddDays(1)));
    }

    private static SeriesItem CreateSeries(int plannedLength, params (string Title, DateOnly? ReleaseDate, bool IsRead)[] titles)
    {
        return new SeriesItem
        {
            Title = "Series",
            Author = "Author",
            PlannedLength = plannedLength,
            Titles = titles.Select((title, position) => new SeriesTitle
            {
                Title = title.Title,
                ReleaseDate = title.ReleaseDate,
                IsRead = title.IsRead,
                Position = position
            }).ToList()
        };
    }

    private static (string Title, DateOnly? ReleaseDate, bool IsRead) Read(string title) =>
        (title, DateOnly.FromDateTime(DateTime.Now), true);

    private static (string Title, DateOnly? ReleaseDate, bool IsRead) Available(string title) =>
        (title, DateOnly.FromDateTime(DateTime.Now), false);

    private static (string Title, DateOnly? ReleaseDate, bool IsRead) Upcoming(string title) =>
        (title, DateOnly.FromDateTime(DateTime.Now).AddDays(1), false);

    private static SeriesFormModel ValidForm()
    {
        return new SeriesFormModel
        {
            Title = "Series",
            Author = "Author",
            PlannedLength = 3
        };
    }

    private static async Task<SqliteConnection> OpenMemoryConnectionAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        return connection;
    }

    private static SeriesTrackerDbContext CreateDbContext(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<SeriesTrackerDbContext>().UseSqlite(connection).Options;
        return new SeriesTrackerDbContext(options);
    }

    private static SeriesTrackerDbContext CreateDbContext(string databasePath)
    {
        var options = new DbContextOptionsBuilder<SeriesTrackerDbContext>()
            .UseSqlite($"Data Source={databasePath}")
            .Options;
        return new SeriesTrackerDbContext(options);
    }
}