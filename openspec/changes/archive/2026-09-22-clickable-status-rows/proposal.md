# Proposal

## Why

The dashboard list has become too dense now that progress is tracked in more detail. Making each row the navigation affordance and using row color for reading status will make the list easier to scan while moving all mutations to the edit screen.

## What Changes

- Remove the dashboard actions column and row action buttons.
- Make each dashboard row clickable and navigable to the series edit screen.
- Remove series length, released so far, and books read columns from the dashboard list.
- Keep progress visible as a fraction instead of a percentage.
- Color dashboard rows by reading status: green for completed, amber for reading/in progress, and red for dropped.
- Move list-screen changes to the edit screen by removing progress, complete, and drop actions from the dashboard list.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Dashboard list interaction and display change to clickable status-colored rows without inline action controls or detailed progress columns.

## Impact

- Dashboard Razor markup and styling need updates for clickable rows and row status colors.
- Edit page remains the place for changing series details, status, released count, and progress.
- Tests or smoke checks should verify the dashboard has no actions column, no list columns for series length/released/books read, and rows link to edit pages.
