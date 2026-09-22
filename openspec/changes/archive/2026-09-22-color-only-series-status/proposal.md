# Proposal

## Why

The dashboard currently communicates reading status twice: through row color and a publication-state pill. That duplication adds visual noise, while a dropped series still presents reading progress that is not actionable. The list should be easier to scan by using color as the status cue and making dropped rows explicit where progress would otherwise appear.

## What Changes

- Remove the `Series state` column from the dashboard list.
- Rely on the existing row colors as the dashboard's reading-status indicator.
- Preserve title, author, progress, and clickable-row navigation.
- Show `Dropped` in the progress area for dropped series instead of progress or availability text.
- Keep normal progress and reading-availability indicators for reading series, and retain completed-row styling for completed series.
- Refresh the UI and smoke test the dashboard after implementation.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Simplify dashboard status presentation and make dropped-series display explicit.

## Impact

- Affected dashboard markup and styling in the Razor Pages UI.
- Existing progress/status rendering and focused dashboard regression tests.
- No data model, persistence, route, or API changes are expected.
