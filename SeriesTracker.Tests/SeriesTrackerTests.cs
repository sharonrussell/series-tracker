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
    public void GetProgressPercent_ReturnsExpectedPercentage()
    {
        var series = new SeriesItem { CurrentProgress = 7, TotalProgress = 10 };

        var result = series.GetProgressPercent();

        Assert.Equal(70, result);
    }

    [Fact]
    public void ArchivedStatus_IsRecognized()
    {
        var series = new SeriesItem { Status = SeriesStatus.Archived };

        Assert.True(series.IsArchived);
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
        var result = IndexModel.ValidateSeriesDraft(string.Empty, "Author", 10, 3, SeriesCompletionState.Ongoing);

        Assert.Equal("Series title is required.", result);
    }

    [Fact]
    public void ValidateSeriesDraft_ReturnsError_WhenAuthorIsMissing()
    {
        var result = IndexModel.ValidateSeriesDraft("The Hobbit", string.Empty, 10, 3, SeriesCompletionState.Ongoing);

        Assert.Equal("Author is required.", result);
    }

    [Fact]
    public void ValidateSeriesDraft_ReturnsError_WhenCompletionStateIsInvalid()
    {
        var result = IndexModel.ValidateSeriesDraft("The Hobbit", "Author", 10, 3, (SeriesCompletionState)99);

        Assert.Equal("Series completion state must be ongoing or completed.", result);
    }

    [Fact]
    public void ValidateSeriesDraft_ReturnsNull_WhenDraftIsValid()
    {
        var result = IndexModel.ValidateSeriesDraft("The Hobbit", "Author", 10, 3, SeriesCompletionState.Completed);

        Assert.Null(result);
    }

    [Fact]
    public async Task OnPostAddAsync_SavesSeriesMetadata()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<SeriesTrackerDbContext>().UseSqlite(connection).Options;
        await using var dbContext = new SeriesTrackerDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        var pageModel = new IndexModel(dbContext)
        {
            Form = new IndexModel.SeriesFormModel
            {
                Title = "Test Series",
                Author = "Test Author",
                CurrentProgress = 2,
                TotalProgress = 5,
                CompletionState = SeriesCompletionState.Ongoing
            }
        };

        await pageModel.OnPostAddAsync();

        var series = await dbContext.Series.SingleAsync();
        Assert.Equal("Test Series", series.Title);
        Assert.Equal("Test Author", series.Author);
        Assert.Equal(SeriesCompletionState.Ongoing, series.CompletionState);
    }

    [Fact]
    public async Task OnPostAsync_UpdatesSeriesMetadata()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<SeriesTrackerDbContext>().UseSqlite(connection).Options;
        await using var dbContext = new SeriesTrackerDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        var series = new SeriesItem
        {
            Title = "Original Series",
            Author = "Original Author",
            TotalProgress = 4,
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
                CurrentProgress = 3,
                Status = SeriesStatus.Active,
                CompletionState = SeriesCompletionState.Completed
            }
        };

        await pageModel.OnPostAsync();

        var updated = await dbContext.Series.SingleAsync();
        Assert.Equal("Updated Series", updated.Title);
        Assert.Equal("Updated Author", updated.Author);
        Assert.Equal(SeriesCompletionState.Completed, updated.CompletionState);
    }
}
