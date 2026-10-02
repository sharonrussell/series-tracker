# Spec Delta

## MODIFIED Requirements

### Requirement: Reading list UI is polished and responsive
The system SHALL present the reading list, shell, controls, status rows, and editor with a clean responsive visual design that uses soft colors, clear hierarchy, subtle reading-themed whimsy, and efficient viewport use while preserving established styling, behavior, and accessibility outside the explicit scope of each UI change.

#### Scenario: View polished reading list
- **WHEN** a user opens the tracker dashboard
- **THEN** the interface uses comfortable spacing, readable typography, soft color contrast, and a clear visual hierarchy for the page heading, controls, rows, and progress notes

#### Scenario: Preserve behavior during polish
- **WHEN** the visual design is updated
- **THEN** existing theme toggle, filtering, row navigation, editor, progress, and derived status behavior remains unchanged unless the change explicitly modifies that behavior

#### Scenario: Preserve established styling
- **WHEN** a UI change is made to a specific component or view
- **THEN** unaffected components and views retain their established dimensions, spacing, alignment, typography, colors, interaction states, and light/dark theme treatment

#### Scenario: Preserve accessibility during UI changes
- **WHEN** a UI change is implemented
- **THEN** existing keyboard operation, visible focus, accessible names, roles and states, readable contrast, and responsive no-overflow behavior remain functional and are regression checked

#### Scenario: Use restrained whimsy
- **WHEN** whimsical visual details are added
- **THEN** they are subtle, reading-themed, and do not reduce readability, density, or accessibility

#### Scenario: Support responsive polish
- **WHEN** a user views the app on desktop or mobile
- **THEN** the list, controls, editor, and theme toggle remain readable, aligned, and free of horizontal overflow

#### Scenario: Scroll the series list
- **WHEN** the series list exceeds its available dashboard space
- **THEN** the series rows scroll within the list region while the filter and Add controls remain visible and usable

#### Scenario: Use available list viewport space
- **WHEN** a user views the dashboard on a viewport with space below the list
- **THEN** the list region grows to use available vertical space without an unnecessarily large bottom gap, while longer lists scroll within the list region

#### Scenario: Keep the footer at the viewport bottom
- **WHEN** dashboard content is shorter than the viewport
- **THEN** the footer sits at the bottom edge of the viewport without covering page content, and when content exceeds the viewport the footer follows the content

#### Scenario: Align dashboard content edges
- **WHEN** a user views the dashboard on desktop or mobile
- **THEN** the page heading, summary, and reading-list container share consistent left and right content edges

#### Scenario: Align the theme toggle with the Add control
- **WHEN** a user views the dashboard on desktop or mobile
- **THEN** the theme toggle remains in the navbar on its own row and its right edge aligns with the dashboard Add control

#### Scenario: Preserve light and dark comfort
- **WHEN** a user views either light mode or dark mode
- **THEN** colors remain soft, readable, and not harsh on the eye across the shell, list, controls, editor, and row states