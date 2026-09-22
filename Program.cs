using Microsoft.EntityFrameworkCore;
using System.Data;
using Series_Tracker.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<SeriesTrackerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=series-tracker.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SeriesTrackerDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
    await EnsureSeriesMetadataColumnsAsync(dbContext);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

static async Task EnsureSeriesMetadataColumnsAsync(SeriesTrackerDbContext dbContext)
{
    var connection = dbContext.Database.GetDbConnection();
    if (connection.State != ConnectionState.Open)
    {
        await connection.OpenAsync();
    }

    var columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    await using (var command = connection.CreateCommand())
    {
        command.CommandText = "PRAGMA table_info('Series');";
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            columns.Add(reader.GetString(1));
        }
    }

    if (!columns.Contains("Author"))
    {
        await dbContext.Database.ExecuteSqlRawAsync("ALTER TABLE \"Series\" ADD COLUMN \"Author\" TEXT NOT NULL DEFAULT '';");
    }

    if (!columns.Contains("CompletionState"))
    {
        await dbContext.Database.ExecuteSqlRawAsync("ALTER TABLE \"Series\" ADD COLUMN \"CompletionState\" TEXT NOT NULL DEFAULT 'Ongoing';");
    }

    await dbContext.Database.ExecuteSqlRawAsync("UPDATE \"Series\" SET \"Status\" = 'Reading' WHERE \"Status\" IN ('Active', 'Ongoing');");
    await dbContext.Database.ExecuteSqlRawAsync("UPDATE \"Series\" SET \"Status\" = 'Dropped' WHERE \"Status\" = 'Archived';");
}
