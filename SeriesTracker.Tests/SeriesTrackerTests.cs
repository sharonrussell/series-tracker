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
    public void GetProgressPercent_ReturnsZero_WhenTotalIsZero()
    {
        var series = new SeriesItem { CurrentProgress = 0, TotalProgress = 0 };

        var result = series.GetProgressPercent();

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetProgressPercent_UsesSeriesLengthAsDenominator()
    {
        var series = new SeriesItem { CurrentProgress = 7, CurrentReleasedCount = 10, TotalProgress = 20 };

        var result = series.GetProgressPercent();

        Assert.Equal(35, result);
    }

    [Fact]
    public void GetProgressPercent_DoesNotTreatAllReleasedBooksAsComplete()
    {
        var series = new SeriesItem { CurrentProgress = 5, CurrentReleasedCount = 5, TotalProgress = 10 };

        var result = series.GetProgressPercent();

        Assert.Equal(50, result);
    }

    [Fact]
    public void GetProgressFraction_ReturnsCurrentOverSeriesLength()
    {
        var series = new SeriesItem { CurrentProgress = 7, CurrentReleasedCount = 10, TotalProgress = 20 };

        var result = series.GetProgressFraction();

        Assert.Equal("7/20", result);
    }

    [Theory]
    [InlineData(SeriesStatus.Dropped, 0, 0, 10, SeriesCompletionState.Ongoing)]
    [InlineData(SeriesStatus.NotStarted, 0, 0, 10, SeriesCompletionState.Ongoing)]
    [InlineData(SeriesStatus.Reading, 2, 5, 10, SeriesCompletionState.Ongoing)]
    [InlineData(SeriesStatus.UpToDate, 5, 5, 10, SeriesCompletionState.Ongoing)]
    [InlineData(SeriesStatus.Completed, 10, 10, 10, SeriesCompletionState.Completed)]
    public void GetDerivedStatus_ReturnsExpectedState(SeriesStatus storedStatus, int currentProgress, int currentReleasedCount, int totalProgress, SeriesCompletionState completionState)
    {
        var series = new SeriesItem
        {
            Status = storedStatus,
            CurrentProgress = currentProgress,
            CurrentReleasedCount = currentReleasedCount,
            TotalProgress = totalProgress,
            CompletionState = completionState
        };

        Assert.Equal(storedStatus, series.GetDerivedStatus());
    }

    [Fact]
    public void GetDerivedStatus_DoesNotRemainCompleted_WhenProgressIsReduced()
    {
        var series = new SeriesItem
        {
            Status = SeriesStatus.Completed,
            CurrentProgress = 4,
            CurrentReleasedCount = 10,
            TotalProgress = 10,
            CompletionState = SeriesCompletionState.Completed
        };

        Assert.Equal(SeriesStatus.Reading, series.GetDerivedStatus());
    }

    [Fact]
    public void GetDashboardProgressText_ReturnsDropped_ForDroppedSeries()
    {
        var series = new SeriesItem { CurrentProgress = 3, CurrentReleasedCount = 5, TotalProgress = 10, Status = SeriesStatus.Dropped };

        var result = series.GetDashboardProgressText();

        Assert.Equal("3/10", result);
    }

    [Fact]
    public void GetDashboardProgressText_ReturnsProgress_ForReadingSeries()
    {
        var series = new SeriesItem { CurrentProgress = 3, CurrentReleasedCount = 5, TotalProgress = 10, Status = SeriesStatus.Reading };

        var result = series.GetDashboardProgressText();

        Assert.Equal("3/10", result);
    }

    [Fact]
    public void GetAvailableToReadCount_ReturnsReleasedBooksNotRead()
    {
        var series = new SeriesItem { CurrentProgress = 3, CurrentReleasedCount = 10, TotalProgress = 20 };

        var result = series.GetAvailableToReadCount();

        Assert.Equal(7, result);
    }

    [Fact]
    public void GetAvailableToReadCount_NeverReturnsNegative()
    {
        var series = new SeriesItem { CurrentProgress = 12, CurrentReleasedCount = 12, TotalProgress = 20 };

        var result = series.GetAvailableToReadCount();

        Assert.Equal(0, result);
    }

    [Fact]
    public void IsUpToDate_ReturnsTrue_WhenAllReleasedBooksAreReadButSeriesContinues()
    {
        var series = new SeriesItem { CurrentProgress = 10, CurrentReleasedCount = 10, TotalProgress = 20 };

        var result = series.IsUpToDate();

        Assert.True(result);
    }

    [Fact]
    public void GetDisplayCompletionState_ReturnsCompleted_WhenProgressIsFull()
    {
        var series = new SeriesItem { CurrentProgress = 20, CurrentReleasedCount = 20, TotalProgress = 20, CompletionState = SeriesCompletionState.Ongoing };

        var result = series.GetDisplayCompletionState();

        Assert.Equal(SeriesCompletionState.Completed, result);
    }

    [Theory]
    [InlineData(4, 10, SeriesCompletionState.Ongoing)]
    [InlineData(10, 10, SeriesCompletionState.Completed)]
    public void GetDerivedCompletionState_ReturnsExpectedPublicationState(int currentReleasedCount, int totalProgress, SeriesCompletionState expected)
    {
        var series = new SeriesItem { CurrentReleasedCount = currentReleasedCount, TotalProgress = totalProgress };

        var result = series.GetDerivedCompletionState();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void SeriesStatus_DefaultsToReading()
    {
        var series = new SeriesItem();

        Assert.Equal(SeriesStatus.Reading, series.Status);
    }

    [Fact]
    public void SeriesMetadata_HasExpectedDefaults()
    {
        var series = new SeriesItem();

        Assert.Equal(string.Empty, series.Author);
        Assert.Equal(SeriesCompletionState.Ongoing, series.CompletionState);
    }

    [Fact]
    public void ValidateSeriesDraft_ReturnsError_WhenTitleIsMissing()
    {
        var result = IndexModel.ValidateSeriesDraft(string.Empty, "Author", 10, 5, 3, SeriesCompletionState.Ongoing);

        Assert.Equal("Series title is required.", result);
    }

    [Fact]
    public void ValidateSeriesDraft_ReturnsError_WhenAuthorIsMissing()
    {
        var result = IndexModel.ValidateSeriesDraft("The Hobbit", string.Empty, 10, 5, 3, SeriesCompletionState.Ongoing);

        Assert.Equal("Author is required.", result);
    }

    [Fact]
    public void ValidateSeriesDraft_ReturnsError_WhenCompletionStateIsInvalid()
    {
        var result = IndexModel.ValidateSeriesDraft("The Hobbit", "Author", 10, 5, 3, (SeriesCompletionState)99);

        Assert.Equal("Series completion state must be ongoing or completed.", result);
    }

    [Fact]
    public void ValidateSeriesDraft_ReturnsNull_WhenDraftIsValid()
    {
        var result = IndexModel.ValidateSeriesDraft("The Hobbit", "Author", 10, 5, 3, SeriesCompletionState.Completed);

        Assert.Null(result);
    }

    [Fact]
    public void ValidateSeriesDraft_ReturnsError_WhenReleasedCountExceedsSeriesLength()
    {
        var result = IndexModel.ValidateSeriesDraft("The Hobbit", "Author", 10, 11, 3, SeriesCompletionState.Ongoing);

        Assert.Equal("Released so far must be between 0 and the series length.", result);
    }

    [Fact]
    public void ValidateSeriesDraft_ReturnsError_WhenBooksReadExceedsReleasedCount()
    {
        var result = IndexModel.ValidateSeriesDraft("The Hobbit", "Author", 10, 5, 6, SeriesCompletionState.Ongoing);

        Assert.Equal("Books read so far must be between 0 and the released count.", result);
    }

    [Fact]
    public async Task OnPostAddAsync_SavesSeriesMetadata()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var pageModel = new IndexModel(dbContext)
        {
            Form = new IndexModel.SeriesFormModel
            {
                Title = "Test Series",
                Author = "Test Author",
                CurrentProgress = 2,
                CurrentReleasedCount = 3,
                TotalProgress = 5,
                CompletionState = SeriesCompletionState.Ongoing
            }
        };

        await pageModel.OnPostAddAsync();

        var series = await dbContext.Series.SingleAsync();
        Assert.Equal("Test Series", series.Title);
        Assert.Equal("Test Author", series.Author);
        Assert.Equal(3, series.CurrentReleasedCount);
        Assert.Equal(SeriesCompletionState.Ongoing, series.CompletionState);
    }

    [Fact]
    public async Task OnPostAddAsync_MarksReadingStatusCompleted_WhenProgressMatchesLengthForCompletedPublication()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var pageModel = new IndexModel(dbContext)
        {
            Form = new IndexModel.SeriesFormModel
            {
                Title = "Test Series",
                Author = "Test Author",
                CurrentProgress = 5,
                CurrentReleasedCount = 5,
                TotalProgress = 5,
                CompletionState = SeriesCompletionState.Completed
            }
        };

        await pageModel.OnPostAddAsync();

        var series = await dbContext.Series.SingleAsync();
        Assert.Equal(SeriesStatus.Completed, series.Status);
        Assert.Equal(SeriesCompletionState.Completed, series.CompletionState);
    }

    [Fact]
    public async Task OnPostAddAsync_DerivesCompletedStatus_WhenProgressMatchesSeriesLength()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var pageModel = new IndexModel(dbContext)
        {
            Form = new IndexModel.SeriesFormModel
            {
                Title = "Test Series",
                Author = "Test Author",
                CurrentProgress = 5,
                CurrentReleasedCount = 5,
                TotalProgress = 5,
                CompletionState = SeriesCompletionState.Ongoing
            }
        };

        await pageModel.OnPostAddAsync();

        var series = await dbContext.Series.SingleAsync();
        Assert.Equal(SeriesStatus.Completed, series.Status);
        Assert.Equal(SeriesCompletionState.Completed, series.CompletionState);
    }

    [Fact]
    public async Task OnPostUpdateAsync_IncrementsProgress_WhenBelowSeriesLength()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = new SeriesItem
        {
            Title = "Test Series",
            Author = "Test Author",
            TotalProgress = 5,
            CurrentReleasedCount = 5,
            CurrentProgress = 3
        };
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var pageModel = new IndexModel(dbContext);

        await pageModel.OnPostUpdateAsync(series.Id, incrementBy: 1);

        var updated = await dbContext.Series.SingleAsync();
        Assert.Equal(4, updated.CurrentProgress);
        Assert.Equal(SeriesStatus.Reading, updated.Status);
    }

    [Fact]
    public async Task OnPostUpdateAsync_CapsIncrementedProgress_AtSeriesLength()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = new SeriesItem
        {
            Title = "Test Series",
            Author = "Test Author",
            TotalProgress = 5,
            CurrentReleasedCount = 5,
            CurrentProgress = 5,
            CompletionState = SeriesCompletionState.Completed
        };
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var pageModel = new IndexModel(dbContext);

        await pageModel.OnPostUpdateAsync(series.Id, incrementBy: 1);

        var updated = await dbContext.Series.SingleAsync();
        Assert.Equal(5, updated.CurrentProgress);
        Assert.Equal(SeriesStatus.Completed, updated.Status);
        Assert.Equal(100, updated.GetProgressPercent());
    }

    [Fact]
    public async Task OnPostUpdateAsync_CapsIncrementedProgress_AtReleasedCount()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = new SeriesItem
        {
            Title = "Test Series",
            Author = "Test Author",
            TotalProgress = 10,
            CurrentReleasedCount = 4,
            CurrentProgress = 4,
            CompletionState = SeriesCompletionState.Ongoing
        };
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var pageModel = new IndexModel(dbContext);

        await pageModel.OnPostUpdateAsync(series.Id, incrementBy: 1);

        var updated = await dbContext.Series.SingleAsync();
        Assert.Equal(4, updated.CurrentProgress);
        Assert.Equal(40, updated.GetProgressPercent());
        Assert.Equal(SeriesStatus.UpToDate, updated.Status);
    }

    [Fact]
    public async Task OnPostUpdateAsync_MarksReadingStatusCompleted_WhenIncrementReachesLengthForCompletedPublication()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = new SeriesItem
        {
            Title = "Test Series",
            Author = "Test Author",
            TotalProgress = 5,
            CurrentReleasedCount = 5,
            CurrentProgress = 4,
            Status = SeriesStatus.Reading,
            CompletionState = SeriesCompletionState.Completed
        };
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var pageModel = new IndexModel(dbContext);

        await pageModel.OnPostUpdateAsync(series.Id, incrementBy: 1);

        var updated = await dbContext.Series.SingleAsync();
        Assert.Equal(5, updated.CurrentProgress);
        Assert.Equal(SeriesStatus.Completed, updated.Status);
        Assert.Equal(SeriesCompletionState.Completed, updated.CompletionState);
    }

    [Fact]
    public async Task OnPostUpdateAsync_DerivesCompletedStatus_WhenProgressReachesSeriesLength()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = new SeriesItem
        {
            Title = "Test Series",
            Author = "Test Author",
            TotalProgress = 5,
            CurrentReleasedCount = 5,
            CurrentProgress = 4,
            Status = SeriesStatus.Reading,
            CompletionState = SeriesCompletionState.Ongoing
        };
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var pageModel = new IndexModel(dbContext);

        await pageModel.OnPostUpdateAsync(series.Id, incrementBy: 1);

        var updated = await dbContext.Series.SingleAsync();
        Assert.Equal(5, updated.CurrentProgress);
        Assert.Equal(SeriesStatus.Completed, updated.Status);
        Assert.Equal(SeriesCompletionState.Completed, updated.CompletionState);
    }

    [Fact]
    public async Task OnPostUpdateAsync_CompletedStatusDoesNotOverrideDerivedProgress()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = new SeriesItem
        {
            Title = "Test Series",
            Author = "Test Author",
            TotalProgress = 5,
            CurrentReleasedCount = 5,
            CurrentProgress = 2,
            CompletionState = SeriesCompletionState.Ongoing
        };
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var pageModel = new IndexModel(dbContext);

        await pageModel.OnPostUpdateAsync(series.Id, status: SeriesStatus.Completed);

        var updated = await dbContext.Series.SingleAsync();
        Assert.Equal(2, updated.CurrentProgress);
        Assert.Equal(SeriesStatus.Reading, updated.Status);
        Assert.Equal(SeriesCompletionState.Completed, updated.CompletionState);
    }

    [Fact]
    public async Task OnPostAsync_UpdatesSeriesMetadata()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = new SeriesItem
        {
            Title = "Original Series",
            Author = "Original Author",
            TotalProgress = 4,
            CurrentReleasedCount = 2,
            CurrentProgress = 1,
            CompletionState = SeriesCompletionState.Ongoing
        };
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var pageModel = new EditModel(dbContext)
        {
            Form = new EditModel.EditFormModel
            {
                Id = series.Id,
                Title = "Updated Series",
                Author = "Updated Author",
                TotalProgress = 6,
                CurrentReleasedCount = 4,
                CurrentProgress = 3,
                Status = SeriesStatus.Reading,
                CompletionState = SeriesCompletionState.Completed
            }
        };

        await pageModel.OnPostAsync();

        var updated = await dbContext.Series.SingleAsync();
        Assert.Equal("Updated Series", updated.Title);
        Assert.Equal("Updated Author", updated.Author);
        Assert.Equal(4, updated.CurrentReleasedCount);
        Assert.Equal(SeriesStatus.Reading, updated.Status);
        Assert.Equal(SeriesCompletionState.Completed, updated.CompletionState);
    }

    [Fact]
    public async Task OnPostAsync_MarksReadingStatusCompleted_WhenProgressMatchesLengthForCompletedPublication()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = new SeriesItem
        {
            Title = "Original Series",
            Author = "Original Author",
            TotalProgress = 4,
            CurrentReleasedCount = 2,
            CurrentProgress = 1,
            Status = SeriesStatus.Reading,
            CompletionState = SeriesCompletionState.Ongoing
        };
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var pageModel = new EditModel(dbContext)
        {
            Form = new EditModel.EditFormModel
            {
                Id = series.Id,
                Title = "Original Series",
                Author = "Original Author",
                TotalProgress = 4,
                CurrentReleasedCount = 4,
                CurrentProgress = 4,
                Status = SeriesStatus.Reading,
                CompletionState = SeriesCompletionState.Completed
            }
        };

        await pageModel.OnPostAsync();

        var updated = await dbContext.Series.SingleAsync();
        Assert.Equal(SeriesStatus.Completed, updated.Status);
        Assert.Equal(SeriesCompletionState.Completed, updated.CompletionState);
    }

    [Fact]
    public async Task OnGetAsync_LoadsSharedEditorFormForEditId()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = new SeriesItem
        {
            Title = "Shared Editor Series",
            Author = "Test Author",
            TotalProgress = 8,
            CurrentReleasedCount = 5,
            CurrentProgress = 3,
            Status = SeriesStatus.Dropped,
            CompletionState = SeriesCompletionState.Ongoing
        };
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var pageModel = new IndexModel(dbContext) { EditId = series.Id };

        await pageModel.OnGetAsync();

        Assert.True(pageModel.IsEditMode);
        Assert.Equal(series.Id, pageModel.Form.Id);
        Assert.Equal(SeriesStatus.Dropped, pageModel.Form.Status);
        Assert.Equal("Shared Editor Series", pageModel.Form.Title);
    }

    [Fact]
    public async Task OnGetAsync_ReadingFilterIncludesDerivedOngoingStates()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        dbContext.Series.AddRange(
            new SeriesItem { Title = "Not Started", Author = "Test", TotalProgress = 5, CurrentReleasedCount = 2, CurrentProgress = 0 },
            new SeriesItem { Title = "Reading", Author = "Test", TotalProgress = 5, CurrentReleasedCount = 3, CurrentProgress = 1 },
            new SeriesItem { Title = "Up To Date", Author = "Test", TotalProgress = 5, CurrentReleasedCount = 3, CurrentProgress = 3 },
            new SeriesItem { Title = "Completed", Author = "Test", TotalProgress = 5, CurrentReleasedCount = 5, CurrentProgress = 5, CompletionState = SeriesCompletionState.Completed },
            new SeriesItem { Title = "Dropped", Author = "Test", TotalProgress = 5, CurrentReleasedCount = 5, CurrentProgress = 1, Status = SeriesStatus.Dropped });
        await dbContext.SaveChangesAsync();
        var pageModel = new IndexModel(dbContext) { StatusFilter = "Reading" };

        await pageModel.OnGetAsync();

        Assert.Equal(["Up To Date", "Reading", "Not Started"], pageModel.SeriesItems.Select(series => series.Title).ToArray());
    }

    [Fact]
    public async Task OnGetAsync_AllFilterPrioritizesActionableSeriesBeforeUpToDateAndFinishedStates()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        dbContext.Series.AddRange(
            new SeriesItem { Title = "Completed", Author = "Test", TotalProgress = 5, CurrentReleasedCount = 5, CurrentProgress = 5 },
            new SeriesItem { Title = "Up To Date", Author = "Test", TotalProgress = 5, CurrentReleasedCount = 3, CurrentProgress = 3 },
            new SeriesItem { Title = "Books To Read", Author = "Test", TotalProgress = 5, CurrentReleasedCount = 3, CurrentProgress = 1 },
            new SeriesItem { Title = "Not Released", Author = "Test", TotalProgress = 5, CurrentReleasedCount = 0, CurrentProgress = 0 },
            new SeriesItem { Title = "Dropped", Author = "Test", TotalProgress = 5, CurrentReleasedCount = 3, CurrentProgress = 1, Status = SeriesStatus.Dropped });
        await dbContext.SaveChangesAsync();
        var pageModel = new IndexModel(dbContext) { StatusFilter = "All" };

        await pageModel.OnGetAsync();

        Assert.Equal(["Books To Read", "Up To Date", "Not Released", "Completed", "Dropped"], pageModel.SeriesItems.Select(series => series.Title).ToArray());
    }

    [Fact]
    public async Task OnPostSaveAsync_CreatesNewSeriesAsReading()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var pageModel = new IndexModel(dbContext)
        {
            Form = new IndexModel.SeriesFormModel
            {
                Title = "New Shared Series",
                Author = "Test Author",
                TotalProgress = 10,
                CurrentReleasedCount = 4,
                CurrentProgress = 2,
                Status = SeriesStatus.Reading,
                CompletionState = SeriesCompletionState.Ongoing
            }
        };

        await pageModel.OnPostSaveAsync();

        var created = await dbContext.Series.SingleAsync();
        Assert.Equal(SeriesStatus.Reading, created.Status);
        Assert.Equal(2, created.CurrentProgress);
    }

    [Fact]
    public async Task OnPostSaveAsync_ReducingCompletedProgressDerivesReading()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var series = new SeriesItem
        {
            Title = "Completed Series",
            Author = "Test Author",
            TotalProgress = 10,
            CurrentReleasedCount = 10,
            CurrentProgress = 10,
            Status = SeriesStatus.Completed,
            CompletionState = SeriesCompletionState.Completed
        };
        dbContext.Series.Add(series);
        await dbContext.SaveChangesAsync();
        var pageModel = new IndexModel(dbContext)
        {
            Form = new IndexModel.SeriesFormModel
            {
                Id = series.Id,
                Title = series.Title,
                Author = series.Author,
                TotalProgress = 10,
                CurrentReleasedCount = 10,
                CurrentProgress = 4,
                CompletionState = SeriesCompletionState.Completed,
                Status = SeriesStatus.Reading
            }
        };

        await pageModel.OnPostSaveAsync();

        var updated = await dbContext.Series.SingleAsync();
        Assert.Equal(SeriesStatus.Reading, updated.Status);
    }

    [Fact]
    public async Task OnPostSaveAsync_FullProgressDerivesCompletionState()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var dbContext = CreateDbContext(connection);
        await dbContext.Database.EnsureCreatedAsync();
        var pageModel = new IndexModel(dbContext)
        {
            Form = new IndexModel.SeriesFormModel
            {
                Title = "Full Progress Series",
                Author = "Test Author",
                TotalProgress = 10,
                CurrentReleasedCount = 10,
                CurrentProgress = 10,
                CompletionState = SeriesCompletionState.Ongoing
            }
        };

        await pageModel.OnPostSaveAsync();

        var created = await dbContext.Series.SingleAsync();
        Assert.Equal(10, created.CurrentReleasedCount);
        Assert.Equal(SeriesCompletionState.Completed, created.CompletionState);
        Assert.Equal(SeriesStatus.Completed, created.Status);
    }

    private static SeriesTrackerDbContext CreateDbContext(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<SeriesTrackerDbContext>().UseSqlite(connection).Options;
        return new SeriesTrackerDbContext(options);
    }
}
