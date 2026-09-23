# Spec Delta

## ADDED Requirements

### Requirement: Application shell is minimal and branded
The system SHALL present a minimal application shell that uses the display name `Series Tracker`, avoids scaffold placeholder pages, and keeps navigation focused on the tracker experience.

#### Scenario: View application shell
- **WHEN** a user opens the application
- **THEN** the shell displays `Series Tracker` as the visible app name without underscores

#### Scenario: Omit redundant dashboard navigation
- **WHEN** a user views the application shell
- **THEN** the shell does not show a separate Dashboard navigation link because the app name links to the tracker page

#### Scenario: Show focused page title
- **WHEN** a user views the tracker page
- **THEN** the app shows `Reading List` as the visible page heading while keeping `Series Tracker` as the shell brand

#### Scenario: Omit duplicate document title
- **WHEN** a user views the tracker page in the browser
- **THEN** the document title shows `Series Tracker` without repeating the name

#### Scenario: Omit placeholder privacy page
- **WHEN** a user views the application shell
- **THEN** the shell does not show Privacy navigation or footer links and the scaffolded Privacy page is not exposed

#### Scenario: Preserve theme control
- **WHEN** navigation links are simplified
- **THEN** the application still exposes the appearance-theme toggle in the shell
