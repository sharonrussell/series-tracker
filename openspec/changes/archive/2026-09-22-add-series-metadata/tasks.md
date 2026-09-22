# Tasks

## 1. Domain and persistence

- [x] 1.1 Add author and publication completion state to the series domain model and verify defaults with unit tests
- [x] 1.2 Update EF Core mapping/database initialization so existing and new SQLite databases include the metadata fields, then verify the app starts against a local database
- [x] 1.3 Add validation for required author and valid publication completion state, then verify invalid inline-add submissions keep the draft row visible with a message

## 2. Create and edit flows

- [x] 2.1 Add author and publication completion state inputs to the dashboard inline-add row and verify saving creates a series with those values
- [x] 2.2 Add author and publication completion state controls to the edit page and verify saved edits appear when returning to the dashboard
- [x] 2.3 Preserve existing title, progress, reading status, notes, completion, drop, and archive behavior while adding metadata, then verify the existing tests still pass

## 3. List and archived display

- [x] 3.1 Show author and publication completion state on the active dashboard list and verify the list remains readable on desktop and narrow widths
- [x] 3.2 Show author and publication completion state on the archived view and verify archived records retain the metadata after archive

## 4. Regression validation

- [x] 4.1 Add or update tests for metadata defaults, validation, creation, and edit mapping, then verify `dotnet test --nologo` succeeds
- [x] 4.2 Run `dotnet build --nologo` and verify it completes with no warnings
- [x] 4.3 Run the app locally and smoke test add, edit, progress increment, complete, drop, archive, filter, and archived view flows
