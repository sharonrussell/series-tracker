# Proposal

## Why

The dashboard row actions currently use text labels for repeated controls, which makes the action column visually heavier than necessary. Symbol buttons will make the row actions quicker to scan while preserving accessible names for users and assistive technology.

## What Changes

- Replace the dashboard row `Edit` text action with a pencil symbol.
- Replace the dashboard row `Drop` text action with a bin symbol.
- Add a dashboard row completion action using a tick symbol.
- Keep the existing `+1` progress action as a compact symbol-style action.
- Add accessible labels and hover titles/tooltips so symbol-only controls remain understandable.
- Preserve existing edit, progress, complete, and drop behavior.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Dashboard row actions use symbol-only controls while preserving action behavior and accessibility, including an explicit tick action for completion.

## Impact

- Dashboard Razor markup needs action button content and accessible labels updated.
- CSS may need a compact icon-button style for consistent sizing and alignment.
- Tests or smoke checks should verify the row no longer renders `Edit` and `Drop` as visible button text while still exposing accessible labels.
