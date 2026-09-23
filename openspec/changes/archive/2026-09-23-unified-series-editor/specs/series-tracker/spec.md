# Spec Delta

## MODIFIED Requirements

### Requirement: User can add a series to their list
The system SHALL allow a user to add a new series from a shared dashboard editor opened in create mode, with title, author, publication completion state, series length, current released count, and progress, without leaving the dashboard.

#### Scenario: Begin inline creation
- **WHEN** a user clicks the Add button above the series list
- **THEN** the system opens the shared series editor in create mode without navigating away from the dashboard

#### Scenario: Save inline series
- **WHEN** a user enters a title, author, series completion state, series length, released so far, and books read so far and saves the editor
- **THEN** the system creates the series record, closes the editor, and refreshes the dashboard to show it in the active list with its metadata and release progress

#### Scenario: New series defaults to reading
- **WHEN** a user opens the shared editor in create mode
- **THEN** the reading status defaults to Reading and the reading-status selector is not shown

#### Scenario: Cancel inline creation
- **WHEN** a user cancels the shared editor in create mode
- **THEN** the system closes the editor and leaves the existing series list unchanged

#### Scenario: Reject released count beyond series length
- **WHEN** a user enters released so far greater than the series length
- **THEN** the system rejects the draft and explains that released count cannot exceed series length

#### Scenario: Reject books read beyond released count
- **WHEN** a user enters books read greater than released so far
- **THEN** the system rejects the draft and explains that books read cannot exceed released count

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single clickable dashboard list showing title, author, summary progress, and row color as the reading-status indicator, without separate status or series-state columns.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all series in a readable list with title, author, summary progress, and row colors, without status or series-state columns

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
- **WHEN** a series has reached 100% progress
- **THEN** the row renders in green and no separate series-state flag is shown in the list

#### Scenario: Hide detailed progress columns
- **WHEN** a user views the dashboard list
- **THEN** the system does not show separate status or series-state columns when row color already communicates reading status

#### Scenario: Color rows by reading status
- **WHEN** a user views the dashboard list
- **THEN** completed series rows are green, reading series rows are amber, and dropped series rows are red

#### Scenario: Indicate reading availability
- **WHEN** a series has released books the user has not read yet
- **THEN** the progress cell shows the number of books still to read without changing the column title

#### Scenario: Indicate that the user is up to date
- **WHEN** a series is ongoing and the user has read all currently released books
- **THEN** the progress cell shows `Up to date` without adding future unreleased-book counts

#### Scenario: Identify dropped series in the list
- **WHEN** a series has been dropped
- **THEN** the progress cell shows the numeric progress fraction with `Dropped` displayed beneath it as a secondary note, and the row uses the dropped color

## ADDED Requirements

### Requirement: Shared series editor provides consistent add and edit flow
The system SHALL provide one responsive editor surface over the dashboard for creating and editing series, with consistent field grouping, validation, and save/cancel actions.

#### Scenario: Edit existing series
- **WHEN** a user activates a series row
- **THEN** the shared editor opens with that series' current title, author, progress, publication state, and reading status populated

#### Scenario: Show reading status while editing
- **WHEN** the shared editor is opened in edit mode
- **THEN** the editor exposes the reading status selector, including Reading, Completed, and Dropped

#### Scenario: Preserve direct edit links
- **WHEN** a user opens a direct edit link for a series
- **THEN** the system resolves the link to the shared editor in edit mode with the dashboard context available

#### Scenario: Use compact mobile editor
- **WHEN** a user opens the shared editor on a small viewport
- **THEN** the editor uses a full-screen sheet layout with all fields and actions accessible without horizontal scrolling

#### Scenario: Validate shared editor input
- **WHEN** a user submits invalid title, author, series length, released count, or books-read values from either mode
- **THEN** the editor displays the relevant validation message without losing the entered values
