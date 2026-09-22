# Spec Delta

## MODIFIED Requirements

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
