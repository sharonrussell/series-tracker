# Proposal

## Why

Editing progress can currently leave reading status inconsistent, such as a series remaining Completed after books read is reduced. Reading status should be derived from progress and publication availability so the list reflects the real state without requiring manual cleanup.

## What Changes

- Derive reading status from books read, currently released books, and total series length.
- Keep Dropped as the only manually selected reading-status override.
- Add a Not started state when books read is zero.
- Keep Reading for partially read released content.
- Keep Up to date for ongoing series where all currently released books are read.
- Mark a series Completed only when books read reaches series length and the publication is complete.
- Add a distinct muted blue-gray row color for Not started.
- Recalculate the derived state whenever progress, released count, series length, publication state, or manual dropped status changes.
- Refresh the UI and smoke test transitions, editing, filters, and row colors.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Derive reading status from progress and publication data, add Not started state, and restrict manual status override to Dropped.

## Impact

- Affected series status model, add/edit save handlers, dashboard row rendering, filters, styling, and regression tests.
- Existing stored series data will be interpreted through the new derived-state rules without requiring a schema migration.
- No external dependency or API change is expected.
