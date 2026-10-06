# Series Tracker

A personal reading-list tracker for book series. Track each series, its author, released books, reading progress, and whether the series is active, up to date, completed, or dropped.

## Prerequisites

- .NET SDK 10
- Node.js 22 and the OpenSpec CLI for quality checks (`npm install -g @fission-ai/openspec`)

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

Every implemented OpenSpec change, including specification-only and development-guidance changes, must pass all Application Quality checks locally before it is declared complete or archived, as well as before opening a pull request. From the repository root, run the exact CI commands in this order:

```bash
dotnet restore SeriesTracker.Tests/SeriesTracker.Tests.csproj
dotnet build SeriesTracker.Tests/SeriesTracker.Tests.csproj --configuration Release --no-restore --nologo
dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --configuration Release --no-build --nologo
dotnet format --verify-no-changes --no-restore
openspec validate --specs --strict
```

Then validate the active change, replacing `<change-name>` with its name:

```bash
openspec validate <change-name> --strict
```

Record the executed commands, outcomes, tested implementation state, and relevant tool versions in the change's `tasks.md` verification summary. Failed, skipped, unavailable, or unrecorded checks block completion and archive; keep final verification unchecked until all required checks pass. Do not disable formatting or substitute a Debug build or filtered tests to obtain a passing result.

Rerun the complete sequence after subsequent implementation, workflow, or specification edits; evidence-only task annotations do not require another run. When CI checks change, update this local checklist and OpenSpec completion guidance in the same change. These checks supplement change-specific verification, including UI smoke tests.

Planning artifacts being ready does not mean implementation is complete. Before archive, review the local evidence again and rerun checks if the tested state changed; validate resulting main specs after specification synchronization. This is a required project process, not an automatic OpenSpec CLI archive lock. Local success does not establish GitHub success: inspect the next Application Quality run after publication separately.

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
