# Tasks

## 1. Visual system polish

- [x] 1.1 Refine light and dark theme tokens for softer backgrounds, accents, borders, shadows, and row-state colors; verify both themes remain readable in browser smoke checks.
- [x] 1.2 Improve typography, page spacing, control sizing, card surface, row rhythm, and editor drawer presentation; verify desktop and mobile layouts remain aligned and free of horizontal overflow.
- [x] 1.3 Make the series rows scroll within a constrained list region while the filter and Add controls stay visible; verify scrolling does not hide or disable those controls.

## 2. Reading-list personality

- [x] 2.1 Add one restrained reading-themed whimsical accent that does not affect semantics or reduce readability; verify it appears appropriately in light and dark modes.
- [x] 2.2 Preserve existing dashboard, filter, row navigation, editor, progress, theme toggle, and derived-status behavior; verify no functional regressions in smoke testing.

## 3. Validation

- [x] 3.1 Run `dotnet test` and `dotnet build` to verify the polish pass does not break the app.
- [x] 3.2 Refresh the running UI and smoke test light mode, dark mode, desktop, mobile, row states, editor drawer, theme toggle, and browser console cleanliness.
