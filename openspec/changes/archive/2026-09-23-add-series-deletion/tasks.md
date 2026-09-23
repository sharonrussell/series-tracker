# Tasks

## 1. Delete workflow

- [x] 1.1 Add an edit-only `Delete series` action with explicit browser confirmation; verify create mode does not expose deletion and cancelling confirmation preserves the editor and record.
- [x] 1.2 Add an antiforgery-protected delete POST handler that removes only the requested existing series, redirects to the active filter, and returns not found for a missing ID; verify focused page-model tests cover successful and missing-record deletion.

## 2. Validation

- [x] 2.1 Refresh the running UI and smoke test delete-action visibility, confirmation cancellation, successful deletion, filter preservation, responsive editor layout, and browser-console cleanliness.
- [x] 2.2 Run `dotnet build --nologo`, `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo`, and `openspec validate add-series-deletion --strict`.
