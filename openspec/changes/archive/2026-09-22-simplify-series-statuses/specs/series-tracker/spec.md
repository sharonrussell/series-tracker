# Spec Delta

## MODIFIED Requirements

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single dashboard view showing title, author, reading status, publication completion state, and current progress.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all series in a readable list with their current progress values and metadata

#### Scenario: Dashboard actions omit manual complete
- **WHEN** a user views actions for a series
- **THEN** the system offers progress, edit, and drop actions without manual complete or archive actions

### Requirement: User can update progress for a series
The system SHALL allow a user to increase the recorded progress for a series without allowing progress to exceed the configured series length, and SHALL mark the series reading status completed when progress reaches the series length only if the publication completion state is not ongoing.

#### Scenario: Increase progress
- **WHEN** a user records progress for a series after reading more content and the updated value is within the series length
- **THEN** the system updates the series progress and refreshes the dashboard to reflect the new value

#### Scenario: Cap progress at series length
- **WHEN** a user records progress that would make books read exceed the series length
- **THEN** the system saves books read as the series length and does not exceed 100% completion

#### Scenario: Complete progress exactly
- **WHEN** a user records progress that makes books read equal the series length and the publication completion state is completed
- **THEN** the system saves the updated progress and marks the reading status completed

#### Scenario: Keep ongoing publication in reading status
- **WHEN** a user records progress that makes books read equal the series length and the publication completion state is ongoing
- **THEN** the system saves the updated progress without automatically marking the reading status completed

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
