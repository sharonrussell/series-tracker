# Design

## Context

The dashboard currently renders a list with a status column and treats progress as a plain percentage. The existing data model already exposes completion-state and status information, so this change can be handled entirely in the presentation layer without changing underlying persistence or page handlers.

## Goals / Non-Goals

**Goals:**
- Remove the redundant status column from the dashboard list.
- Use the existing status and completion-state data to show clearer visual completion cues.
- Keep the dashboard readable and consistent when a series reaches 100% progress.

**Non-Goals:**
- No data model or persistence migration.
- No new API surface or workflow for archive behavior.
- No change to edit-page logic or route structure.

## Decisions

- The list view will render only the columns necessary for at-a-glance tracking: title, author, publication state, and progress.
- Completion status will be inferred from the existing series completion state and progress threshold logic rather than adding a new source of truth.
- Row styling will use the current status classes and a completed-row override when progress reaches 100% to keep styles consistent with the rest of the interface.

## Risks / Trade-offs

- [Risk] A row-colored completion state may reduce the contrast of some status badges.
  → Mitigation: keep the green completed row in a high-contrast palette and maintain readable text colors.
- [Risk] Removing the status column may hide context for readers scanning the list quickly.
  → Mitigation: completion and reading status remain visible through the row color and the existing publication state value.

## Migration Plan

- Update the dashboard markup to remove the status column.
- Update the status/class mapping used for row styling.
- Refresh the running UI and validate the list in browser smoke testing to confirm the completed row appears green, the status column is absent, and the dashboard remains usable.

## Open Questions

- None. The required behavior is fully specified by the change request and existing status model.
