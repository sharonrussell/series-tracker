# Tasks

## 1. Status-row distinction

- [x] 1.1 Add distinct light and dark cool-blue up-to-date row tokens and apply them only to the up-to-date selector; verify reading remains amber and not started, completed, and dropped treatments are unchanged.
- [x] 1.2 Refresh the dashboard and smoke test all visible row states in light and dark themes; verify readable text contrast, hover/focus treatment, and no horizontal overflow.

## 2. Validation

- [x] 2.1 Run `dotnet build --nologo`, `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo`, and `openspec validate differentiate-up-to-date-row-color --strict`.
