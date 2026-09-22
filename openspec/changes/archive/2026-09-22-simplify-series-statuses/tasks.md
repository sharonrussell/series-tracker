# Tasks

## 1. Domain and persistence

- [x] 1.1 Replace reading status `Active` with `Reading` and remove `Archived`, then verify status defaults and enum tests pass
- [x] 1.2 Add startup repair for existing `Active`, prior `Ongoing`, and `Archived` persisted values and verify local database startup succeeds

## 2. Status and archive behavior

- [x] 2.1 Update dashboard, edit, and filter options to show only reading, completed, and dropped reading statuses and verify archived is absent
- [x] 2.2 Remove the dashboard `Complete` button and verify row actions still include edit, progress, and drop
- [x] 2.3 Remove archive actions, restore handlers, archive navigation, and archived page files, then verify archived routes/actions are no longer available
- [x] 2.4 Update dashboard queries to show series by retained reading status and verify no archive state is required

## 3. Progress completion rules

- [x] 3.1 Update add, edit, and increment flows so progress equal to length marks reading status completed only when publication completion state is completed
- [x] 3.2 Verify progress equal to length for an ongoing publication does not automatically mark reading status completed
- [x] 3.3 Verify explicit completed reading status still sets books read to the series length without changing publication completion state

## 4. Regression validation

- [x] 4.1 Update tests for status values, removed dashboard complete action, archive removal, and publication-gated auto-completion, then verify `dotnet test --nologo` succeeds
- [x] 4.2 Run `dotnet build --nologo` and verify it completes with no warnings
- [x] 4.3 Run the app locally and smoke test add, progress, drop, filters, and edit flows, and verify no archived view is linked
