# Spec Delta

## ADDED Requirements

### Requirement: User can manage ordered titles within a series
The system SHALL allow a user to add, rename, remove, and reorder known titles within a series, and SHALL assign each known title exactly one state: Upcoming, Released, or Read.

#### Scenario: Add a known title
- **WHEN** a user adds a non-empty title while creating or editing a series
- **THEN** the system appends it to the ordered title list and includes it in the known-title count

#### Scenario: Change title state
- **WHEN** a user selects Upcoming, Released, or Read for a known title
- **THEN** the system stores that single state without exposing an invalid read-but-unreleased combination

#### Scenario: Reorder known titles
- **WHEN** a user moves a known title to a different series position
- **THEN** the system preserves the new order for subsequent editor and next-title views

#### Scenario: Remove a known title
- **WHEN** a user removes a title before saving the series
- **THEN** the title is excluded from the saved ordered list and all derived counts

#### Scenario: Reject blank known title
- **WHEN** a title row is present with no title text
- **THEN** the system rejects the draft and identifies the blank title row

### Requirement: User can see the next title in a series
The system SHALL derive one concise next-title message from ordered known titles and display it as the dashboard row's secondary status line.

#### Scenario: Show next released unread title
- **WHEN** one or more known titles are Released and unread
- **THEN** the dashboard shows `Next: <title>` for the earliest such title in series order

#### Scenario: Show next upcoming title
- **WHEN** no released unread title exists and a known title is Upcoming
- **THEN** the dashboard shows `Upcoming: <title>` for the earliest upcoming title in series order

#### Scenario: Show up-to-date fallback
- **WHEN** all known released titles are Read, at least one planned title remains unannounced, and no known title is Upcoming
- **THEN** the dashboard shows `Up to date`

#### Scenario: Show completed fallback
- **WHEN** every planned title is known and Read
- **THEN** the dashboard shows `Completed`

#### Scenario: Show no-announcement fallback
- **WHEN** the series has no known titles
- **THEN** the dashboard shows `No titles announced`

### Requirement: Application data adopts title-based tracking
The system SHALL initialize the title-based storage model without retaining aggregate-only series records from the previous schema.

#### Scenario: Start with the previous aggregate schema
- **WHEN** the application first starts after the title-tracking update against an existing aggregate-only database
- **THEN** it resets tracker data, creates the title-based schema, and starts with an empty series list

#### Scenario: Start again after schema adoption
- **WHEN** the application starts against an already compatible title-based database
- **THEN** it retains existing series and title records

## MODIFIED Requirements

### Requirement: User can add a series to their list
The system SHALL allow a user to add a new series from a shared editor with title, author, planned series length, and an optional ordered list of known titles, without manually entering released or read counts and without manually selecting publication or reading status.

#### Scenario: Begin inline creation
- **WHEN** a user activates the Add button above the series list
- **THEN** the system opens the shared series editor in create mode

#### Scenario: Save inline series
- **WHEN** a user enters valid series details and zero or more ordered known titles and saves
- **THEN** the system creates the series and its titles, derives all counts and statuses, closes the editor, and returns to the dashboard

#### Scenario: New series defaults to reading
- **WHEN** a user opens the shared editor in create mode
- **THEN** no reading-status selector is shown and status is derived from the submitted title states

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
- **WHEN** a user marks titles Released or Read
- **THEN** the derived released count cannot exceed planned series length because known titles beyond planned length are rejected

#### Scenario: Reject books read beyond released count
- **WHEN** a user marks a title Read
- **THEN** that title is also counted as released and the derived read count cannot exceed the derived released count

### Requirement: User can maintain series metadata
The system SHALL allow a user to update title, author, and planned series length while deriving known, unannounced, released, and read counts from the planned length and ordered title list.

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
- **WHEN** a user records series metadata and title states
- **THEN** the system does not show a manual publication-state selector and derives publication state from the title states and planned length

### Requirement: User can view all series at a glance
The system SHALL present each tracked series as one compact clickable dashboard row containing series identity, read-versus-planned progress, and one context-sensitive next-title or status line, without title lists, badges, progress bars, status columns, or inline row actions.

#### Scenario: View dashboard
- **WHEN** a user opens the dashboard
- **THEN** each row shows the series title, author, `<read> / <planned> read`, and one derived secondary line

#### Scenario: Keep title details off the dashboard
- **WHEN** a series has one or more known titles
- **THEN** the dashboard does not render its full title list or per-title state controls

#### Scenario: Navigate to edit from row
- **WHEN** a user activates a dashboard row
- **THEN** the system opens that series in the shared editor with its ordered titles available

#### Scenario: Preserve derived row color
- **WHEN** a dashboard row is displayed
- **THEN** the row uses the color associated with its derived reading status while the text remains readable in light and dark themes

#### Scenario: Show progress on mobile
- **WHEN** the dashboard is viewed on a small viewport
- **THEN** progress and the secondary line move beneath the series identity without horizontal overflow

#### Scenario: Dashboard actions omit manual complete
- **WHEN** a user views actions for a series
- **THEN** the dashboard does not show an actions column or a manual completion action

#### Scenario: Symbol row actions
- **WHEN** a user views a series row
- **THEN** the dashboard does not show row action symbols or per-title controls

#### Scenario: Symbol actions remain accessible
- **WHEN** a user focuses or hovers over dashboard list navigation
- **THEN** each clickable row provides an accessible edit navigation name

#### Scenario: Series state reflects completion
- **WHEN** every planned title is known and Read
- **THEN** the row renders in the completed color and its secondary line shows `Completed`

#### Scenario: Hide detailed progress columns
- **WHEN** a user views the dashboard
- **THEN** it does not show separate known, released, status, or publication-state columns

#### Scenario: Color rows by reading status
- **WHEN** a user views the dashboard
- **THEN** Not started, Reading, Up to date, Completed, and Dropped rows retain distinct restrained colors

#### Scenario: Indicate reading availability
- **WHEN** a series has a Released title that has not been Read
- **THEN** its secondary line identifies the earliest such title as `Next: <title>`

#### Scenario: Indicate that the user is up to date
- **WHEN** all released titles are Read and no known Upcoming title can be shown
- **THEN** the secondary line shows `Up to date`

#### Scenario: Identify dropped series in the list
- **WHEN** a series is Dropped
- **THEN** its secondary line shows `Dropped` and the row uses the dropped color

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
- **THEN** the list region uses the available viewport space efficiently without an unnecessarily large bottom buffer

#### Scenario: Preserve light and dark comfort
- **WHEN** a user views either light mode or dark mode
- **THEN** colors remain soft, readable, and not harsh on the eye across the shell, list, controls, editor, and row states

### Requirement: Shared series editor provides consistent add and edit flow
The system SHALL provide one responsive editor structure for creating and editing series, grouping series details, planned length, ordered title rows, live summary counts, and save/cancel actions into a clear flow.

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
- **THEN** it exposes only the Dropped manual override and derives all other reading statuses from title states

#### Scenario: Preserve direct edit links
- **WHEN** a user opens a direct edit link for a series
- **THEN** the system renders the same shared editor structure in edit mode for that series

#### Scenario: Edit a title row
- **WHEN** a user works with a title row
- **THEN** the editor provides its series position, title input, one Upcoming/Released/Read state control, reorder controls, and a remove action

#### Scenario: Update live summary
- **WHEN** the user changes planned length, adds or removes a title, changes title state, or reorders titles
- **THEN** the editor updates the planned, known, unannounced, released, and read summary without requiring a save

#### Scenario: Use compact mobile editor
- **WHEN** the editor is viewed on a small viewport
- **THEN** title input and state controls stack without horizontal scrolling and save/cancel actions remain readily accessible

#### Scenario: Validate without losing input
- **WHEN** the user submits invalid series metadata or title data
- **THEN** the editor displays the relevant validation message without losing entered metadata, title order, or title states

#### Scenario: Validate shared editor input
- **WHEN** a user submits invalid title, author, planned-length, known-title, or title-state data from either mode
- **THEN** the editor shows the relevant validation message and preserves the submitted draft

### Requirement: User can update progress for a series
The system SHALL derive progress from the states of ordered known titles rather than accepting aggregate released or read counts.

#### Scenario: Mark a title released
- **WHEN** a user changes a title from Upcoming to Released and saves
- **THEN** the released count increases while the read count remains unchanged

#### Scenario: Mark a title read
- **WHEN** a user changes a title to Read and saves
- **THEN** both released and read counts include that title

#### Scenario: Move a read title back to released
- **WHEN** a user changes a title from Read to Released and saves
- **THEN** the read count decreases while the released count continues to include that title

#### Scenario: Move a title back to upcoming
- **WHEN** a user changes a title from Released or Read to Upcoming and saves
- **THEN** the title is excluded from both released and read counts

#### Scenario: Increase progress
- **WHEN** a user changes the next unread title to Read and saves
- **THEN** the derived read count increases and the dashboard reflects the recalculated status

#### Scenario: Cap progress at series length
- **WHEN** a user tries to add title rows beyond planned series length
- **THEN** the system rejects the draft so derived progress cannot exceed planned length

#### Scenario: Complete progress exactly
- **WHEN** every planned title is known and changed to Read
- **THEN** read and released counts equal planned length and the series becomes Completed

#### Scenario: Keep ongoing publication in reading status
- **WHEN** all currently Released titles are Read but at least one planned title remains Upcoming or unannounced
- **THEN** publication remains Ongoing and reading status becomes Up to date rather than Completed

#### Scenario: Cap progress at released count
- **WHEN** a title is changed to Read
- **THEN** it is included in both derived read and released counts so read count cannot exceed released count

#### Scenario: Reject released count beyond series length
- **WHEN** released and read title states would require more known titles than planned length
- **THEN** the system rejects the draft

#### Scenario: Reduce progress and recalculate status
- **WHEN** a title in a Completed series is changed from Read to Released or Upcoming
- **THEN** the system reduces derived progress and recalculates reading and publication status

### Requirement: User can see derived reading status
The system SHALL derive non-dropped reading status from planned series length and title states using this precedence: Not started when no title is Read; Completed when every planned title is known and Read; Up to date when every released title is Read but the series is not complete; otherwise Reading.

#### Scenario: Derive not started
- **WHEN** no known title is Read and the series is not dropped
- **THEN** the series status is Not started

#### Scenario: Derive reading
- **WHEN** at least one title is Read and at least one other title is Released and unread
- **THEN** the series status is Reading

#### Scenario: Derive up to date
- **WHEN** every Released title is Read and fewer than the planned number of titles are Read
- **THEN** the series status is Up to date

#### Scenario: Derive completed
- **WHEN** the number of Read titles equals planned series length
- **THEN** the series status is Completed

#### Scenario: Preserve dropped override
- **WHEN** a series is manually marked Dropped
- **THEN** its status remains Dropped without changing title states

### Requirement: User can see derived publication status
The system SHALL derive publication status from title states and planned series length without exposing a manual publication-state control.

#### Scenario: Derive ongoing publication
- **WHEN** fewer titles are Released or Read than the planned series length
- **THEN** publication status is Ongoing

#### Scenario: Derive fully released publication
- **WHEN** the number of Released and Read titles equals planned series length
- **THEN** publication status is Fully released

#### Scenario: Full progress implies fully released publication
- **WHEN** every planned title is Read
- **THEN** every title also counts as released and publication status is Fully released

### Requirement: User can filter active reading list by status
The system SHALL allow a user to filter the dashboard by All, To read, Up to date, Completed, or Dropped using derived title state and series status.

#### Scenario: Filter by status
- **WHEN** a user selects a dashboard filter
- **THEN** the system shows only series matching that filter's derived criteria

#### Scenario: Filter series with unread released titles
- **WHEN** a user selects To read
- **THEN** the system shows only series containing at least one Released title

#### Scenario: Filter up-to-date series
- **WHEN** a user selects Up to date
- **THEN** the system shows only non-dropped, incomplete series whose released titles are all Read

#### Scenario: Filter completed series
- **WHEN** a user selects Completed
- **THEN** the system shows only series whose planned titles are all known and Read

#### Scenario: Filter dropped series
- **WHEN** a user selects Dropped
- **THEN** the system shows only manually dropped series

#### Scenario: Status filter excludes archive state
- **WHEN** a user opens the status filter
- **THEN** the options are All, To read, Up to date, Completed, and Dropped and do not include Archived
