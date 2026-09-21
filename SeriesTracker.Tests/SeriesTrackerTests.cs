using Series_Tracker.Models;

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
}
