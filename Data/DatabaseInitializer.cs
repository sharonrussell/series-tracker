using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace Series_Tracker.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(SeriesTrackerDbContext dbContext)
    {
        if (!dbContext.Database.IsSqlite())
        {
            await dbContext.Database.EnsureCreatedAsync();
            return;
        }

        var connection = dbContext.Database.GetDbConnection();
        var wasOpen = connection.State == ConnectionState.Open;
        if (!wasOpen)
        {
            await connection.OpenAsync();
        }

        var hasSeries = await TableExistsAsync(connection, "Series");
        var hasSeriesTitles = await TableExistsAsync(connection, "SeriesTitles");
        var seriesColumns = hasSeries
            ? await GetColumnsAsync(connection, "Series")
            : [];

        var isLegacySchema = hasSeries &&
            (!hasSeriesTitles || !seriesColumns.Contains("PlannedLength") || !seriesColumns.Contains("IsDropped"));

        if (!wasOpen)
        {
            await connection.CloseAsync();
        }

        if (isLegacySchema)
        {
            await dbContext.Database.EnsureDeletedAsync();
        }

        await dbContext.Database.EnsureCreatedAsync();
    }

    private static async Task<bool> TableExistsAsync(DbConnection connection, string tableName)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = $name;";
        var parameter = command.CreateParameter();
        parameter.ParameterName = "$name";
        parameter.Value = tableName;
        command.Parameters.Add(parameter);
        return Convert.ToInt32(await command.ExecuteScalarAsync(), CultureInfo.InvariantCulture) > 0;
    }

    private static async Task<HashSet<string>> GetColumnsAsync(DbConnection connection, string tableName)
    {
        var columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        await using var command = connection.CreateCommand();
        command.CommandText = $"PRAGMA table_info('{tableName}');";
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            columns.Add(reader.GetString(1));
        }

        return columns;
    }
}