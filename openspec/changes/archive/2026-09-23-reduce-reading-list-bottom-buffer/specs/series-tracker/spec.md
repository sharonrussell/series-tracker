# Spec Delta

## MODIFIED Requirements

### Requirement: Reading list UI is polished and responsive
The system SHALL present the reading list, shell, controls, status rows, and editor with a clean responsive visual design that uses soft colors, clear hierarchy, subtle reading-themed whimsy, and efficient viewport use while preserving existing behavior.

#### Scenario: View polished reading list
- **WHEN** a user opens the tracker dashboard
- **THEN** the interface uses comfortable spacing, readable typography, soft color contrast, and a clear visual hierarchy for the page heading, controls, rows, and progress notes

#### Scenario: Preserve behavior during polish
- **WHEN** the visual design is updated
- **THEN** existing theme toggle, filtering, row navigation, editor, progress, and derived status behavior remains unchanged

#### Scenario: Use restrained whimsy
- **WHEN** whimsical visual details are added
- **THEN** they are subtle, reading-themed, and do not reduce readability, density, or accessibility

#### Scenario: Support responsive polish
- **WHEN** a user views the app on desktop or mobile
- **THEN** the list, controls, editor drawer, and theme toggle remain readable, aligned, and free of horizontal overflow

#### Scenario: Scroll the series list
- **WHEN** the series list exceeds its available dashboard space
- **THEN** the series rows scroll within the list region while the filter and Add controls remain visible and usable

#### Scenario: Use available list viewport space
- **WHEN** a user views the dashboard on a viewport with space below the list
- **THEN** the list region uses the available viewport space efficiently without an unnecessarily large bottom buffer

#### Scenario: Preserve light and dark comfort
- **WHEN** a user views either light mode or dark mode
- **THEN** colors remain soft, readable, and not harsh on the eye across the shell, list, controls, editor, and row states
