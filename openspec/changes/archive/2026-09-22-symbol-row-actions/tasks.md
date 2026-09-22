# Tasks

## 1. Dashboard action symbols

- [x] 1.1 Replace the dashboard row edit text action with a pencil symbol and verify the edit link still routes to the edit page
- [x] 1.2 Replace the dashboard row drop text action with a bin symbol and verify the drop form still posts to the update handler
- [x] 1.3 Keep the progress action compact and verify the `+1` form still posts the increment value
- [x] 1.4 Add a tick symbol action for explicit completion and verify it still posts completed reading status

## 2. Accessibility and styling

- [x] 2.1 Add accessible labels and hover titles for edit, progress, complete, and drop symbol controls and verify the rendered HTML exposes clear action names
- [x] 2.2 Add or update compact action-button styling and verify row action buttons remain aligned and consistently sized

## 3. Regression validation

- [x] 3.1 Run `dotnet test --nologo` and verify all tests pass
- [x] 3.2 Run `dotnet build --nologo` and verify it completes with no warnings
- [x] 3.3 Smoke test the dashboard and verify `Edit` and `Drop` are no longer visible row-action text while pencil and bin symbols are visible
