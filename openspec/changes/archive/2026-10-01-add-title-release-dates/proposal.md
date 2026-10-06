# Proposal

## Why

Manually selecting Upcoming or Released for each title means availability can
become stale on release day. An optional release date can keep availability and
reading progress consistent while still representing titles whose date is
unknown.

## What Changes

- Add an optional release date to every known title and derive Unknown,
  Upcoming, or Available from its date relative to the current local calendar
  date.
- **BREAKING** Replace the Upcoming/Released/Read title-state selector with a
  Read/Unread control. A title can be Read only with a release date on or before
  today; undated and future-dated titles remain Unread.
- Derive released counts, publication and reading statuses, dashboard next-title
  text, and filters from availability and read state; availability updates when
  the calendar date changes without editing the title.
- Assume a clean database slate for this change; do not preserve or migrate
  existing title records or add EF migrations.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Replace manual title publication states with optional
  release dates, date-derived availability, and constrained Read/Unread progress
  across the editor and dashboard.

## Impact

- Title storage and initialization, series-level derivations, create/edit form
  validation and live summary, dashboard filtering and presentation, and focused
  xUnit/UI regression coverage.
- No external API or new dependency is required. Existing title data is not
  retained under the clean-slate assumption.
