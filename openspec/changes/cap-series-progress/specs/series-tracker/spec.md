# Spec Delta

## MODIFIED Requirements

### Requirement: User can update progress for a series
The system SHALL allow a user to increase the recorded progress for a series without allowing progress to exceed the configured series length, and SHALL mark the series reading status completed when progress reaches the series length.

#### Scenario: Increase progress
- **WHEN** a user records progress for a series after reading more content and the updated value is within the series length
- **THEN** the system updates the series progress and refreshes the dashboard to reflect the new value

#### Scenario: Cap progress at series length
- **WHEN** a user records progress that would make books read exceed the series length
- **THEN** the system saves books read as the series length, marks the reading status completed, and does not exceed 100% completion

#### Scenario: Complete progress exactly
- **WHEN** a user records progress that makes books read equal the series length
- **THEN** the system saves the updated progress and marks the reading status completed
