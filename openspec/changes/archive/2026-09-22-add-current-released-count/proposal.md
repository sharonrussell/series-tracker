# Proposal

## Why

For ongoing series, total series length and currently released books are not the same thing. Tracking released count separately prevents the app from treating unread unreleased books as available progress and keeps reading progress grounded in what can actually be read.

## What Changes

- Add a current released count for each series, separate from total series length.
- Enforce `current released count <= series length`.
- Enforce `books read <= current released count` for add, edit, and progress increment flows.
- Display current released count on the dashboard alongside series length and books read.
- Include current released count in add and edit forms.
- Adjust progress percentage to use released count as the denominator for reading progress against available material.

Assumptions:
- Current released count is labeled `Released so far` in the UI.
- For existing data, current released count should default to the larger of books read and series length when publication state is completed, and at least books read for ongoing publications.
- Reading status should auto-complete only when publication state is completed and books read reaches the total series length.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Series progress tracking distinguishes total series length, current released count, and books read.

## Impact

- Series data model and SQLite startup repair need a new persisted released-count field.
- Add, edit, dashboard display, progress increment, and validation logic need updates.
- Tests need coverage for released-count validation, progress capping, migration defaults, and progress percentage behavior.
