# Spec Delta

## MODIFIED Requirements

### Requirement: User can add a series to their list
The system SHALL allow a user to add a new series from a shared dashboard editor opened in create mode, with title, author, series length, current released count, and progress, without manually selecting publication status and without leaving the dashboard.

#### Scenario: Begin inline creation
- **WHEN** a user clicks the Add button above the series list
- **THEN** the system opens the shared series editor in create mode without navigating away from the dashboard

#### Scenario: Save inline series
- **WHEN** a user enters a title, author, series length, released so far, and books read so far and saves the row
- **THEN** the system creates the series record, derives publication status from released count and series length, closes the editor, and refreshes the dashboard to show it in the active list with its metadata and release progress

#### Scenario: New series defaults to reading
- **WHEN** a user opens the shared editor in create mode
- **THEN** the reading status defaults to Reading and the reading-status selector is not shown

#### Scenario: Cancel inline creation
- **WHEN** a user cancels the inline add row
- **THEN** the system closes the shared editor and leaves the existing series list unchanged

#### Scenario: Reject released count beyond series length
- **WHEN** a user enters released so far greater than the series length
- **THEN** the system rejects the draft and explains that released count cannot exceed series length

#### Scenario: Reject books read beyond released count
- **WHEN** a user enters books read greater than released so far
- **THEN** the system rejects the draft and explains that books read cannot exceed released count

### Requirement: User can maintain series metadata
The system SHALL allow a user to record and update the author and derive publication status from released count and configured series length for a tracked series.

#### Scenario: Edit series metadata
- **WHEN** a user edits an existing series and changes the author, series length, or released count
- **THEN** the system saves the updated metadata and shows it with the derived publication status on subsequent views

#### Scenario: Select publication completion state
- **WHEN** a user records series metadata
- **THEN** the system does not show a manual publication-state selector and instead derives whether the series is ongoing or fully released from released count compared with series length

### Requirement: User can update progress for a series
The system SHALL allow a user to increase or reduce recorded progress from the shared editor, SHALL NOT allow books read to exceed the current released count, SHALL NOT allow current released count to exceed the configured series length, SHALL derive publication status from released count and series length, and SHALL derive reading status from the resulting progress unless the series is manually dropped.

#### Scenario: Increase progress
- **WHEN** a user records progress for a series after reading more content and the updated value is within the current released count
- **THEN** the system updates the series progress and refreshes the dashboard to reflect the new derived status

#### Scenario: Cap progress at series length
- **WHEN** a user records progress that would make books read exceed the series length
- **THEN** the system saves books read no higher than the current released count and does not exceed 100% progress against released content

#### Scenario: Complete progress exactly
- **WHEN** a user records progress that makes books read equal the series length
- **THEN** the system saves the updated progress, sets released count to the series length when necessary, derives publication status as fully released, and derives reading status as completed

#### Scenario: Keep ongoing publication in reading status
- **WHEN** a user records progress that makes books read equal the current released count and the current released count is less than series length
- **THEN** the system saves the updated progress, derives publication status as ongoing, and derives reading status as up to date without marking it completed

#### Scenario: Cap progress at released count
- **WHEN** a user records progress that would make books read exceed the current released count
- **THEN** the system saves books read as the current released count and does not exceed 100% progress against released content

#### Scenario: Reject released count beyond series length
- **WHEN** a user updates released so far to be greater than series length
- **THEN** the system rejects the update and explains that released count cannot exceed series length

#### Scenario: Reduce progress and recalculate status
- **WHEN** a user reduces books read on a series currently marked completed
- **THEN** the system recalculates the reading status from the new progress instead of leaving the series completed

### Requirement: User can see derived reading status
The system SHALL derive the non-dropped reading status using this precedence: Dropped when manually overridden; Not started when books read is zero; Completed when books read equals series length and publication is fully released; Up to date when all released books are read while publication is ongoing; otherwise Reading.

#### Scenario: Derive not started
- **WHEN** books read is zero and the series is not dropped
- **THEN** the series status is not started and the row uses the muted blue-gray color

#### Scenario: Derive reading
- **WHEN** books read is greater than zero, unread released books remain, and the series is not dropped or completed
- **THEN** the series status is reading and the row uses the amber color

#### Scenario: Derive up to date
- **WHEN** books read equals the current released count, the current released count is less than series length, and the series is not dropped
- **THEN** the series status is up to date

#### Scenario: Derive completed
- **WHEN** books read equals series length and released count equals series length
- **THEN** the publication status is fully released, the series status is completed, and the row uses the green color

## ADDED Requirements

### Requirement: User can see derived publication status
The system SHALL derive publication status from released count compared with series length instead of requiring manual publication-state selection.

#### Scenario: Derive ongoing publication
- **WHEN** released so far is less than series length
- **THEN** the publication status is ongoing

#### Scenario: Derive fully released publication
- **WHEN** released so far equals series length
- **THEN** the publication status is fully released

#### Scenario: Full progress implies fully released publication
- **WHEN** books read is set to the configured series length
- **THEN** the system treats released so far as the series length and derives publication status as fully released
