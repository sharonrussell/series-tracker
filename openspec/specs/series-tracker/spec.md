# series-tracker Specification

## Purpose
The series tracker lets a user maintain an up-to-date personal list of series they are following, track progress across each title, and move series through reading, completed, or dropped states without needing a separate tracking tool.

## Requirements

### Requirement: User can add a series to their list
The system SHALL allow a user to add a new series with title, author, publication completion state, series length, current released count, and progress directly from the active series list without leaving the dashboard.

#### Scenario: Begin inline creation
- **WHEN** a user clicks the Add button above the series list
- **THEN** the system inserts a new editable row with fields for title, author, series completion state, series length, released so far, and books read so far

#### Scenario: Save inline series
- **WHEN** a user enters a title, author, series completion state, series length, released so far, and books read so far and saves the row
- **THEN** the system creates the series record and refreshes the dashboard to show it in the active list with its metadata and release progress

#### Scenario: Cancel inline creation
- **WHEN** a user cancels the inline add row
- **THEN** the system removes the draft row and leaves the existing series list unchanged

#### Scenario: Reject released count beyond series length
- **WHEN** a user enters released so far greater than the series length
- **THEN** the system rejects the draft and explains that released count cannot exceed series length

#### Scenario: Reject books read beyond released count
- **WHEN** a user enters books read greater than released so far
- **THEN** the system rejects the draft and explains that books read cannot exceed released count

### Requirement: User can maintain series metadata
The system SHALL allow a user to record and update the author and publication completion state for a tracked series.

#### Scenario: Edit series metadata
- **WHEN** a user edits an existing series and changes the author or series completion state
- **THEN** the system saves the updated metadata and shows it with the series on subsequent views

#### Scenario: Select publication completion state
- **WHEN** a user records series metadata
- **THEN** the system allows the user to choose whether the series is ongoing or completed

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single clickable dashboard list showing title, author, summary progress, and row color as the reading-status indicator, without separate status or series-state columns.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all series in a readable list with title, author, summary progress, and row colors, without status or series-state columns

#### Scenario: Dashboard actions omit manual complete
- **WHEN** a user views actions for a series
- **THEN** the system does not show an actions column or inline action buttons on the dashboard list

#### Scenario: Symbol row actions
- **WHEN** a user views row actions for a series
- **THEN** the system does not show row action symbols on the dashboard list

#### Scenario: Symbol actions remain accessible
- **WHEN** a user focuses or hovers over dashboard list navigation
- **THEN** the system provides an accessible edit navigation name for each clickable row

#### Scenario: Navigate to edit from row
- **WHEN** a user activates a dashboard row
- **THEN** the system navigates to that series edit screen

#### Scenario: Series state reflects completion
- **WHEN** a series has reached 100% progress
- **THEN** the row renders in green and no separate series-state flag is shown in the list

#### Scenario: Hide detailed progress columns
- **WHEN** a user views the dashboard list
- **THEN** the system does not show separate status or series-state columns when row color already communicates reading status

#### Scenario: Color rows by reading status
- **WHEN** a user views the dashboard list
- **THEN** completed series rows are green, reading series rows are amber, and dropped series rows are red

#### Scenario: Indicate reading availability
- **WHEN** a series has released books the user has not read yet
- **THEN** the progress cell shows the number of books still to read without changing the column title

#### Scenario: Indicate that the user is up to date
- **WHEN** a series is ongoing and the user has read all currently released books
- **THEN** the progress cell shows `Up to date` without adding future unreleased-book counts

#### Scenario: Identify dropped series in the list
- **WHEN** a series has been dropped
- **THEN** the progress cell shows the numeric progress fraction with `Dropped` displayed beneath it as a secondary note, and the row uses the dropped color

### Requirement: Dashboard UI changes are refreshed and smoke tested
Any dashboard UI change SHALL be verified against a freshly refreshed browser view before it is considered complete.

#### Scenario: Validate a dashboard UI change
- **WHEN** a dashboard UI change is implemented
- **THEN** the developer refreshes the running UI and performs a smoke test that checks the changed behavior is visible and the dashboard remains usable

### Requirement: User can update progress for a series
The system SHALL allow a user to increase or edit recorded progress for a series from the edit screen without allowing books read to exceed the current released count, SHALL NOT allow current released count to exceed the configured series length, and SHALL mark the series reading status completed when progress reaches the series length only if the publication completion state is not ongoing.

#### Scenario: Increase progress
- **WHEN** a user records progress for a series after reading more content and the updated value is within the current released count
- **THEN** the system updates the series progress and refreshes the dashboard to reflect the new value

#### Scenario: Cap progress at series length
- **WHEN** a user records progress that would make books read exceed the series length
- **THEN** the system saves books read no higher than the current released count and does not exceed 100% progress against released content

#### Scenario: Complete progress exactly
- **WHEN** a user records progress that makes books read equal the series length and the publication completion state is completed
- **THEN** the system saves the updated progress and marks the reading status completed

#### Scenario: Keep ongoing publication in reading status
- **WHEN** a user records progress that makes books read equal the current released count and the publication completion state is ongoing
- **THEN** the system saves the updated progress without automatically marking the reading status completed

#### Scenario: Cap progress at released count
- **WHEN** a user records progress that would make books read exceed the current released count
- **THEN** the system saves books read as the current released count and does not exceed 100% progress against released content

#### Scenario: Reject released count beyond series length
- **WHEN** a user updates released so far to be greater than series length
- **THEN** the system rejects the update and explains that released count cannot exceed series length

### Requirement: User can change series status
The system SHALL allow a user to set reading status to reading, completed, or dropped.

#### Scenario: Mark series complete
- **WHEN** a user sets reading status to completed
- **THEN** the system updates the status and sets books read to the series length

#### Scenario: Mark series dropped
- **WHEN** a user sets reading status to dropped
- **THEN** the system updates the status without changing recorded progress

#### Scenario: Restore series to reading
- **WHEN** a user edits a tracked series back to reading
- **THEN** the system updates the reading status to reading

### Requirement: User can archive a finished or dropped series
The system SHALL NOT provide an archive workflow or dedicated archived view for tracked series.

#### Scenario: Archive a series
- **WHEN** a user chooses to archive a series that is finished or no longer being followed
- **THEN** the system does not provide an archive action and the series remains managed through reading, completed, or dropped reading status

#### Scenario: Restore an archived series
- **WHEN** a user uses the tracker after archive behavior is removed
- **THEN** the system does not provide a restore-from-archive action or archived view

### Requirement: User can filter active reading list by status
The system SHALL allow a user to filter the series list by reading, completed, or dropped reading status.

#### Scenario: Filter by status
- **WHEN** a user selects a status filter
- **THEN** the system shows only series matching that reading status

#### Scenario: Status filter excludes archive state
- **WHEN** a user opens the status filter
- **THEN** the filter options include reading, completed, and dropped statuses, and do not include archived
