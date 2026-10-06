# series-tracker Specification

## Purpose
The series tracker lets a user maintain an up-to-date personal list of series they are following, track progress across each title, and move series through reading, completed, or dropped states without needing a separate tracking tool.

## Requirements

### Requirement: User can add a series to their list
The system SHALL allow a user to add a new series from a shared editor with title, author, planned series length, and an optional ordered list of known titles, each with an optional release date and Read/Unread value, without manually entering released or read counts or selecting publication status.

#### Scenario: Begin inline creation
- **WHEN** a user activates the Add button above the series list
- **THEN** the system opens the shared series editor in create mode

#### Scenario: Save inline series
- **WHEN** a user enters valid series details and zero or more ordered known titles and saves
- **THEN** the system creates the series and its titles, derives all counts and statuses, closes the editor, and returns to the dashboard

#### Scenario: New series defaults to reading
- **WHEN** a user opens the shared editor in create mode
- **THEN** no reading-status selector is shown, new titles default to Unread, and series status is derived from their dates and read values

#### Scenario: Create series before titles are announced
- **WHEN** a user enters a planned series length but no known titles
- **THEN** the system creates the series with zero known, released, and read titles and an unannounced count equal to planned length

#### Scenario: Cancel inline creation
- **WHEN** a user cancels the add flow
- **THEN** the system closes the editor and leaves the series list unchanged

#### Scenario: Reject known titles beyond planned length
- **WHEN** a draft contains more known titles than its planned series length
- **THEN** the system rejects the draft and explains that known titles cannot exceed planned length

#### Scenario: Reject released count beyond series length
- **WHEN** titles have release dates on or before today
- **THEN** the derived released count cannot exceed planned series length because known titles beyond planned length are rejected

#### Scenario: Reject books read beyond released count
- **WHEN** a user marks a title Read
- **THEN** the title must have a date on or before today, so the derived read count cannot exceed the derived released count

### Requirement: User can maintain series metadata
The system SHALL allow a user to update title, author, and planned series length while deriving known, unannounced, released, and read counts from the planned length and ordered title dates and read values.

#### Scenario: Edit series metadata
- **WHEN** a user changes the series title, author, or planned series length and saves a valid draft
- **THEN** the system stores the updated metadata and recalculates derived counts and statuses

#### Scenario: Reduce planned length
- **WHEN** a user sets planned series length below the number of known titles
- **THEN** the system rejects the draft and preserves the entered values for correction

#### Scenario: Increase planned length on a completed series
- **WHEN** a user increases planned series length above the number of known read titles
- **THEN** the series is no longer Completed and its publication and reading statuses are recalculated

#### Scenario: Infer unannounced titles
- **WHEN** planned series length exceeds the number of known titles
- **THEN** the system reports the difference as the unannounced count without creating placeholder title records

#### Scenario: Select publication completion state
- **WHEN** a user records series metadata and title dates and read values
- **THEN** the system does not show a manual publication-state selector and derives publication state from release dates and planned length

### Requirement: User can manage ordered titles within a series
The system SHALL allow a user to add, rename, remove, and reorder known titles within a series, record an optional release date for each, and set only a Read/Unread value; it SHALL derive each title's availability as Unknown, Upcoming, or Available from its date.

#### Scenario: Add a known title
- **WHEN** a user adds a non-empty title while creating or editing a series
- **THEN** the system appends it to the ordered title list, includes it in the known-title count, and defaults it to Unread with no date

#### Scenario: Show unknown availability
- **WHEN** a title has no release date
- **THEN** its availability is Unknown, it does not count as released, and it cannot be marked Read

#### Scenario: Show upcoming availability
- **WHEN** a title has a release date after the current local calendar date
- **THEN** its availability is Upcoming, it does not count as released, and it cannot be marked Read

#### Scenario: Show available availability
- **WHEN** a title has a release date on or before the current local calendar date
- **THEN** its availability is Available and it counts as released regardless of whether it is Read

#### Scenario: Read an available title
- **WHEN** a user marks an Available title Read
- **THEN** the system stores its read value and includes it in both released and read counts

#### Scenario: Change title state
- **WHEN** a user changes a title between Read and Unread
- **THEN** the system stores only that read value and derives publication availability from its release date rather than offering manual Upcoming or Released states

#### Scenario: Reject read without an available date
- **WHEN** a user submits a Read title without a date or with a future date
- **THEN** the system rejects the draft, identifies the title, and retains the submitted date and read value for correction

#### Scenario: Change date on a read title
- **WHEN** a user removes or moves the release date of a Read title into the future
- **THEN** the system requires the user to mark it Unread or restore an available date before saving

#### Scenario: Advance past the release date
- **WHEN** an Unread title's release date becomes today without a user edit
- **THEN** it is Available and included in the released count on the next view

#### Scenario: Reorder known titles
- **WHEN** a user moves a known title to a different series position
- **THEN** the system preserves the new order for subsequent editor and next-title views

#### Scenario: Remove a known title
- **WHEN** a user removes a title before saving the series
- **THEN** the title is excluded from the saved ordered list and all derived counts

#### Scenario: Reject blank known title
- **WHEN** a title row is present with no title text
- **THEN** the system rejects the draft and identifies the blank title row

### Requirement: User can view all series at a glance
The system SHALL present each tracked series as one compact clickable dashboard row containing series identity, read-versus-planned progress, and one context-sensitive next-title or status line derived from title dates and read values, without title lists, badges, progress bars, status columns, or inline row actions.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** each row shows the series title, author, `<read> / <planned> read`, and one derived secondary line

#### Scenario: Keep title details off the dashboard
- **WHEN** a series has one or more known titles
- **THEN** the dashboard does not render its full title list or per-title controls

#### Scenario: Dashboard actions omit manual complete
- **WHEN** a user views actions for a series
- **THEN** the dashboard does not show an actions column or a manual completion action

#### Scenario: Symbol row actions
- **WHEN** a user views a series row
- **THEN** the dashboard does not show row action symbols or per-title controls

#### Scenario: Symbol actions remain accessible
- **WHEN** a user focuses or hovers over dashboard list navigation
- **THEN** each clickable row provides an accessible edit navigation name

#### Scenario: Navigate to edit from row
- **WHEN** a user activates a dashboard row
- **THEN** the system opens that series in the shared editor with its ordered titles available

#### Scenario: Preserve derived row color
- **WHEN** a dashboard row is displayed
- **THEN** the row uses the color associated with its derived reading status while the text remains readable in light and dark themes

#### Scenario: Show progress on mobile
- **WHEN** the dashboard is viewed on a small viewport
- **THEN** progress and the secondary line move beneath the series identity without horizontal overflow

#### Scenario: Series state reflects completion
- **WHEN** every planned title is known, has a date on or before today, and is Read
- **THEN** the row renders in the completed color and its secondary line shows `Completed`

#### Scenario: Hide detailed progress columns
- **WHEN** a user views the dashboard
- **THEN** it does not show separate known, released, status, or publication-state columns

#### Scenario: Color rows by reading status
- **WHEN** a user views the dashboard
- **THEN** Not started, Reading, Up to date, Completed, and Dropped rows retain distinct restrained colors

#### Scenario: Indicate reading availability
- **WHEN** a series has an Available title that has not been Read
- **THEN** its secondary line identifies the earliest such title as `Next: <title>`

#### Scenario: Indicate that the user is up to date
- **WHEN** all Available titles are Read and no known Upcoming or Unknown title can be shown
- **THEN** the secondary line shows `Up to date`

#### Scenario: Identify dropped series in the list
- **WHEN** a series is Dropped
- **THEN** its secondary line shows `Dropped` and the row uses the dropped color

#### Scenario: Prefer started active series within a priority group
- **WHEN** a non-dropped started series and a non-dropped Not started series share an existing dashboard priority group in the All or To read view
- **THEN** the started series, which has at least one Read title, appears first without changing priority-group order, and most-recently-updated order remains the fallback within each started/not-started class; Dropped series remain in their existing lowest-priority group and ordering

### Requirement: User can see the next title in a series
The system SHALL derive one concise next-title message from ordered known titles' availability and read values and display it as the dashboard row's secondary status line.

#### Scenario: Show next available unread title
- **WHEN** one or more known titles are Available and unread
- **THEN** the dashboard shows `Next: <title>` for the earliest such title in series order

#### Scenario: Show next released unread title
- **WHEN** a known title's release date is on or before today and it remains Unread
- **THEN** the dashboard shows `Next: <title>` for the earliest such title in series order

#### Scenario: Show next upcoming title
- **WHEN** no Available unread title exists and a known title is Upcoming
- **THEN** the dashboard shows `Upcoming: <title>` for the earliest upcoming title in series order

#### Scenario: Show next unknown title
- **WHEN** no Available unread or Upcoming title exists and a known title has no release date
- **THEN** the dashboard shows `Date unknown: <title>` for the earliest such title in series order

#### Scenario: Show up-to-date fallback
- **WHEN** all known Available titles are Read, at least one planned title remains unannounced, and no known title is Upcoming or Unknown
- **THEN** the dashboard shows `Up to date`

#### Scenario: Show completed fallback
- **WHEN** every planned title is known and Read
- **THEN** the dashboard shows `Completed`

#### Scenario: Show no-announcement fallback
- **WHEN** the series has no known titles
- **THEN** the dashboard shows `No titles announced`

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
The system SHALL provide one responsive editor structure for creating and editing series, grouping series details, planned length, ordered title rows with optional dates and Read/Unread controls, live summary counts, and save/cancel actions into a clear flow.

#### Scenario: Open editor in add mode
- **WHEN** the shared editor is opened to add a series
- **THEN** it presents empty series details, a planned-length control, an empty title list, an Add title action, and an Add series action

#### Scenario: Open editor in edit mode
- **WHEN** the shared editor is opened for an existing series
- **THEN** it presents the same field structure populated with the series metadata and ordered title rows and provides a Save changes action

#### Scenario: Edit existing series
- **WHEN** a user activates a series row
- **THEN** the shared editor opens in edit mode with the current metadata, planned length, Dropped override, and ordered title rows populated

#### Scenario: Show reading status while editing
- **WHEN** the shared editor is opened in edit mode
- **THEN** it exposes only the Dropped manual series override and derives all other reading statuses from title dates and read values

#### Scenario: Preserve direct edit links
- **WHEN** a user opens a direct edit link for a series
- **THEN** the system renders the same shared editor structure in edit mode for that series

#### Scenario: Edit a title row
- **WHEN** a user works with a title row
- **THEN** the editor provides its series position, title input, optional release-date input, Read/Unread control, reorder controls, and a remove action without a manual publication-state selector or availability label

#### Scenario: Update live summary
- **WHEN** the user changes planned length, adds or removes a title, changes a release date or read value, or reorders titles
- **THEN** the editor updates the planned, known, unannounced, released, and read summary without requiring a save

#### Scenario: Use compact mobile editor
- **WHEN** the editor is viewed on a small viewport
- **THEN** title, date, and read controls stack without horizontal scrolling and save/cancel actions remain readily accessible

#### Scenario: Validate without losing input
- **WHEN** the user submits invalid series metadata or title data
- **THEN** the editor displays the relevant validation message without losing entered metadata, title order, dates, or read values

#### Scenario: Validate shared editor input
- **WHEN** a user submits invalid title, author, planned-length, known-title, release-date, or read data from either mode
- **THEN** the editor shows the relevant validation message and preserves the submitted draft

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
The system SHALL derive progress from the dates and Read/Unread values of ordered known titles rather than accepting aggregate released or read counts.

#### Scenario: Title becomes available
- **WHEN** the local calendar date reaches an Unread title's release date
- **THEN** the released count increases without changing its read count

#### Scenario: Mark a title released
- **WHEN** a user records today's or an earlier release date for an Unread title and saves
- **THEN** the released count increases without changing its read count

#### Scenario: Mark a title read
- **WHEN** a user marks an Available title Read and saves
- **THEN** both released and read counts include that title

#### Scenario: Move a read title back to unread
- **WHEN** a user changes a Read title to Unread and saves
- **THEN** the read count decreases while the released count continues to include that title

#### Scenario: Move a read title back to released
- **WHEN** a user changes a Read title to Unread and saves
- **THEN** its released count remains unchanged because its date remains available, while the read count decreases

#### Scenario: Remove an unread title's date
- **WHEN** a user removes the date of an Unread title and saves
- **THEN** the title becomes Unknown and is excluded from both released and read counts

#### Scenario: Move a title back to upcoming
- **WHEN** a user changes an Unread title's release date to a future date and saves
- **THEN** the title becomes Upcoming and is excluded from both released and read counts

#### Scenario: Increase progress
- **WHEN** a user changes the next Available unread title to Read and saves
- **THEN** the derived read count increases and the dashboard reflects the recalculated status

#### Scenario: Cap progress at series length
- **WHEN** a user tries to add title rows beyond planned series length
- **THEN** the system rejects the draft so derived progress cannot exceed planned length

#### Scenario: Complete progress exactly
- **WHEN** every planned title is known, Available, and Read
- **THEN** read and released counts equal planned length and the series becomes Completed

#### Scenario: Keep ongoing publication in reading status
- **WHEN** all currently Available titles are Read but at least one planned title remains Upcoming, Unknown, or unannounced
- **THEN** publication remains Ongoing and reading status becomes Up to date rather than Completed, unless no title has been Read

#### Scenario: Cap progress at released count
- **WHEN** a title is changed to Read
- **THEN** it has a date on or before today and is included in both derived read and released counts

#### Scenario: Reject released count beyond series length
- **WHEN** the number of dated Available titles would exceed planned length
- **THEN** the system rejects the draft because known titles cannot exceed planned length

#### Scenario: Reduce progress and recalculate status
- **WHEN** a title in a Completed series is changed from Read to Unread
- **THEN** the system reduces derived progress and recalculates reading and publication status

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
The system SHALL derive non-dropped reading status from planned series length, date-derived availability, and title read values using this precedence: Not started when no title is Read; Completed when every planned title is known and Read; Up to date when every Available title is Read but the series is not complete; otherwise Reading.

#### Scenario: Derive not started
- **WHEN** no known title is Read and the series is not dropped
- **THEN** the series status is Not started

#### Scenario: Derive reading
- **WHEN** at least one title is Read and at least one other title is Available and unread
- **THEN** the series status is Reading

#### Scenario: Derive up to date
- **WHEN** every Available title is Read and fewer than the planned number of titles are Read
- **THEN** the series status is Up to date if at least one title is Read

#### Scenario: Derive completed
- **WHEN** the number of Read titles equals planned series length
- **THEN** the series status is Completed

#### Scenario: Preserve dropped override
- **WHEN** a series is manually marked Dropped
- **THEN** its status remains Dropped without changing title dates or read values

### Requirement: User can see derived publication status
The system SHALL derive publication status from the number of titles with release dates on or before the current local date and the planned series length without exposing a manual publication-state control.

#### Scenario: Derive ongoing publication
- **WHEN** fewer titles are Available than the planned series length
- **THEN** publication status is Ongoing

#### Scenario: Derive fully released publication
- **WHEN** the number of Available titles equals planned series length
- **THEN** publication status is Fully released

#### Scenario: Full progress implies fully released publication
- **WHEN** every planned title is Read
- **THEN** every title has an available release date and publication status is Fully released

### Requirement: Application data adopts title-based tracking
The system SHALL initialize date-based title storage on a clean database slate, without migrating or retaining records from an earlier title-state or aggregate-only schema.

#### Scenario: Start with the previous aggregate schema
- **WHEN** the application first starts after the release-date update against a previous aggregate-only database
- **THEN** it resets tracker data and creates the date-based title schema with an empty series list

#### Scenario: Start with an older tracker schema
- **WHEN** the application first starts after the release-date update against an older tracker database
- **THEN** it resets tracker data and creates the new date-based title schema with an empty series list

#### Scenario: Start again after schema adoption
- **WHEN** the application starts against an already compatible date-based database
- **THEN** it retains existing series and title records

### Requirement: User can archive a finished or dropped series
The system SHALL NOT provide an archive workflow or dedicated archived view for tracked series.

#### Scenario: Archive a series
- **WHEN** a user chooses to archive a series that is finished or no longer being followed
- **THEN** the system does not provide an archive action and the series remains managed through reading, completed, or dropped reading status

#### Scenario: Restore an archived series
- **WHEN** a user uses the tracker after archive behavior is removed
- **THEN** the system does not provide a restore-from-archive action or archived view

### Requirement: User can filter active reading list by status
The system SHALL allow a user to filter the dashboard by All, To read, Up to date, Completed, or Dropped using date-derived availability, read values, and derived series status.

#### Scenario: Filter by status
- **WHEN** a user selects a dashboard filter
- **THEN** the system shows only series matching that filter's derived criteria

#### Scenario: Filter series with unread available titles
- **WHEN** a user selects To read
- **THEN** the system shows only non-dropped series containing at least one Available unread title

#### Scenario: Filter series with unread released titles
- **WHEN** a user selects To read and a series has an Unread title dated today or earlier
- **THEN** the series is included unless it is Dropped, without requiring a manually selected Released state

#### Scenario: Filter up-to-date series
- **WHEN** a user selects Up to date
- **THEN** the system shows only non-dropped, incomplete series with at least one Read title whose Available titles are all Read

#### Scenario: Filter completed series
- **WHEN** a user selects Completed
- **THEN** the system shows only series whose planned titles are all known and Read

#### Scenario: Filter dropped series
- **WHEN** a user selects Dropped
- **THEN** the system shows only manually dropped series

#### Scenario: Status filter excludes archive state
- **WHEN** a user opens the status filter
- **THEN** the options are All, To read, Up to date, Completed, and Dropped and do not include Archived

### Requirement: User can see an actionable read-next summary
The system SHALL show a concise informational summary headed `Up next` above the reading list on the All view, based on the current local calendar date, prioritizing series that can be completed now and otherwise naming one available unread title in the most progressed eligible series.

#### Scenario: Identify finishable series
- **WHEN** a non-dropped, incomplete series has all planned titles known and every remaining unread title has a release date on or before today
- **THEN** the series counts as finishable and its next available unread title is eligible for the summary

#### Scenario: Summarize finishable series
- **WHEN** one or more series are finishable
- **THEN** the summary names only the highest-ranked series' next available unread book without showing a finishable-series count

#### Scenario: Highlight a final available book
- **WHEN** a finishable series has exactly one unread title
- **THEN** the summary says `Wrap up <series> with <title>` beneath the `Up next` heading

#### Scenario: Describe a finishable series with multiple unread books
- **WHEN** the highest-ranked finishable series has more than one available unread title
- **THEN** the summary says `Continue <series> with <title>` for its next available book beneath the `Up next` heading

#### Scenario: Do not promise completion prematurely
- **WHEN** a series has an unannounced title or any remaining title with a future or unknown release date
- **THEN** the summary does not count that series as finishable, even if it has only one planned unread book left

#### Scenario: Suggest progress when no series can be completed
- **WHEN** no series is finishable and at least one non-dropped, incomplete series has an available unread title
- **THEN** the summary shows `<title> from <series>` for the earliest available unread title in series order from the eligible series with the highest read-to-planned proportion

#### Scenario: Break equal progress ties
- **WHEN** eligible series share the highest read-to-planned proportion
- **THEN** the summary prefers the series whose next available unread title has the older release date, then fewer unread planned titles, then the most recently updated series, then a stable series identifier order

#### Scenario: Preserve finishable priority over older releases
- **WHEN** a finishable series and a nonfinishable series both have available unread titles and the nonfinishable series' next title was released earlier
- **THEN** the summary still recommends a finishable series

#### Scenario: Show nothing available now
- **WHEN** no non-dropped, incomplete series has an available unread title
- **THEN** the summary shows `Nothing available to read right now` without recommending a future, undated, completed, or dropped title

#### Scenario: Reflect release-day and progress changes
- **WHEN** the current date reaches a title's release date or the user changes a title's date or read value
- **THEN** the next dashboard view recalculates finishable counts and suggestions without requiring a separate status update

#### Scenario: Respect the dashboard filter
- **WHEN** the dashboard filter is not All
- **THEN** the summary is hidden and the existing filtered list continues to show its normal rows and controls

#### Scenario: Keep the summary informational
- **WHEN** a suggestion is displayed
- **THEN** it is plain readable text without a link or command, and existing series-row edit navigation remains available

### Requirement: Series plans contain at least two titles
The system SHALL require a planned series length of at least two when creating or updating a series, while retaining any already stored series with a shorter plan until the user edits it.

#### Scenario: Default to a valid planned length
- **WHEN** a user opens the series editor to create a series
- **THEN** the planned length starts at two and the input does not offer values below two

#### Scenario: Reject a one-title series on create
- **WHEN** a user submits a new series with a planned length below two
- **THEN** the system rejects the draft with a clear minimum-length message and preserves the entered values

#### Scenario: Reject a one-title series on edit
- **WHEN** a user submits changes to a series with a planned length below two, including an existing series saved before this rule
- **THEN** the system rejects the draft without deleting the series and preserves the entered values until the plan is raised to at least two

#### Scenario: Accept the minimum series length
- **WHEN** a user saves a valid series with a planned length of two
- **THEN** the series is saved and its normal title and progress rules continue to apply

### Requirement: Local application quality checks gate change completion
The project development workflow SHALL require every check enforced by Application Quality to run and pass locally against the final implementation state before an implemented OpenSpec change is declared complete or archived. The checks SHALL include dependency restore, Release build, the complete test suite, formatting verification without source changes, and strict main-spec validation, with the same targets, options, and ordering as CI. Strict validation of the active change SHALL also pass before completion. This gate SHALL apply to every implemented change, including specification-only changes, and SHALL supplement existing change-specific verification requirements.

#### Scenario: Complete a verified change
- **WHEN** an implemented change is ready for completion
- **THEN** every CI-equivalent check and strict active-change validation has passed locally against its final implementation state, with the commands and outcomes recorded in the change's verification summary

#### Scenario: Formatting failure blocks completion
- **WHEN** local formatting verification finds a missing final newline or another formatting violation
- **THEN** the change remains incomplete until the violation is corrected and the required checks pass without disabling formatting enforcement

#### Scenario: An unrun check blocks completion
- **WHEN** any required check is skipped, unavailable, or lacks a recorded local result
- **THEN** the missing check is reported as a blocker and the change is not declared complete or archived

#### Scenario: Any failing check blocks completion
- **WHEN** any required local check exits unsuccessfully
- **THEN** its failure is recorded and completion and archive remain blocked until the defect is resolved and the required verification succeeds

#### Scenario: Edits invalidate earlier verification
- **WHEN** implementation, workflow, or specification changes are made after a successful local verification pass
- **THEN** the required checks are rerun against the updated final state before completion or archive

#### Scenario: CI checks evolve
- **WHEN** Application Quality adds or changes a check
- **THEN** the local completion checklist is updated to match and completion requires a passing local result for the updated check

#### Scenario: Planning readiness is not implementation completion
- **WHEN** proposal, design, specification, and task artifacts exist but the change has not been implemented and locally verified
- **THEN** the change may be described as ready for implementation but not as an implemented, verified, or archive-ready change

#### Scenario: Verify a specification-only change
- **WHEN** an implemented change updates only specifications or development guidance
- **THEN** all CI-equivalent local checks and strict active-change validation still pass before it is declared complete or archived
