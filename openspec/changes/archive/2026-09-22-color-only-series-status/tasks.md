# Tasks

## 1. Dashboard presentation

- [x] 1.1 Remove the series-state column and pill from the dashboard list while preserving title, author, progress, row colors, and accessible row navigation; verify the rendered headers contain no series-state column.
- [x] 1.2 Render `Dropped` in the progress cell for dropped series and retain progress, `to read`, and `Up to date` indicators for other statuses; verify each display branch with focused tests.

## 2. Validation

- [x] 2.1 Update or add dashboard regression coverage for color-only status presentation and dropped-row output; verify `dotnet test` passes.
- [x] 2.2 Refresh the running UI and smoke test reading, completed, up-to-date, and dropped rows; verify the dashboard remains clickable, accessible, and free of console errors.
