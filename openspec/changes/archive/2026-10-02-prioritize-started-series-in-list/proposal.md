# Proposal

## Why

The dashboard currently orders series within the same availability group by most
recent update, so a not-started series can appear ahead of one the user has
already begun. Putting started series first makes the reading list better
reflect the user's active reading without changing which availability groups
take priority.

## What Changes

- Within each existing non-dropped dashboard priority group, rank series with at
  least one Read title before series with no Read titles.
- Apply the ordering consistently to the All and To read views.
- Preserve existing availability-group priority, keep Dropped series in their
  current lowest group and ordering, and retain most-recently-updated ordering
  after the started/not-started distinction.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Define started-first ordering within existing dashboard
  priority groups.

## Impact

- Dashboard list ordering in `Pages/Index.cshtml.cs`.
- Dashboard ordering tests in `SeriesTracker.Tests/SeriesTrackerTests.cs`.
- The existing `series-tracker` dashboard requirement and its scenarios.
