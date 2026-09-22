# Spec Delta

## MODIFIED Requirements

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single clickable dashboard list showing title, author, reading status, publication completion state, summary progress, and row color by reading status.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all series in a readable list with their summary progress values and metadata

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

#### Scenario: Hide detailed progress columns
- **WHEN** a user views the dashboard list
- **THEN** the system does not show separate series length, released so far, or books read columns

#### Scenario: Color rows by reading status
- **WHEN** a user views the dashboard list
- **THEN** completed series rows are green, reading series rows are amber, and dropped series rows are red

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
