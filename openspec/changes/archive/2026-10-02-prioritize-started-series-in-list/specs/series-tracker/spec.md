# Spec Delta

## MODIFIED Requirements

### Requirement: User can view all series at a glance

The system SHALL present each tracked series as one compact clickable dashboard
row containing series identity, read-versus-planned progress, and one
context-sensitive next-title or status line derived from title dates and read
values, without title lists, badges, progress bars, status columns, or inline
row actions.

#### Scenario: View dashboard

- **WHEN** a user opens the dashboard
- **THEN** each row shows the series title, author, `<read> / <planned> read`,
  and one derived secondary line

#### Scenario: Keep title details off the dashboard

- **WHEN** a series has one or more known titles
- **THEN** the dashboard does not render its full title list or per-title
  controls

#### Scenario: Dashboard actions omit manual complete

- **WHEN** a user views actions for a series
- **THEN** the dashboard does not show an actions column or a manual completion
  action

#### Scenario: Symbol row actions

- **WHEN** a user views a series row
- **THEN** the dashboard does not show row action symbols or per-title controls

#### Scenario: Symbol actions remain accessible

- **WHEN** a user focuses or hovers over dashboard list navigation
- **THEN** each clickable row provides an accessible edit navigation name

#### Scenario: Navigate to edit from row

- **WHEN** a user activates a dashboard row
- **THEN** the system opens that series in the shared editor with its ordered
  titles available

#### Scenario: Preserve derived row color

- **WHEN** a dashboard row is displayed
- **THEN** the row uses the color associated with its derived reading status
  while the text remains readable in light and dark themes

#### Scenario: Show progress on mobile

- **WHEN** the dashboard is viewed on a small viewport
- **THEN** progress and the secondary line move beneath the series identity
  without horizontal overflow

#### Scenario: Series state reflects completion

- **WHEN** every planned title is known, has a date on or before today, and is
  Read
- **THEN** the row renders in the completed color and its secondary line shows
  `Completed`

#### Scenario: Hide detailed progress columns

- **WHEN** a user views the dashboard
- **THEN** it does not show separate known, released, status, or
  publication-state columns

#### Scenario: Color rows by reading status

- **WHEN** a user views the dashboard
- **THEN** Not started, Reading, Up to date, Completed, and Dropped rows retain
  distinct restrained colors

#### Scenario: Indicate reading availability

- **WHEN** a series has an Available title that has not been Read
- **THEN** its secondary line identifies the earliest such title as
  `Next: <title>`

#### Scenario: Indicate that the user is up to date

- **WHEN** all Available titles are Read and no known Upcoming or Unknown title
  can be shown
- **THEN** the secondary line shows `Up to date`

#### Scenario: Identify dropped series in the list

- **WHEN** a series is Dropped
- **THEN** its secondary line shows `Dropped` and the row uses the dropped color

#### Scenario: Prefer started active series within a priority group

- **WHEN** a non-dropped started series and a non-dropped Not started series
  share an existing dashboard priority group in the All or To read view
- **THEN** the started series, which has at least one Read title, appears first
  without changing priority-group order, and most-recently-updated order remains
  the fallback within each started/not-started class; Dropped series remain in
  their existing lowest-priority group and ordering
