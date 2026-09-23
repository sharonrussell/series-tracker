# Spec Delta

## ADDED Requirements

### Requirement: User can permanently delete a series
The system SHALL allow a user to permanently remove an existing series from the shared editor only after explicit confirmation.

#### Scenario: Show deletion action while editing
- **WHEN** a user opens the shared editor for an existing series
- **THEN** the editor provides a clearly identified delete action

#### Scenario: Confirm deletion
- **WHEN** a user chooses the delete action
- **THEN** the system requires explicit confirmation before removing the series

#### Scenario: Delete confirmed series
- **WHEN** a user confirms deletion of an existing series
- **THEN** the system permanently removes that series record, closes the editor, and returns to the active dashboard filter without the deleted row

#### Scenario: Cancel deletion
- **WHEN** a user dismisses or cancels the deletion confirmation
- **THEN** the system keeps the series record unchanged and leaves the editor available

#### Scenario: Do not expose deletion while creating
- **WHEN** a user opens the shared editor in create mode
- **THEN** the system does not show a delete action

#### Scenario: Handle missing series deletion
- **WHEN** a user submits a deletion request for a series that no longer exists
- **THEN** the system does not delete another record and returns a not-found response
