# Tasks

## 1. Midnight-blue palette

- [x] 1.1 Replace dark-mode surface, text, border, focus, accent, overlay, note, and status-row tokens with the approved muted midnight-blue palette; verify the light-theme token block is unchanged.
- [x] 1.2 Check dark-mode shell, dashboard, scrollable list, row states, controls, and editor drawer for readable contrast; verify no horizontal overflow at desktop and mobile widths.

## 2. Validation

- [x] 2.1 Verify the theme toggle persists and switches cleanly between unchanged light mode and the new midnight-blue dark mode; verify browser console cleanliness.
- [x] 2.2 Run `dotnet build --nologo`, `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo`, and `openspec validate refresh-dark-theme-palette --strict`.
