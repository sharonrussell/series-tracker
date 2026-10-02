# Tasks

## 1. Dashboard Ordering

- [x] 1.1 Rank non-dropped started series before not-started series within the same existing dashboard priority group; verify the behavior in All and To read views.
- [x] 1.2 Verify availability-group priority, update-time fallback within started/not-started classes, and existing Dropped ordering with focused dashboard tests.

## 2. Verification

- [x] 2.1 Run `dotnet build --nologo` and `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo`; verify both pass.
- [x] 2.2 Smoke-test dashboard ordering in All and To read, then run `openspec validate prioritize-started-series-in-list --strict` and `openspec validate --specs --strict`.