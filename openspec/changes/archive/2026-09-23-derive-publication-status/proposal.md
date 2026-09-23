# Proposal

## Why

Publication status can currently conflict with the numeric counts, such as a series showing all books read out of the full series length while still being marked ongoing. Publication status should be derived from released count versus series length so it cannot disagree with the underlying availability data.

## What Changes

- Derive publication status from released so far compared with series length.
- Treat released so far less than series length as Ongoing.
- Treat released so far equal to series length as Fully released.
- Remove manual publication-status selection from create and edit flows.
- Ensure books read equal to series length implies released so far equals series length and publication status is Fully released.
- Keep derived reading status dependent on the numeric counts and Dropped override.
- Refresh the UI and smoke test create/edit flows, derived publication status, and completed reading transitions.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Derive publication status from series length and released count instead of manually selecting it.

## Impact

- Affected series model helpers, add/edit validation and save handlers, editor fields, dashboard display, and regression tests.
- Existing stored publication-state values can be interpreted through the derived rule without a schema migration.
- No external dependency or API change is expected.
