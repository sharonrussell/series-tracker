# Spec Delta

## MODIFIED Requirements

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single clickable dashboard list showing a consolidated series identity, derived reading status through row color, summary progress, and availability notes, without separate status or series-state columns.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all series in a readable list with a `Series` cell containing the title and author, a summary progress cell, derived-status row colors, and no status or series-state columns

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
- **THEN** the system opens the shared series editor in edit mode for that series

#### Scenario: Series state reflects completion
- **WHEN** a series has reached its configured series length and the publication is complete
- **THEN** the row renders in green and no separate series-state flag is shown in the list

#### Scenario: Hide detailed progress columns
- **WHEN** a user views the dashboard list
- **THEN** the system does not show separate status or series-state columns when row color already communicates reading status

#### Scenario: Color rows by reading status
- **WHEN** a user views the dashboard list
- **THEN** completed series rows are green, reading series rows are amber, not started series rows are muted blue-gray, and dropped series rows are red

#### Scenario: Indicate reading availability
- **WHEN** a series has released books the user has not read yet
- **THEN** the progress cell shows the number of books still to read without changing the column title

#### Scenario: Indicate that the user is up to date
- **WHEN** a series is ongoing and the user has read all currently released books
- **THEN** the progress cell shows `Up to date` without adding future unreleased-book counts

#### Scenario: Identify dropped series in the list
- **WHEN** a series has been dropped
- **THEN** the progress cell shows the numeric progress fraction with `Dropped` displayed beneath it as a secondary note, and the row uses the dropped color

### Requirement: User can update progress for a series
The system SHALL allow a user to increase or reduce recorded progress from the shared editor, SHALL NOT allow books read to exceed the current released count, SHALL NOT allow current released count to exceed the configured series length, and SHALL derive reading status from the resulting progress and publication state unless the series is manually dropped.

#### Scenario: Increase progress
- **WHEN** a user records progress for a series after reading more content and the updated value is within the current released count
- **THEN** the system updates the series progress and refreshes the dashboard to reflect the new derived status

#### Scenario: Cap progress at series length
- **WHEN** a user records progress that would make books read exceed the series length
- **THEN** the system saves books read no higher than the current released count and does not exceed 100% progress against released content

#### Scenario: Complete progress exactly
- **WHEN** a user records progress that makes books read equal the series length and the publication completion state is completed
- **THEN** the system saves the updated progress and derives the reading status as completed

#### Scenario: Keep ongoing publication in reading status
- **WHEN** a user records progress that makes books read equal the current released count and the publication completion state is ongoing
- **THEN** the system saves the updated progress and derives the reading status as up to date without marking it completed

#### Scenario: Cap progress at released count
- **WHEN** a user records progress that would make books read exceed the current released count
- **THEN** the system saves books read as the current released count and does not exceed 100% progress against released content

#### Scenario: Reject released count beyond series length
- **WHEN** a user updates released so far to be greater than series length
- **THEN** the system rejects the update and explains that released count cannot exceed series length

#### Scenario: Reduce progress and recalculate status
- **WHEN** a user reduces books read on a series currently marked completed
- **THEN** the system recalculates the reading status from the new progress instead of leaving the series completed

### Requirement: User can change series status
The system SHALL allow a user to manually set a series to dropped, while reading, not started, up to date, and completed states are derived from progress, released count, series length, and publication completion state.

#### Scenario: Mark series complete
- **WHEN** a user sets reading status to completed
- **THEN** the system does not provide a manual completed override and derives completed only when books read equals series length and the publication state is completed

#### Scenario: Mark series dropped
- **WHEN** a user sets reading status to dropped
- **THEN** the system updates the status to dropped without changing recorded progress

#### Scenario: Restore series to reading
- **WHEN** a user edits a dropped tracked series back to a non-dropped state
- **THEN** the system derives not started, reading, up to date, or completed from the recorded progress and publication state

## ADDED Requirements

### Requirement: User can see derived reading status
The system SHALL derive the non-dropped reading status using this precedence: Dropped when manually overridden; Not started when books read is zero; Completed when books read equals series length and publication is complete; Up to date when all released books are read while publication is ongoing; otherwise Reading.

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
- **WHEN** books read equals series length and the publication completion state is completed
- **THEN** the series status is completed and the row uses the green color
