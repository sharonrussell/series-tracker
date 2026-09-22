# Spec Delta

## ADDED Requirements

### Requirement: User can maintain series metadata
The system SHALL allow a user to record and update the author and publication completion state for a tracked series.

#### Scenario: Edit series metadata
- **WHEN** a user edits an existing series and changes the author or series completion state
- **THEN** the system saves the updated metadata and shows it with the series on subsequent views

#### Scenario: Select publication completion state
- **WHEN** a user records series metadata
- **THEN** the system allows the user to choose whether the series is ongoing or completed

## MODIFIED Requirements

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

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single dashboard view showing title, author, reading status, publication completion state, and current progress.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all non-archived series in a readable list with their current progress values and metadata

### Requirement: User can archive a finished or dropped series
The system SHALL allow a user to archive a series when it is no longer active in their reading list while retaining its metadata.

#### Scenario: Archive a series
- **WHEN** a user chooses to archive a series that is finished or no longer being followed
- **THEN** the system removes it from the active dashboard while retaining its title, author, progress, reading status, and publication completion state in archived history for reference
