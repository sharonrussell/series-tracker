# Design

## Context

See `proposal.md` for motivation. The current model uses `SeriesStatus.Active`, `Completed`, `Dropped`, and `Archived`, while publication completion is represented separately by `SeriesCompletionState.Ongoing` or `Completed`. Archive is currently implemented by setting reading status to `Archived` and by exposing a dedicated archived page.

## Goals / Non-Goals

**Goals:**

- Limit reading status values to `Reading`, `Completed`, and `Dropped`.
- Remove archive actions, restore actions, archive-specific navigation, and the dedicated archived view.
- Remove the dashboard `Complete` row action.
- Automatically mark reading status completed only when progress reaches length and publication completion state is completed.
- Migrate existing local status values predictably.

**Non-Goals:**

- Do not remove the ability to set completed status from the edit page.
- Do not change publication completion values.
- Do not add new filtering or sorting dimensions beyond the existing status filter.

## Decisions

- Rename `SeriesStatus.Active` to `Reading` and remove `SeriesStatus.Archived`.
  - Rationale: The user wants retained statuses to be reading, completed, and dropped. `Reading` avoids confusion with the publication completion state value `Ongoing`.
  - Alternative considered: keep `Active` internally and label it as `Reading` in the UI. Rejected because the status filter, persisted value, tests, and specs should use the same vocabulary.

- Remove archive state and archive-specific pages/actions.
  - Rationale: The user clarified that there should not be an archived view. Dropped status is sufficient for series that are no longer being followed, and completed status is sufficient for finished reading.
  - Alternative considered: keep archive as a separate flag while removing it from status. Rejected because it would still require an archived view or hidden archive workflow.

- Gate automatic reading completion on publication completion state.
  - Rationale: If a publication is still ongoing, reading all currently listed books should not imply the user has completed the whole series.
  - Alternative considered: always complete at progress cap. Rejected because this would conflict with the requested condition that completion depends on non-ongoing series state.

- Remove only the dashboard `Complete` button.
  - Rationale: Automatic completion handles the fast-path workflow, while the edit page can still correct status directly when needed.
  - Alternative considered: remove all manual completed status changes. Rejected because the user specifically named the button and the edit page remains useful for correction.

## Risks / Trade-offs

- Existing rows with `Status = 'Archived'` need conversion -> Add a migration/repair path that maps them to a retained reading status, likely `Dropped` unless another status can be inferred.
- Changing persisted enum strings can break existing local data -> Handle `Active` and prior `Ongoing` reading status values to `Reading`, and `Archived` to `Dropped` during startup repair.

## Migration Plan

1. Rename reading status values and repair existing `Active` or prior `Ongoing` records to `Reading`.
2. Repair existing `Archived` records to a retained reading status such as `Dropped`.
3. Remove archive and restore handlers, archive actions, archive-specific navigation, and archived page files.
4. Update dashboard, edit, and filter logic to use only reading, completed, and dropped statuses.
5. Remove the dashboard `Complete` button.
6. Update automatic completion logic to require publication completion state `Completed`.
7. Update tests for migration/defaults, status values, button removal, archive removal, and publication-gated auto-completion.
8. Run `dotnet test --nologo`, `dotnet build --nologo`, and a local smoke test of add, progress, drop, filters, and edit flows.

Rollback is to revert the model/UI changes and restore the previous local database file if the migration repair produces unexpected local data.
