# Tasks

## 1. Dashboard UX update

- [x] 1.1 Add an Add button above the active series list and verify it reveals a new editable row without leaving the dashboard
- [x] 1.2 Add inline fields for title, series length, and books read so far and verify the draft row is clearly scoped to the new entry
- [x] 1.3 Implement Save and Cancel actions for the inline row and verify the row disappears or persists correctly based on the action taken

## 2. Data and validation flow

- [x] 2.1 Bind the inline row to the existing series model and verify required values are validated before a record is created
- [x] 2.2 Save the new series and verify the dashboard refreshes to show it in the active list with correct progress values

## 3. Regression validation

- [x] 3.1 Run the application and verify the dashboard still renders, filters, edits, and archives existing series correctly
- [x] 3.2 Verify the new inline add flow works without breaking the current progress update and archive patterns
