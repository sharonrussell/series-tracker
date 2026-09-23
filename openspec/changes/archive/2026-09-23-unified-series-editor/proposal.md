# Proposal

## Why

Adding a series inline inside the dashboard and editing one on a separate generic page create two inconsistent workflows for the same task. The add form is cramped inside the table, while the edit form loses dashboard context and does not present the fields as a coherent editing experience.

## What Changes

- Replace the inline add row and separate edit-page interaction with one shared series editor drawer over the dashboard.
- Open the drawer in create mode from Add and in edit mode when a user selects a series row.
- Keep the dashboard visible behind the drawer and use a full-screen sheet treatment on small screens.
- Group fields into series details, progress, and tracking sections with consistent validation and save/cancel actions.
- Default new series to Reading and do not expose reading-status selection while creating.
- Expose reading status, including Dropped, when editing an existing series.
- Preserve direct edit URLs by resolving them to the shared editor in edit mode.
- Refresh the UI and smoke test both create and edit flows.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Unify add and edit into a shared responsive dashboard editor with distinct create and edit field visibility.

## Impact

- Affected dashboard markup, edit-page routing, client-side drawer behavior, and shared form styling.
- Existing add/edit handlers and validation will be consolidated or adapted without changing stored series data.
- Focused regression tests and browser smoke coverage will be updated.
- No new external dependency or persistence migration is expected.
