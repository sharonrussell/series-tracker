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
    public void DerivedCounts_UseTitleStatesAndPlannedLength()
    {
        var series = CreateSeries(6,
            ("Read", SeriesTitleState.Read),
            ("Released", SeriesTitleState.Released),
            ("Upcoming", SeriesTitleState.Upcoming));

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
    public void GetDerivedStatus_UsesTitleStatePrecedence(bool dropped, int plannedLength, string states, SeriesStatus expected)
    {
        var titles = states.Select((state, index) => ($"Book {index + 1}", state switch
        {
            'R' => SeriesTitleState.Read,
            'L' => SeriesTitleState.Released,
            _ => SeriesTitleState.Upcoming
        })).ToArray();
        var series = CreateSeries(plannedLength, titles);
        series.IsDropped = dropped;

        Assert.Equal(expected, series.GetDerivedStatus());
    }

    [Fact]
    public void DashboardSecondaryText_UsesActionableTitleOrderAndFallbacks()
    {
        var series = CreateSeries(5,
            ("Already read", SeriesTitleState.Read),
            ("Later release", SeriesTitleState.Upcoming),
            ("Read next", SeriesTitleState.Released));

        Assert.Equal("Next: Read next", series.GetDashboardSecondaryText());

        series.Titles.Single(title => title.Title == "Read next").State = SeriesTitleState.Read;
        Assert.Equal("Upcoming: Later release", series.GetDashboardSecondaryText());

        series.Titles.Single(title => title.Title == "Later release").State = SeriesTitleState.Read;
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
            new SeriesTitleDraft { Title = "One", State = SeriesTitleState.Read },
            new SeriesTitleDraft { Title = "Two", State = SeriesTitleState.Released }
        ];

        Assert.Null(SeriesEditorPageModel.ValidateSeriesDraft(form));
    }

    [Fact]
    public void EfModel_ConfiguresRequiredCascadeAndUniquePosition()
    {
        using var connection = new SqliteConnection("Data Source=:memory:");
        var dbContext = CreateDbContext(connection);
        var entity = dbContext.Model.FindEntityType(typeof(SeriesTitle))!;

        Assert.False(entity.FindProperty(nameof(SeriesTitle.Title))!.IsNullable);
        Assert.Equal(200, entity.FindProperty(nameof(SeriesTitle.Title))!.GetMaxLength());
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
            ("Second", SeriesTitleState.Upcoming),
            ("First", SeriesTitleState.Read));
        series.Titles.ElementAt(0).Position = 1;
        series.Titles.ElementAt(1).Position = 0;
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();

        var loaded = await dbContext.Series.AsNoTracking().Include(item => item.Titles).SingleAsync();
        Assert.Equal(["First", "Second"], loaded.Titles.OrderBy(title => title.Position).Select(title => title.Title));

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
            ("One", SeriesTitleState.Read),
            ("Two", SeriesTitleState.Read));
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
                resetContext.Series.Add(CreateSeries(1, ("Fresh", SeriesTitleState.Read)));
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
            new SeriesTitleDraft { Title = "First", State = SeriesTitleState.Read },
            new SeriesTitleDraft { Title = "Second", State = SeriesTitleState.Released }
        ];

        var result = await page.OnPostAsync();

        Assert.IsType<RedirectToPageResult>(result);
        var saved = await dbContext.Series.Include(series => series.Titles).SingleAsync();
        Assert.Equal(["First", "Second"], saved.Titles.OrderBy(title => title.Position).Select(title => title.Title));
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
    public async Task EditPage_LoadsAndReplacesOrderedTitles()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = CreateSeries(3, ("Old", SeriesTitleState.Released));
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var page = new EditModel(dbContext);

        Assert.IsType<PageResult>(await page.OnGetAsync(series.Id));
        Assert.Equal("Old", page.Form.Titles.Single().Title);

        page.Form.Titles =
        [
            new SeriesTitleDraft { Title = "New first", State = SeriesTitleState.Read },
            new SeriesTitleDraft { Title = "New second", State = SeriesTitleState.Upcoming }
        ];
        page.Form.IsDropped = true;
        var result = await page.OnPostAsync(series.Id);

        Assert.IsType<RedirectToPageResult>(result);
        var updated = await dbContext.Series.AsNoTracking().Include(item => item.Titles).SingleAsync();
        Assert.True(updated.IsDropped);
        Assert.Equal(["New first", "New second"], updated.Titles.OrderBy(title => title.Position).Select(title => title.Title));
    }

    [Fact]
    public async Task EditPage_DeleteRemovesSeries()
    {
        await using var connection = await OpenMemoryConnectionAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = CreateSeries(1, ("One", SeriesTitleState.Read));
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
        var next = CreateSeries(3, ("Read", SeriesTitleState.Read), ("Next", SeriesTitleState.Released));
        next.Title = "Has next";
        var current = CreateSeries(3, ("Read", SeriesTitleState.Read), ("Soon", SeriesTitleState.Upcoming));
        current.Title = "Up to date";
        var complete = CreateSeries(1, ("Done", SeriesTitleState.Read));
        complete.Title = "Complete";
        var dropped = CreateSeries(2, ("Read", SeriesTitleState.Read));
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
        var toRead = CreateSeries(3, ("Read", SeriesTitleState.Read), ("Next", SeriesTitleState.Released));
        var upToDate = CreateSeries(3, ("Read", SeriesTitleState.Read), ("Soon", SeriesTitleState.Upcoming));
        var complete = CreateSeries(1, ("Done", SeriesTitleState.Read));
        var dropped = CreateSeries(2, ("Read", SeriesTitleState.Read));
        dropped.IsDropped = true;

        var result = IndexModel.ApplyFilter([toRead, upToDate, complete, dropped], filter).ToList();

        Assert.Equal(expectedCount, result.Count);
    }

    private static SeriesItem CreateSeries(int plannedLength, params (string Title, SeriesTitleState State)[] titles)
    {
        return new SeriesItem
        {
            Title = "Series",
            Author = "Author",
            PlannedLength = plannedLength,
            Titles = titles.Select((title, position) => new SeriesTitle
            {
                Title = title.Title,
                State = title.State,
                Position = position
            }).ToList()
        };
    }

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