# Proposal

## Why

The dashboard currently shows a status column and does not clearly signal completed series when progress reaches 100%. The list view is noisier than necessary and does not align the visual treatment with the series completion state.

## What Changes

- Remove the status column from the dashboard list view.
- When a series reaches 100% progress, show the series state as completed and render the row in green.
- Keep the dashboard focused on title, author, publication state, and progress while retaining clear completion cues.

## Capabilities

### New Capabilities

- None

### Modified Capabilities

- `series-tracker`: Dashboard list presentation and completion-state visibility for series whose progress is complete.

## Impact

- Dashboard rendering in the main list page.
- Styling and presentation logic for reading-state rows.
- No external API or persistence changes are required for this behavior update.
