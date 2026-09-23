# Proposal

## Why

Rows for active reading and up-to-date series currently use the same amber color, even though they represent different states: actionable books are available versus the user is waiting for a future release. A distinct up-to-date color will make the list easier to scan.

## What Changes

- Give `UpToDate` rows a distinct cool-blue treatment in light and dark themes.
- Retain amber for `Reading` rows with currently available books.
- Preserve separate blue-gray `NotStarted`, green `Completed`, and berry `Dropped` treatments.
- Preserve all progress, availability, filtering, and derived-status behavior.
- Smoke test the status-row palette in light and dark modes.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Distinguish waiting/up-to-date rows from actionable reading rows through status-row color.

## Impact

- Affected status-row theme tokens and the `UpToDate` CSS treatment in `wwwroot/css/site.css`.
- No data model, derived-status, persistence, filtering, or interaction changes.
