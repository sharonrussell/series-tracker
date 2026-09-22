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
The system SHALL present each tracked series in a single dashboard view showing title, author, reading status, publication completion state, series length, current released count, current progress, and compact symbol-based row actions.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all series in a readable list with their current progress values, released count, and metadata

#### Scenario: Dashboard actions omit manual complete
- **WHEN** a user views actions for a series
- **THEN** the system offers progress, edit, complete, and drop actions without archive actions

#### Scenario: Symbol row actions
- **WHEN** a user views row actions for a series
- **THEN** the edit action is shown as a pencil symbol, the complete action is shown as a tick symbol, and the drop action is shown as a bin symbol

#### Scenario: Symbol actions remain accessible
- **WHEN** a user focuses or hovers over a symbol action
- **THEN** the system provides an accessible action name for edit, progress, complete, and drop actions

### Requirement: User can update progress for a series
The system SHALL allow a user to increase the recorded progress for a series without allowing books read to exceed the current released count, SHALL NOT allow current released count to exceed the configured series length, and SHALL mark the series reading status completed when progress reaches the series length only if the publication completion state is not ongoing.

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
