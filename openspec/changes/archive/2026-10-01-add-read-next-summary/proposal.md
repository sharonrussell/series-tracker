# Proposal

## Why

The reading list identifies the next book within each series, but the user must
scan and compare rows to find an actionable place to start. A concise summary
can highlight series that can be finished now or suggest one useful next read
without recommending unavailable books.

## What Changes

- Add a compact read-next summary to the dashboard that considers non-dropped,
  incomplete series with available unread titles.
- Give priority to finishable series: every planned title must be known, and
  every remaining unread title must be available. Show only the highest-ranked
  next book as information under a consistent `Up next` heading, with copy that
  distinguishes a final book from other finishable series.
- When no series is finishable, name the next available unread book in the most
  progressed eligible series, comparing read-versus-planned proportions.
- When nothing can be read now, show a clear, non-judgmental empty message. Do
  not suggest future-dated, undated, already read, or dropped titles.
- Require a planned series length of at least two on create and edit; preserve
  existing data while requiring correction before an older one-title series can
  be saved again.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Add an informational dashboard summary of the one best
  available next read and require at least two planned titles per saved series.

## Impact

- Dashboard model, Razor view, shared editor validation and default, restrained
  responsive styling, focused tests, and desktop/mobile UI checks.
- Reuses existing series and title data. No external service, data deletion,
  schema migration, or new dependency is required.
