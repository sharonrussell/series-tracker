# Spec Delta

## MODIFIED Requirements

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