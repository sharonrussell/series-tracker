# Design

## Context

`OnGetAsync` filters the series, then orders them by the existing dashboard
priority and update time. The priority function keeps available unread series
first, then up-to-date, other active, completed, and dropped series. The same
ordering path serves All and To read. See `proposal.md` for motivation and the
spec delta for the behavior contract.

## Goals / Non-Goals

**Goals:**

- Put active started series before active not-started series inside the same
  existing dashboard priority group.
- Keep the ordering consistent in All and To read.
- Preserve existing priority groups and the update-time fallback.

**Non-Goals:**

- Add a persisted or manually editable started state; started is derived from
  read progress.
- Change the Up next recommendation ranking or any filter membership.
- Move Dropped series out of their existing lowest-priority group.

## Decisions

- Define started as `GetReadCount() > 0`, matching the existing derived
  reading-status model. This avoids introducing state that could disagree with
  title read values.
- Add the started/not-started comparison after the existing dashboard priority
  and before `UpdatedAt`. This changes only ordering among series in the same
  priority group; globally prioritizing started series was rejected because it
  would override availability priority.
- Keep the comparison in the shared dashboard ordering path so All and To read
  behave consistently. Keep Dropped at its current lowest priority and preserve
  its existing ordering.
- Cover the change with dashboard tests for started-first ordering, the
  unchanged availability priority, the update-time fallback, and both All and To
  read views.

## Risks / Trade-offs

- A series becomes started as soon as any title is marked Read, including when
  it is later Up to date. This matches the existing progress-derived definition
  and requires no extra state.
- Within a priority group, started status takes precedence over recent edits;
  update recency remains the fallback.
