# Series Tracker

A personal reading-list tracker for book series. Track each series, its author, released books, reading progress, and whether the series is active, up to date, completed, or dropped.

## Prerequisites

- .NET SDK 10

## Run Locally

```bash
dotnet run
```

The development profile serves the app at `http://localhost:5293`.

## Test

```bash
dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo
```

## Quality Checks

Run the same checks enforced by the application quality workflow before opening a pull request:

```bash
dotnet format --verify-no-changes --no-restore
dotnet build --nologo
dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo
openspec validate --specs --strict
```

## How It Works

- Use the Add button to open the shared series editor.
- Select a series row to edit its details, progress, or dropped state.
- Delete an existing series from the editor after confirming the permanent deletion prompt.
- Filter the list by reading status and use the theme control to switch between light and dark modes.
- Reading status is derived from books read, released count, and series length. Dropped is the only manual status override.

## Persistence

The app uses Entity Framework Core with SQLite. By default, data is stored in `series-tracker.db` through the `DefaultConnection` setting in `appsettings.json`. The application creates the database and applies compatibility updates when it starts.

## Project Structure

- `Pages/`: Razor Pages dashboard, shared layout, and legacy edit redirect
- `Models/`: Series entity and derived progress/status logic
- `Data/`: Entity Framework Core database context
- `wwwroot/`: Custom CSS and browser behavior
- `SeriesTracker.Tests/`: xUnit test suite
- `openspec/`: Change proposals, specifications, and archived change records
