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
        tooMany.PlannedLength = 2;
        tooMany.Titles =
        [
            new SeriesTitleDraft { Title = "One" },
            new SeriesTitleDraft { Title = "Two" },
            new SeriesTitleDraft { Title = "Three" }
        ];
        Assert.Equal("Known titles cannot exceed the planned series length.", SeriesEditorPageModel.ValidateSeriesDraft(tooMany));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void ValidateSeriesDraft_RequiresAtLeastTwoPlannedTitles(int plannedLength)
    {
        var form = ValidForm();
        form.PlannedLength = plannedLength;

        Assert.Equal("Planned series length must be at least 2.", SeriesEditorPageModel.ValidateSeriesDraft(form));
        Assert.Equal(plannedLength, form.PlannedLength);
    }

    [Fact]
    public void SeriesAndCreateForm_DefaultToTwoPlannedTitles()
    {
        Assert.Equal(2, new SeriesItem().PlannedLength);
        Assert.Equal(2, new SeriesFormModel().PlannedLength);
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
    public async Task CreatePage_RequiresTwoPlannedTitlesWithoutLosingTheDraft()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var page = new CreateModel(dbContext) { Form = ValidForm() };
        page.Form.PlannedLength = 1;
        page.Form.Titles.Add(new SeriesTitleDraft { Title = "Known title" });

        Assert.IsType<PageResult>(await page.OnPostAsync());
        Assert.Equal(1, page.Form.PlannedLength);
        Assert.Equal("Known title", Assert.Single(page.Form.Titles).Title);
        Assert.Empty(await dbContext.Series.ToListAsync());

        page.Form.PlannedLength = 2;
        Assert.IsType<RedirectToPageResult>(await page.OnPostAsync());
        Assert.Equal(2, (await dbContext.Series.SingleAsync()).PlannedLength);
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
    public async Task EditPage_RequiresCorrectionOfAnExistingOneTitlePlan()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = CreateSeries(1, Read("Legacy title"));
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var page = new EditModel(dbContext);

        Assert.IsType<PageResult>(await page.OnGetAsync(series.Id));
        Assert.Equal(1, page.Form.PlannedLength);
        Assert.IsType<PageResult>(await page.OnPostAsync(series.Id));
        Assert.Equal(1, (await dbContext.Series.SingleAsync()).PlannedLength);
        Assert.Equal("Legacy title", Assert.Single(page.Form.Titles).Title);

        page.Form.PlannedLength = 2;
        Assert.IsType<RedirectToPageResult>(await page.OnPostAsync(series.Id));
        var updated = await dbContext.Series.Include(item => item.Titles).SingleAsync();
        Assert.Equal(2, updated.PlannedLength);
        Assert.Equal("Legacy title", Assert.Single(updated.Titles).Title);
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
        Assert.Equal("Has next", allPage.ReadingSummary!.Suggestion!.Series.Title);

        var toReadPage = new IndexModel(dbContext) { StatusFilter = "To read" };
        await toReadPage.OnGetAsync();
        Assert.Equal("Has next", Assert.Single(toReadPage.SeriesItems).Title);
        Assert.Null(toReadPage.ReadingSummary);

        var completedPage = new IndexModel(dbContext) { StatusFilter = "Completed" };
        await completedPage.OnGetAsync();
        Assert.Equal("Complete", Assert.Single(completedPage.SeriesItems).Title);
        Assert.Null(completedPage.ReadingSummary);
    }

    [Fact]
    public async Task Dashboard_EmptyAllViewHasAnEmptyReadNextSummary()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var page = new IndexModel(dbContext);

        await page.OnGetAsync();

        Assert.NotNull(page.ReadingSummary);
        Assert.Null(page.ReadingSummary.Suggestion);
        Assert.Empty(page.SeriesItems);
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

    [Fact]
    public void ReadNextSummary_RequiresEveryPlannedUnreadBookToBeAvailableToFinish()
    {
        var today = new DateOnly(2026, 10, 1);
        var finishable = CreateSeries(2, ("Read", today, true), ("Final book", today, false));
        var unannounced = CreateSeries(3, ("Read", today, true), ("Ready", today, false));

        var summary = IndexModel.BuildReadNextSummary([unannounced, finishable], today);

        var suggestion = Assert.IsType<ReadNextSuggestion>(summary.Suggestion);
        Assert.True(suggestion.CanComplete);
        Assert.Same(finishable, suggestion.Series);
        Assert.Equal("Final book", suggestion.NextTitle.Title);
        Assert.Equal(1, suggestion.RemainingCount);
    }

    [Fact]
    public void ReadNextSummary_ExcludesUnavailableAndInactiveTitlesFromCompletion()
    {
        var today = new DateOnly(2026, 10, 1);
        var finishable = CreateSeries(3, ("Read", today, true), ("Second", today, false), ("Third", today, false));
        var future = CreateSeries(3, ("Read", today, true), ("Ready", today, false), ("Later", today.AddDays(1), false));
        var unknown = CreateSeries(3, ("Read", today, true), ("Ready", today, false), ("Undated", null, false));
        var completed = CreateSeries(1, ("Done", today, true));
        var dropped = CreateSeries(1, ("Dropped book", today, false));
        dropped.IsDropped = true;

        var summary = IndexModel.BuildReadNextSummary([finishable, future, unknown, completed, dropped], today);

        var suggestion = Assert.IsType<ReadNextSuggestion>(summary.Suggestion);
        Assert.True(suggestion.CanComplete);
        Assert.Same(finishable, suggestion.Series);
        Assert.Equal("Second", suggestion.NextTitle.Title);
        Assert.Equal(2, suggestion.RemainingCount);
    }

    [Fact]
    public void ReadNextSummary_OnlySuggestsAvailableUnreadTitles()
    {
        var today = new DateOnly(2026, 10, 1);
        var future = CreateSeries(1, ("Later", today.AddDays(1), false));
        var unknown = CreateSeries(1, ("Undated", null, false));
        var completed = CreateSeries(1, ("Done", today, true));
        var dropped = CreateSeries(1, ("Dropped book", today, false));
        dropped.IsDropped = true;

        var summary = IndexModel.BuildReadNextSummary([future, unknown, completed, dropped], today);

        Assert.Null(summary.Suggestion);
    }

    [Fact]
    public void ReadNextSummary_RanksAndSelectsOneFinishableSuggestion()
    {
        var today = new DateOnly(2026, 10, 1);
        var latest = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc);
        var mostProgressed = CreateSeries(4, ("One", today, true), ("Two", today, true), ("Three", today, true), ("Finish", today, false));
        var fewerRemaining = CreateSeries(2, ("Read", today, true), ("Finish", today, false));
        var recentlyUpdated = CreateSeries(2, ("Read", today, true), ("Finish", today, false));
        var older = CreateSeries(2, ("Read", today, true), ("Finish", today, false));
        var lessProgressed = CreateSeries(1, ("Begin", today, false));
        mostProgressed.Id = 5;
        recentlyUpdated.Id = 4;
        fewerRemaining.Id = 2;
        older.Id = 1;
        recentlyUpdated.UpdatedAt = latest;
        fewerRemaining.UpdatedAt = latest.AddDays(-1);
        older.UpdatedAt = latest.AddDays(-1);

        var summary = IndexModel.BuildReadNextSummary([lessProgressed, fewerRemaining, older, recentlyUpdated, mostProgressed], today);

        Assert.Same(mostProgressed, summary.Suggestion!.Series);
        Assert.True(summary.Suggestion.CanComplete);
    }

    [Fact]
    public void ReadNextSummary_TiesPreferOlderReleaseOverFewerBooksAndRecentUpdate()
    {
        var today = new DateOnly(2026, 10, 1);
        var olderRelease = CreateSeries(4, ("One", today, true), ("Two", today, true),
            ("Read next", today.AddYears(-2), false), ("Finish", today, false));
        var newerRelease = CreateSeries(2, ("Read", today, true), ("Finish", today.AddDays(-1), false));
        olderRelease.UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        newerRelease.UpdatedAt = olderRelease.UpdatedAt.AddYears(1);

        var summary = IndexModel.BuildReadNextSummary([newerRelease, olderRelease], today);

        Assert.Same(olderRelease, summary.Suggestion!.Series);
        Assert.Equal("Read next", summary.Suggestion.NextTitle.Title);
    }

    [Fact]
    public void ReadNextSummary_FinishabilityAndProgressTakePriorityOverReleaseAge()
    {
        var today = new DateOnly(2026, 10, 1);
        var finishable = CreateSeries(2, ("Read", today, true), ("Finish", today, false));
        var olderNonfinishable = CreateSeries(4, ("One", today, true), ("Two", today, true),
            ("Overdue", today.AddYears(-2), false), ("Upcoming", today.AddDays(1), false));

        Assert.Same(finishable, IndexModel.BuildReadNextSummary([olderNonfinishable, finishable], today).Suggestion!.Series);

        var furtherAlong = CreateSeries(3, ("One", today, true), ("Two", today, true), ("Finish", today, false));
        finishable.Titles.Single(title => title.Title == "Finish").ReleaseDate = today.AddYears(-3);

        Assert.Same(furtherAlong, IndexModel.BuildReadNextSummary([finishable, furtherAlong], today).Suggestion!.Series);
    }

    [Fact]
    public void ReadNextSummary_EqualDatesRetainUpdateTimeAndStableIdFallbacks()
    {
        var today = new DateOnly(2026, 10, 1);
        var earlierId = CreateSeries(2, ("Read", today, true), ("Next", today, false));
        var recentlyUpdated = CreateSeries(2, ("Read", today, true), ("Next", today, false));
        earlierId.Id = 1;
        recentlyUpdated.Id = 2;
        earlierId.UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        recentlyUpdated.UpdatedAt = earlierId.UpdatedAt.AddDays(1);

        Assert.Same(recentlyUpdated, IndexModel.BuildReadNextSummary([earlierId, recentlyUpdated], today).Suggestion!.Series);

        earlierId.UpdatedAt = recentlyUpdated.UpdatedAt;
        Assert.Same(earlierId, IndexModel.BuildReadNextSummary([recentlyUpdated, earlierId], today).Suggestion!.Series);
    }

    [Fact]
    public void ReadNextSummary_ComparesSeriesOrderNextDateAndRespondsToDateChanges()
    {
        var today = new DateOnly(2026, 10, 1);
        var laterNext = CreateSeries(3, ("Read", today, true), ("Next", today.AddDays(-2), false),
            ("Older but later in series", today.AddYears(-3), false));
        var olderNext = CreateSeries(3, ("Read", today, true), ("Next", today.AddDays(-3), false),
            ("Last", today, false));

        Assert.Same(olderNext, IndexModel.BuildReadNextSummary([laterNext, olderNext], today).Suggestion!.Series);

        laterNext.Titles.Single(title => title.Title == "Next").ReleaseDate = today.AddDays(-4);
        Assert.Same(laterNext, IndexModel.BuildReadNextSummary([olderNext, laterNext], today).Suggestion!.Series);
    }

    [Fact]
    public void ReadNextSummary_TiesPreferFewerRemainingBooksBeforeRecency()
    {
        var today = new DateOnly(2026, 10, 1);
        var shorter = CreateSeries(2, ("Read", today, true), ("Finish", today, false));
        var longer = CreateSeries(4, ("One", today, true), ("Two", today, true),
            ("Three", today, false), ("Four", today, false));
        shorter.UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        longer.UpdatedAt = shorter.UpdatedAt.AddDays(1);

        var summary = IndexModel.BuildReadNextSummary([longer, shorter], today);

        Assert.Same(shorter, summary.Suggestion!.Series);
    }

    [Fact]
    public void ReadNextSummary_FallsBackToMostProgressedSeriesAndEarliestAvailableTitle()
    {
        var today = new DateOnly(2026, 10, 1);
        var lessProgressed = CreateSeries(4, ("Read", today, true), ("Ready", today, false));
        var mostProgressed = CreateSeries(5, ("Read first", today, true), ("Read second", today, true),
            ("Earliest ready", today, false), ("Later ready", today, false));

        var summary = IndexModel.BuildReadNextSummary([lessProgressed, mostProgressed], today);

        var suggestion = Assert.IsType<ReadNextSuggestion>(summary.Suggestion);
        Assert.False(suggestion.CanComplete);
        Assert.Same(mostProgressed, suggestion.Series);
        Assert.Equal("Earliest ready", suggestion.NextTitle.Title);
    }

    [Fact]
    public void ReadNextSummary_RecalculatesWhenTheReleaseDateArrives()
    {
        var today = new DateOnly(2026, 10, 1);
        var series = CreateSeries(2, ("Read", today, true), ("Tomorrow", today.AddDays(1), false));

        Assert.Null(IndexModel.BuildReadNextSummary([series], today).Suggestion);
        var tomorrow = IndexModel.BuildReadNextSummary([series], today.AddDays(1));
        Assert.Equal("Tomorrow", tomorrow.Suggestion!.NextTitle.Title);
        Assert.True(tomorrow.Suggestion.CanComplete);
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