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
- **THEN** reading series rows are amber, up-to-date series rows are cool blue, not started series rows are muted blue-gray, completed series rows are green, and dropped series rows are red

#### Scenario: Indicate reading availability
- **WHEN** a series has released books the user has not read yet
- **THEN** the progress cell shows the number of books still to read without changing the column title

#### Scenario: Indicate that the user is up to date
- **WHEN** a series is ongoing and the user has read all currently released books
- **THEN** the progress cell shows `Up to date` without adding future unreleased-book counts

#### Scenario: Identify dropped series in the list
- **WHEN** a series has been dropped
- **THEN** the progress cell shows the numeric progress fraction with `Dropped` displayed beneath it as a secondary note, and the row uses the dropped color
