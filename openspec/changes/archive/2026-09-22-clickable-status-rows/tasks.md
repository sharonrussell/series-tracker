# Tasks

## 1. Dashboard row navigation

- [x] 1.1 Remove the dashboard actions column and verify no row action buttons render
- [x] 1.2 Make each dashboard row navigate to the existing edit page and verify the rendered row exposes an accessible edit navigation name
- [x] 1.3 Verify dashboard row navigation works without changing edit page routes or handlers

## 2. Dashboard column simplification

- [x] 2.1 Remove series length, released so far, and books read columns from the dashboard list and verify those headers no longer render
- [x] 2.2 Keep title, author, reading status, publication state, and summary progress visible and verify the table shows progress as a fraction

## 3. Status row styling

- [x] 3.1 Add row-level status classes for completed, reading, and dropped and verify each status renders a distinct class
- [x] 3.2 Style completed rows green, reading rows amber, and dropped rows red and verify text remains readable

## 4. Regression validation

- [x] 4.1 Run `dotnet test --nologo` and verify all tests pass
- [x] 4.2 Run `dotnet build --nologo` and verify it completes with no warnings
- [x] 4.3 Smoke test the dashboard HTML and verify there is no actions column, no inline action controls, no detailed progress columns, and each row links to edit
