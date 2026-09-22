# Spec Delta

## MODIFIED Requirements

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single dashboard list showing title, author, publication completion state, summary progress, and row color by reading status, without a separate status column in the list view.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all series in a readable list with their summary progress values and metadata, and the status column is omitted from the list view

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
- **THEN** the series state displays as completed and the row renders in green

#### Scenario: Hide detailed progress columns
- **WHEN** a user views the dashboard list
- **THEN** the system does not show separate status or redundant status metadata columns when the list is already grouped by publication state and progress

#### Scenario: Color rows by reading status
- **WHEN** a user views the dashboard list
- **THEN** completed series rows are green, reading series rows are amber, and dropped series rows are red

#### Scenario: Indicate reading availability
- **WHEN** a series has released books the user has not read yet
- **THEN** the progress cell shows the number of books still to read without changing the column title

#### Scenario: Indicate that the user is up to date
- **WHEN** a series is ongoing and the user has read all currently released books
- **THEN** the progress cell shows `Up to date` without adding future unreleased-book counts

## ADDED Requirements

### Requirement: Dashboard UI changes are refreshed and smoke tested
Any dashboard UI change SHALL be verified against a freshly refreshed browser view before it is considered complete.

#### Scenario: Validate a dashboard UI change
- **WHEN** a dashboard UI change is implemented
- **THEN** the developer refreshes the running UI and performs a smoke test that checks the changed behavior is visible and the dashboard remains usable
