# Tasks

## 1. Title Model And Storage

- [x] 1.1 Replace title state with optional `DateOnly` release date and `IsRead`; derive Unknown/Upcoming/Available against a supplied date and verify boundary cases with xUnit tests.
- [x] 1.2 Replace series count, reading/publication status, and next-title derivations with availability/read logic, including Unknown fallback and priority order; verify series-level unit tests at yesterday/today/tomorrow boundaries.
- [x] 1.3 Update EF mapping and SQLite initialization to reset incompatible title-state/aggregate schemas while retaining compatible date-based records; verify reset and repeat-startup persistence tests.

## 2. Editor And Dashboard

- [x] 2.1 Replace title draft state with nullable date and Read/Unread fields in create/edit handlers, reject Read without an available date on the server, and verify save/load plus invalid-draft preservation tests.
- [x] 2.2 Replace editor state radios with date input and read control; update row reindexing and live summary using the page's supplied date and verify add, edit, reorder, remove, and invalid-date interactions in the browser.
- [x] 2.3 Update dashboard rendering, ordering, and filters to use a single captured date for available unread, upcoming, and unknown titles; verify filter and next-title tests plus row behavior in the browser.

## 3. Verification

- [x] 3.1 Run `dotnet build --nologo` and `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo`; verify both succeed with new and existing title scenarios.
- [x] 3.2 Refresh the running app and smoke test desktop/mobile dashboard and editor, light/dark themes, list scrolling, date rollover presentation, keyboard access, and browser console; verify changed behavior appears without overflow or errors.
- [x] 3.3 Run `openspec validate add-title-release-dates --strict` and `openspec validate --specs --strict`; verify the change and existing main specs remain valid.