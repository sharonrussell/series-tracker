# Spec Delta

## MODIFIED Requirements

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single dashboard view showing title, author, reading status, publication completion state, current progress, and compact symbol-based row actions.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all series in a readable list with their current progress values and metadata

#### Scenario: Dashboard actions omit manual complete
- **WHEN** a user views actions for a series
- **THEN** the system offers progress, edit, complete, and drop actions without archive actions

#### Scenario: Symbol row actions
- **WHEN** a user views row actions for a series
- **THEN** the edit action is shown as a pencil symbol, the complete action is shown as a tick symbol, and the drop action is shown as a bin symbol

#### Scenario: Symbol actions remain accessible
- **WHEN** a user focuses or hovers over a symbol action
- **THEN** the system provides an accessible action name for edit, progress, complete, and drop actions
