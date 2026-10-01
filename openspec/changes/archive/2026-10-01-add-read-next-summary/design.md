# Design

## Context

See [proposal.md](proposal.md) for motivation and [specs/series-tracker/spec.md](specs/series-tracker/spec.md) for behavior. The dashboard already loads series with ordered titles, captures one local calendar date per request, derives availability from each title's date, and renders a compact row with one next-title line. The All/To read/etc. filters are applied after loading series.

## Goals / Non-Goals

**Goals:**
- Keep recommendations aligned with the existing date-based availability and reading-status rules.
- Preserve the dashboard's compact list, scrolling region, responsive behavior, and accessible row navigation.

**Non-Goals:**
- Persist recommendation state, score books by preference, change existing row messages or ordering, or suggest books that cannot be read today.

## Decisions

### Derive suggestions from the loaded series

Compute the summary from the already-loaded, unfiltered series and the same captured date used by the dashboard. A series is eligible only when it is not Dropped or Completed and has at least one Available unread title. A finishable series additionally has exactly its planned number of known titles and no remaining unread title that is Upcoming or Unknown. Within a series, select the earliest Available unread title by position. Do not use `GetUnannouncedCount()` alone as proof of completion: remaining known titles must be available too. Prefer deriving this on each view to storing flags, which would go stale as dates advance.

### Rank finishable series first

If finishable series exist, show one highest-ranked next-book suggestion without a count. Order by read/planned proportion descending, then fewer unread planned titles, then most recent update, then stable ID. Keep `Up next` as the heading for every state: use `Wrap up <series> with <title>` for a single remaining book, `Continue <series> with <title>` when several available books remain, and `<title> from <series>` for the highest-ranked fallback when no series is finishable. Compare proportions without relying on rounded display percentages.

### Keep the summary separate from list filtering

Render an unframed, compact plain-text summary above the list only on All. Stack a small `Up next` label above the sentence at every viewport width, with a thin accent rule and stronger book-title emphasis than series-name emphasis. Hide it on filtered views rather than showing a global suggestion that may not appear in the filtered rows. Navigation stays in existing series rows; the summary itself is not interactive. The current list header, row structure, status colors, theme toggle, and scroll container remain unchanged. Preserve a useful empty state when no series is eligible; this includes an empty tracker or one containing only future/undated/Read/Dropped titles.

### Require at least two planned titles on save

Change the shared create form's default and numeric input minimum to two, and enforce the same minimum in server-side draft validation for both create and edit. Do not silently adjust old rows or reset SQLite: an existing one-title series stays visible, but the editor explains the new minimum and requires the user to raise the plan before the next save. The summary can still reflect existing data until corrected. Prefer this to a database constraint or destructive migration because the rule applies to future saves without inventing extra titles for existing series.

## Risks / Trade-offs

- [A series looks one book from completion but its last date is unknown] -> Do not call it finishable or recommend that title until it becomes Available.
- [The current day changes between visits] -> Recompute on each dashboard request using the captured date and cover boundary dates in tests.
- [Many finishable series overwhelm the page] -> Show only the top-ranked suggestion.
- [Summary duplicates row details] -> Keep it brief and action-oriented, with the full title list remaining in the editor.
- [An older series has a plan of one] -> Keep its data intact but reject edits until the planned length becomes at least two.

## Migration Plan

No storage migration is needed. Existing records remain unchanged, while new or edited series must meet the two-title minimum. Rollback restores the old editor rule and dashboard presentation without rewriting stored data.