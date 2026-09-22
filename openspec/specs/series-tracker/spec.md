# series-tracker Specification

## Purpose
The series tracker lets a user maintain an up-to-date personal list of series they are following, track progress across each title, and move series through active, completed, or archived states without needing a separate tracking tool.

## Requirements

### Requirement: User can add a series to their list
The system SHALL allow a user to add a new series directly from the active series list without leaving the dashboard.

#### Scenario: Begin inline creation
- **WHEN** a user clicks the Add button above the series list
- **THEN** the system inserts a new editable row with fields for title, series length, and books read so far

#### Scenario: Save inline series
- **WHEN** a user enters a title, series length, and books read so far and saves the row
- **THEN** the system creates the series record and refreshes the dashboard to show it in the active list

#### Scenario: Cancel inline creation
- **WHEN** a user cancels the inline add row
- **THEN** the system removes the draft row and leaves the existing series list unchanged

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single dashboard view showing title, status, and current progress.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all non-archived series in a readable list with their current progress values

### Requirement: User can update progress for a series
The system SHALL allow a user to increase the recorded progress for a series and save the updated value.

#### Scenario: Increase progress
- **WHEN** a user records progress for a series after reading more content
- **THEN** the system updates the series progress and refreshes the dashboard to reflect the new value

### Requirement: User can change series status
The system SHALL allow a user to set a series status to active, completed, dropped, or archived.

#### Scenario: Mark series complete
- **WHEN** a user marks a series as completed
- **THEN** the system updates the status and shows the series in the appropriate completed or archived view

### Requirement: User can archive a finished or dropped series
The system SHALL allow a user to archive a series when it is no longer active in their reading list.

#### Scenario: Archive a series
- **WHEN** a user chooses to archive a series that is finished or no longer being followed
- **THEN** the system removes it from the active dashboard while retaining it in archived history for reference

### Requirement: User can filter active reading list by status
The system SHALL allow a user to filter the series list by active, completed, or archived states.

#### Scenario: Filter by status
- **WHEN** a user selects a status filter
- **THEN** the system shows only series matching that status
