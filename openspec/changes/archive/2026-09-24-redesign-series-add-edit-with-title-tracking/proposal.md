# Proposal

## Why

Aggregate progress counts cannot identify which book is next or let users manage known and upcoming titles directly. Tracking ordered titles while retaining the planned series length makes progress easier to update and enables a useful next-in-series summary without cluttering the dashboard.

## What Changes

- Replace aggregate released/read entry with an ordered list of known titles in both Add Series and Edit Series.
- Give each title a single mutually exclusive state: Upcoming, Released, or Read; derive released and read counts from those states.
- Retain planned series length independently from the number of known titles and derive the unannounced count from their difference.
- Prevent known titles from exceeding planned length and make Read imply Released through the title-state model.
- Derive the next-in-series label from title order, prioritizing the first released unread title and then the first known upcoming title.
- Keep the dashboard compact by showing series identity, read-versus-planned progress, and one context-sensitive next/status line without individual title rows or inline actions.
- Use one polished, responsive editor experience for both add and edit, with live summary counts, title ordering, and mobile-friendly controls.
- Reset existing application data when adopting the title-based schema because aggregate progress cannot be mapped reliably to named titles.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Change series creation, editing, progress tracking, derived counts and statuses, dashboard summaries, filtering, and persistence to use ordered title records alongside planned series length.

## Impact

- Domain and persistence: `SeriesItem`, a new child title entity, `SeriesTrackerDbContext`, and startup schema compatibility/reset behavior.
- UI: dashboard, shared add/edit editor, responsive styling, and client-side interactions for title rows, state selection, ordering, and live counts.
- Tests: model derivation, validation, add/edit persistence, ordering, next-title selection, dashboard filtering, and responsive browser smoke coverage.
- Existing local SQLite data is intentionally reset as a breaking data-compatibility change; no EF migration is introduced.
