# Tasks

## 1. Project setup

- [x] 1.1 Scaffold the ASP.NET Core app with Razor Pages and EF Core dependencies and verify the project builds successfully
- [x] 1.2 Configure SQLite persistence and verify the local database initializes without runtime errors

## 2. Data model and domain logic

- [x] 2.1 Create the Series entity with fields for title, status, current progress, total progress, notes, and timestamps, and verify the database schema is created correctly
- [x] 2.2 Add status transitions for active, completed, dropped, and archived with validation checks and verify state updates behave as expected

## 3. User experience and CRUD flows

- [x] 3.1 Build the dashboard listing all active series with progress and status information and verify the page renders the expected records
- [x] 3.2 Implement add and edit series forms and verify new entries can be created and updated successfully
- [x] 3.3 Implement progress update actions and verify the value changes are saved and reflected in the dashboard
- [x] 3.4 Implement archive and status filter behavior and verify archived items are hidden from the active dashboard while remaining accessible in archived views

## 4. Validation and polish

- [x] 4.1 Run the application and verify the core workflow works end-to-end for adding, updating, and archiving series
- [x] 4.2 Review the UI for clarity and simplicity and verify the dashboard remains easy to scan for a personal reading list
