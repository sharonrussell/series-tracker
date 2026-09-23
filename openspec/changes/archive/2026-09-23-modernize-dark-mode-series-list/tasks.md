# Tasks

## 1. Theme system

- [x] 1.1 Replace repeated literal UI colors with semantic light/dark theme tokens covering shell, surfaces, text, controls, borders, overlays, editor drawer, and status rows; verify the app builds and both themes have readable contrast.
- [x] 1.2 Add an accessible sun/moon theme toggle in the application shell with system-preference fallback and local persistence; verify first-load preference, toggle state, reload persistence, and keyboard focus behavior.

## 2. Dashboard modernization

- [x] 2.1 Consolidate dashboard title and author into a single `Series` cell with prominent title and secondary author text while preserving accessible row navigation and progress behavior; verify rendered headers and row content.
- [x] 2.2 Refresh shell, dashboard, controls, table, progress notes, row colors, and editor drawer styling to use the modern semantic visual system in light and dark themes; verify no surface remains unreadable in either theme.

## 3. Validation

- [x] 3.1 Add or update regression coverage for theme preference behavior and consolidated series identity markup; verify `dotnet test` and `dotnet build` pass.
- [x] 3.2 Refresh the running UI and smoke test light mode, dark mode, system preference fallback, persisted toggle state, consolidated list rows, editor drawer, desktop, and mobile layouts; verify the browser console is clean and controls remain accessible.
