# Tasks

## 1. Domain and persistence

- [x] 1.1 Add `CurrentReleasedCount` to the series domain model and verify default behavior with unit tests
- [x] 1.2 Update EF mapping/startup repair so existing and new SQLite databases include `CurrentReleasedCount`, then verify app startup succeeds
- [x] 1.3 Initialize existing records with a valid released count and verify released count is never below books read or above series length

## 2. Validation and progress rules

- [x] 2.1 Update add/edit validation to reject released count greater than series length and verify validation messages
- [x] 2.2 Update add/edit validation to reject books read greater than released count and verify validation messages
- [x] 2.3 Update dashboard `+1` progress logic to cap books read at released count and verify handler tests
- [x] 2.4 Keep reading status completion based on total series length and completed publication state, then verify reading all released books in an ongoing publication does not mark completed

## 3. UI updates

- [x] 3.1 Add `Released so far` to the inline add row and verify saving persists the value
- [x] 3.2 Add `Released so far` to the edit page and verify edits persist the value
- [x] 3.3 Show released count on the dashboard and verify the table distinguishes series length, released so far, and books read
- [x] 3.4 Update dashboard progress percentage to use released count as the denominator and verify displayed values do not exceed 100%

## 4. Regression validation

- [x] 4.1 Update tests for released-count validation, progress percentage, add/edit mapping, and increment capping, then verify `dotnet test --nologo` succeeds
- [x] 4.2 Run `dotnet build --nologo` and verify it completes with no warnings
- [x] 4.3 Run the app locally and smoke test add, edit, progress increment, completion behavior, and dashboard display for released count
