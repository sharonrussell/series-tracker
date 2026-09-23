# Tasks

## 1. Dashboard viewport use

- [x] 1.1 Adjust dashboard lower spacing and the bounded scrollable list height so the list uses more available viewport space; verify the Filter and Add controls remain outside the scrolling region.
- [x] 1.2 Preserve responsive minimum list height, footer spacing, and horizontal-overflow protection; verify desktop and mobile layouts remain readable.

## 2. Validation

- [x] 2.1 Refresh the running UI and smoke test list height, list scrolling, persistent controls, footer framing, theme toggle, and browser-console cleanliness at desktop and mobile sizes.
- [x] 2.2 Run `dotnet build --nologo`, `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo`, and `openspec validate reduce-reading-list-bottom-buffer --strict`.
