# Proposal

## Why

The tracker currently exposes overlapping ways to mark completion: a manual `Complete` button, automatic completion at full progress, an archived view, and an `Archived` value in the reading status list. Simplifying reading status to `Reading`, `Completed`, and `Dropped` and removing archive-specific UI makes the workflow clearer.

## What Changes

- Replace the reading status value `Active` with `Reading`.
- Remove `Archived` from reading status options so the retained reading statuses are `Reading`, `Completed`, and `Dropped`.
- Remove archive behavior and the dedicated archived view.
- Remove the manual `Complete` button from the dashboard row actions.
- Keep automatic completion when progress reaches series length only when the publication completion state is not `Ongoing`.
- Preserve the ability to drop a series.

Assumption: "status is not ongoing" refers to the series publication completion state, not the reading status. A series whose publication state is still `Ongoing` should not be automatically marked reading-completed just because the user has read all currently listed books.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Reading status values, dashboard status actions, archive removal, and automatic completion rules change.

## Impact

- Domain status enum and any persisted status values need migration from `Active` or prior `Ongoing` values to `Reading` and from `Archived` to `Dropped` or another retained status.
- Dashboard, edit page, filter options, navigation, archived view, and archive/restore handlers need status-flow updates or removal.
- Automatic completion logic must account for publication completion state.
- Tests need updates for status values, removed archive UI/workflow, removed complete button, and publication-gated auto-completion.
