# Spec Delta

## MODIFIED Requirements

### Requirement: User can choose the application appearance theme
The system SHALL provide an accessible appearance-theme toggle that supports a soft light theme and a muted midnight-blue dark theme, respects the system preference when no explicit choice exists, and persists an explicit user choice locally.

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

#### Scenario: Use a midnight-blue dark palette
- **WHEN** a user views the application in dark mode
- **THEN** the application uses muted midnight-blue backgrounds and surfaces, blue accents, soft blue-white text, and distinct but restrained reading, completed, and dropped row states

#### Scenario: Preserve theme accessibility
- **WHEN** a user navigates the theme toggle and dashboard controls by keyboard
- **THEN** the controls expose their current state and remain visibly focusable in both themes
