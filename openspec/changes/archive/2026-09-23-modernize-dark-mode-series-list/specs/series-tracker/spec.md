# Spec Delta

## MODIFIED Requirements

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single clickable dashboard list showing a consolidated series identity, summary progress, and row color as the reading-status indicator, without separate status or series-state columns.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all series in a readable list with a `Series` cell containing the title and author, a summary progress cell, and row colors, without status or series-state columns

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

### Requirement: User can choose the application appearance theme
The system SHALL provide an accessible appearance-theme toggle that supports light and dark themes, respects the system preference when no explicit choice exists, and persists an explicit user choice locally.

#### Scenario: Initial theme follows system preference
- **WHEN** a user opens the application without a saved theme choice
- **THEN** the application uses the device's light or dark color preference

#### Scenario: Toggle theme
- **WHEN** a user activates the appearance-theme toggle
- **THEN** the application switches between light and dark themes and updates the toggle's accessible label and state

#### Scenario: Persist theme choice
- **WHEN** a user selects a theme
- **THEN** the application stores that choice locally and restores it on later visits

#### Scenario: Theme covers the application surface
- **WHEN** a user views the application in dark mode
- **THEN** the shell, dashboard, editor drawer, controls, table rows, progress notes, overlays, and text use readable dark-theme colors with sufficient contrast

#### Scenario: Preserve theme accessibility
- **WHEN** a user navigates the theme toggle and dashboard controls by keyboard
- **THEN** the controls expose their current state and remain visibly focusable in both themes
