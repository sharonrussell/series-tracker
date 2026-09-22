# series-tracker Specification

## Purpose
The series tracker lets a user maintain an up-to-date personal list of series they are following, track progress across each title, and move series through active, completed, or archived states without needing a separate tracking tool.

## Requirements

### Requirement: User can add a series to their list
The system SHALL allow a user to add a new series with title, author, publication completion state, and progress directly from the active series list without leaving the dashboard.

#### Scenario: Begin inline creation
- **WHEN** a user clicks the Add button above the series list
- **THEN** the system inserts a new editable row with fields for title, author, series completion state, series length, and books read so far

#### Scenario: Save inline series
- **WHEN** a user enters a title, author, series completion state, series length, and books read so far and saves the row
- **THEN** the system creates the series record and refreshes the dashboard to show it in the active list with its metadata

#### Scenario: Cancel inline creation
- **WHEN** a user cancels the inline add row
- **THEN** the system removes the draft row and leaves the existing series list unchanged

### Requirement: User can maintain series metadata
The system SHALL allow a user to record and update the author and publication completion state for a tracked series.

#### Scenario: Edit series metadata
- **WHEN** a user edits an existing series and changes the author or series completion state
- **THEN** the system saves the updated metadata and shows it with the series on subsequent views

#### Scenario: Select publication completion state
- **WHEN** a user records series metadata
- **THEN** the system allows the user to choose whether the series is ongoing or completed

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single dashboard view showing title, author, reading status, publication completion state, and current progress.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all non-archived series in a readable list with their current progress values and metadata

### Requirement: User can update progress for a series
The system SHALL allow a user to increase the recorded progress for a series without allowing progress to exceed the configured series length, and SHALL mark the series reading status completed when progress reaches the series length.

#### Scenario: Increase progress
- **WHEN** a user records progress for a series after reading more content and the updated value is within the series length
- **THEN** the system updates the series progress and refreshes the dashboard to reflect the new value

#### Scenario: Cap progress at series length
- **WHEN** a user records progress that would make books read exceed the series length
- **THEN** the system saves books read as the series length, marks the reading status completed, and does not exceed 100% completion

#### Scenario: Complete progress exactly
- **WHEN** a user records progress that makes books read equal the series length
- **THEN** the system saves the updated progress and marks the reading status completed

### Requirement: User can change series status
The system SHALL allow a user to set a series status to active, completed, dropped, or archived.

#### Scenario: Mark series complete
- **WHEN** a user marks a series as completed
- **THEN** the system updates the status and shows the series in the appropriate completed or archived view

### Requirement: User can archive a finished or dropped series
The system SHALL allow a user to archive a series when it is no longer active in their reading list while retaining its metadata.

#### Scenario: Archive a series
- **WHEN** a user chooses to archive a series that is finished or no longer being followed
- **THEN** the system removes it from the active dashboard while retaining its title, author, progress, reading status, and publication completion state in archived history for reference

### Requirement: User can filter active reading list by status
The system SHALL allow a user to filter the series list by active, completed, or archived states.

#### Scenario: Filter by status
- **WHEN** a user selects a status filter
- **THEN** the system shows only series matching that status
