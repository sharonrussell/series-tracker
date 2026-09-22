# Proposal

## Why

The tracker currently identifies series by title and reading progress only, which makes it harder to distinguish works with similar names or decide how to interpret progress for a series that is still being published. Adding author and publication completion state gives the list enough context to manage active reading without changing the user's reading status workflow.

## What Changes

- Add an author value to each tracked series so the dashboard and edit flows can show who wrote the series.
- Add a series completion state with `Ongoing` and `Completed` values to describe whether the series itself is still being published.
- Capture author and series completion state when creating a new series from the dashboard inline row.
- Allow author and series completion state to be updated from the edit page.
- Display author and series completion state in the dashboard and archived views.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Series entries include author metadata and a publication completion state in create, edit, dashboard, and archived-list workflows.

## Impact

- Data model and SQLite schema need new persisted fields for author and series completion state.
- Dashboard inline-add form, list table, edit page, and archived view need UI updates.
- Validation should ensure author is captured consistently with title while keeping notes optional.
- Tests should cover metadata defaults, validation, and display/update flows where practical.
