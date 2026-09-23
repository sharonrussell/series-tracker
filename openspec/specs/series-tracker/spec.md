# series-tracker Specification

## Purpose
The series tracker lets a user maintain an up-to-date personal list of series they are following, track progress across each title, and move series through reading, completed, or dropped states without needing a separate tracking tool.

## Requirements

### Requirement: User can add a series to their list
The system SHALL allow a user to add a new series from a shared dashboard editor opened in create mode, with title, author, series length, current released count, and progress, without manually selecting publication status and without leaving the dashboard.

#### Scenario: Begin inline creation
- **WHEN** a user clicks the Add button above the series list
- **THEN** the system opens the shared series editor in create mode without navigating away from the dashboard

#### Scenario: Save inline series
- **WHEN** a user enters a title, author, series length, released so far, and books read so far and saves the row
- **THEN** the system creates the series record, derives publication status from released count and series length, closes the editor, and refreshes the dashboard to show it in the active list with its metadata and release progress

#### Scenario: New series defaults to reading
- **WHEN** a user opens the shared editor in create mode
- **THEN** the reading status defaults to Reading and the reading-status selector is not shown

#### Scenario: Cancel inline creation
- **WHEN** a user cancels the inline add row
- **THEN** the system closes the shared editor and leaves the existing series list unchanged

#### Scenario: Reject released count beyond series length
- **WHEN** a user enters released so far greater than the series length
- **THEN** the system rejects the draft and explains that released count cannot exceed series length

#### Scenario: Reject books read beyond released count
- **WHEN** a user enters books read greater than released so far
- **THEN** the system rejects the draft and explains that books read cannot exceed released count

### Requirement: User can maintain series metadata
The system SHALL allow a user to record and update the author and derive publication status from released count and configured series length for a tracked series.

#### Scenario: Edit series metadata
- **WHEN** a user edits an existing series and changes the author, series length, or released count
- **THEN** the system saves the updated metadata and shows it with the derived publication status on subsequent views

#### Scenario: Select publication completion state
- **WHEN** a user records series metadata
- **THEN** the system does not show a manual publication-state selector and instead derives whether the series is ongoing or fully released from released count compared with series length

### Requirement: User can view all series at a glance
The system SHALL present each tracked series in a single clickable dashboard list showing a consolidated series identity, derived reading status through row color, summary progress, and availability notes, without separate status or series-state columns.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** the system displays all series in a readable list with a `Series` cell containing the title and author, a summary progress cell, derived-status row colors, and no status or series-state columns

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
- **WHEN** a series has reached its configured series length and the publication is complete
- **THEN** the row renders in green and no separate series-state flag is shown in the list

#### Scenario: Hide detailed progress columns
- **WHEN** a user views the dashboard list
- **THEN** the system does not show separate status or series-state columns when row color already communicates reading status

#### Scenario: Color rows by reading status
- **WHEN** a user views the dashboard list
- **THEN** completed series rows are green, reading series rows are amber, not started series rows are muted blue-gray, and dropped series rows are red

#### Scenario: Indicate reading availability
- **WHEN** a series has released books the user has not read yet
- **THEN** the progress cell shows the number of books still to read without changing the column title

#### Scenario: Indicate that the user is up to date
- **WHEN** a series is ongoing and the user has read all currently released books
- **THEN** the progress cell shows `Up to date` without adding future unreleased-book counts

#### Scenario: Identify dropped series in the list
- **WHEN** a series has been dropped
- **THEN** the progress cell shows the numeric progress fraction with `Dropped` displayed beneath it as a secondary note, and the row uses the dropped color

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

### Requirement: Dashboard UI changes are refreshed and smoke tested
Any dashboard UI change SHALL be verified against a freshly refreshed browser view before it is considered complete.

#### Scenario: Validate a dashboard UI change
- **WHEN** a dashboard UI change is implemented
- **THEN** the developer refreshes the running UI and performs a smoke test that checks the changed behavior is visible and the dashboard remains usable

### Requirement: User can update progress for a series
The system SHALL allow a user to increase or reduce recorded progress from the shared editor, SHALL NOT allow books read to exceed the current released count, SHALL NOT allow current released count to exceed the configured series length, SHALL derive publication status from released count and series length, and SHALL derive reading status from the resulting progress unless the series is manually dropped.

#### Scenario: Increase progress
- **WHEN** a user records progress for a series after reading more content and the updated value is within the current released count
- **THEN** the system updates the series progress and refreshes the dashboard to reflect the new derived status

#### Scenario: Cap progress at series length
- **WHEN** a user records progress that would make books read exceed the series length
- **THEN** the system saves books read no higher than the current released count and does not exceed 100% progress against released content

#### Scenario: Complete progress exactly
- **WHEN** a user records progress that makes books read equal the series length
- **THEN** the system saves the updated progress, sets released count to the series length when necessary, derives publication status as fully released, and derives reading status as completed

#### Scenario: Keep ongoing publication in reading status
- **WHEN** a user records progress that makes books read equal the current released count and the current released count is less than series length
- **THEN** the system saves the updated progress, derives publication status as ongoing, and derives reading status as up to date without marking it completed

#### Scenario: Cap progress at released count
- **WHEN** a user records progress that would make books read exceed the current released count
- **THEN** the system saves books read as the current released count and does not exceed 100% progress against released content

#### Scenario: Reject released count beyond series length
- **WHEN** a user updates released so far to be greater than series length
- **THEN** the system rejects the update and explains that released count cannot exceed series length

#### Scenario: Reduce progress and recalculate status
- **WHEN** a user reduces books read on a series currently marked completed
- **THEN** the system recalculates the reading status from the new progress instead of leaving the series completed

### Requirement: User can change series status
The system SHALL allow a user to manually set a series to dropped, while reading, not started, up to date, and completed states are derived from progress, released count, series length, and publication completion state.

#### Scenario: Mark series complete
- **WHEN** a user sets reading status to completed
- **THEN** the system does not provide a manual completed override and derives completed only when books read equals series length and the publication state is completed

#### Scenario: Mark series dropped
- **WHEN** a user sets reading status to dropped
- **THEN** the system updates the status without changing recorded progress

#### Scenario: Restore series to reading
- **WHEN** a user edits a tracked series back to reading
- **THEN** the system derives not started, reading, up to date, or completed from the recorded progress and publication state

### Requirement: User can see derived reading status
The system SHALL derive the non-dropped reading status using this precedence: Dropped when manually overridden; Not started when books read is zero; Completed when books read equals series length and publication is fully released; Up to date when all released books are read while publication is ongoing; otherwise Reading.

#### Scenario: Derive not started
- **WHEN** books read is zero and the series is not dropped
- **THEN** the series status is not started and the row uses the muted blue-gray color

#### Scenario: Derive reading
- **WHEN** books read is greater than zero, unread released books remain, and the series is not dropped or completed
- **THEN** the series status is reading and the row uses the amber color

#### Scenario: Derive up to date
- **WHEN** books read equals the current released count, the current released count is less than series length, and the series is not dropped
- **THEN** the series status is up to date

#### Scenario: Derive completed
- **WHEN** books read equals series length and released count equals series length
- **THEN** the publication status is fully released, the series status is completed, and the row uses the green color

### Requirement: User can see derived publication status
The system SHALL derive publication status from released count compared with series length instead of requiring manual publication-state selection.

#### Scenario: Derive ongoing publication
- **WHEN** released so far is less than series length
- **THEN** the publication status is ongoing

#### Scenario: Derive fully released publication
- **WHEN** released so far equals series length
- **THEN** the publication status is fully released

#### Scenario: Full progress implies fully released publication
- **WHEN** books read is set to the configured series length
- **THEN** the system treats released so far as the series length and derives publication status as fully released

### Requirement: User can archive a finished or dropped series
The system SHALL NOT provide an archive workflow or dedicated archived view for tracked series.

#### Scenario: Archive a series
- **WHEN** a user chooses to archive a series that is finished or no longer being followed
- **THEN** the system does not provide an archive action and the series remains managed through reading, completed, or dropped reading status

#### Scenario: Restore an archived series
- **WHEN** a user uses the tracker after archive behavior is removed
- **THEN** the system does not provide a restore-from-archive action or archived view

### Requirement: User can filter active reading list by status
The system SHALL allow a user to filter the series list by not started, reading, up to date, completed, or dropped reading status.

#### Scenario: Filter by status
- **WHEN** a user selects a status filter
- **THEN** the system shows only series matching that reading status

#### Scenario: Status filter excludes archive state
- **WHEN** a user opens the status filter
- **THEN** the filter options include not started, reading, up to date, completed, and dropped statuses, and do not include archived
