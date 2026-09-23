# Proposal

## Why

Dropped series currently replace their numeric progress with the word `Dropped`, which removes useful information about how far the reader got. The dashboard already uses secondary progress notes for reading availability, so dropped status should use the same compact pattern while preserving the progress fraction.

## What Changes

- Keep the numeric progress fraction visible for dropped series.
- Show `Dropped` beneath the fraction as a secondary progress note.
- Preserve the existing color-only status presentation, row navigation, and other progress notes.
- Refresh the UI and smoke test the dashboard after implementation.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Change dropped-series dashboard output to show progress with a secondary `Dropped` note.

## Impact

- Affected dashboard progress-cell rendering and its focused regression tests.
- No data model, persistence, route, or API changes are expected.
