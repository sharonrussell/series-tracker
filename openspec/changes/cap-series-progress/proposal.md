# Proposal

## Why

A series progress value should never exceed the configured series length, because over-counted progress makes completion percentages and reading status misleading. The inline add and edit flows already constrain progress, but dashboard progress increments need the same boundary.

## What Changes

- Prevent progress increment actions from increasing books read beyond the series length.
- Preserve existing behavior for valid increments below the series length.
- Keep completed-series behavior aligned with the existing rule that completed reading status sets progress to the series length.
- Add tests covering increment-at-limit and near-limit behavior.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Progress updates must cap books read at the configured series length.

## Impact

- Dashboard progress update page handler needs to clamp incremented progress to total series length.
- Tests need to cover dashboard update behavior at and below the cap.
- No data model, database schema, or external dependency changes are expected.
