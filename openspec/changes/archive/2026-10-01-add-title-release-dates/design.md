# Design

## Context

See [proposal.md](proposal.md) for motivation and
[specs/series-tracker/spec.md](specs/series-tracker/spec.md) for behavior.
`SeriesTitle.State` currently encodes Upcoming, Released, or Read; `SeriesItem`
calculates counts and next-title text from that state. The Razor editor and its
JavaScript summary mirror the enum. The dashboard filter also tests the enum
directly. SQLite is initialized with `EnsureCreated` plus an explicit
legacy-schema check rather than EF migrations.

## Goals / Non-Goals

**Goals:**

- Centralize availability derivation so the dashboard, filters, and persisted
  title validation agree for a given date.
- Keep the existing single-line dashboard layout and series-level Dropped
  override.

**Non-Goals:**

- Backfill or preserve data from existing databases; fetch dates from an
  external catalog; model time zones, release times, or dates of varying
  precision.

## Decisions

### Store a date and a separate read flag

Replace the title state enum with nullable `DateOnly` release date and
`bool IsRead`. Availability is calculated, never stored: null -> Unknown, date
later than the current calendar date -> Upcoming, and date on or before today ->
Available. Read is permitted only when Available. Prefer this to storing a
second availability enum, which could become stale at midnight or disagree with
the date. Invalid combinations are rejected on save rather than silently
changing the user's input.

### Compute from one application-local date per operation

Pass a captured application-local `DateOnly` into shared model calculations for
availability, counts, statuses, dashboard next-title text, ordering, and
filters. Capture it once per page request; use the same date for edit-form
validation. Render that date into the editor for its live summary so client
previews match server rules for the loaded page. Server validation remains
authoritative and the next view recomputes availability after a day changes.
Prefer this to cached status updates or scheduled writes; no background job is
needed. The app is a local tracker; multi-time-zone user preferences are out of
scope.

### Preserve the existing series workflow

Keep title order, editor create/edit flow, and Dropped override. Replace state
radios with a date input and read control; availability labels belong on the
dashboard, not in title editor rows. The dashboard continues to show one
context-sensitive line, prioritizing Available unread in series order, then
Upcoming, then Unknown; keep existing fallbacks. `To read` selects non-dropped
series with Available unread titles; other derived statuses use date-based
released and read counts. Reuse the existing title draft binding, reindexing,
and summary scripts with fields changed to date and read flag. This avoids a
parallel status workflow.

### Initialize incompatible storage on a clean slate

Configure the new nullable date and read flag in EF and update the SQLite
initializer to detect a `SeriesTitles` table with the old `State` shape or
missing new fields, dropping/recreating the incompatible database. New and
already compatible schemas keep `EnsureCreated` behavior. Prefer explicit schema
detection over migrations because the user chose to discard old data and the
project does not use EF migrations; do not drop a compatible database on repeat
startup.

## Risks / Trade-offs

- [Old database contents are lost] -> Treat clean-slate reset as intentional,
  limit it to incompatible schemas, and test both old-schema reset and
  compatible-data retention.
- [Calendar rollover changes counts and filters without writes] -> Derive values
  on each request from a captured date; test yesterday/today/tomorrow boundaries
  with a supplied date.
- [Editor stays open across midnight or client clock differs] -> Refresh summary
  on the next load; reject invalid Read/date combinations on the server while
  retaining entered data and showing a clear error.
- [No date for a title already published] -> It remains Unknown and cannot be
  marked Read until a date is entered, as agreed; no separate publication
  override is added.

## Migration Plan

Deploy the new model and compatibility check together. On first startup, reset
incompatible tracker databases to an empty date-based schema; later starts
retain compatible data. Rollback requires restoring a database backup made
before deployment; there is no in-app migration or automatic rollback for
discarded records.
