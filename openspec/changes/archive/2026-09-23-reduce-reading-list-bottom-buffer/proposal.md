# Proposal

## Why

The dashboard list stops well above the bottom of larger viewports, leaving unused screen space below the list. Letting the list region use more of the available height improves scanning and reduces unnecessary scrolling.

## What Changes

- Increase the available vertical space for the independently scrollable series list.
- Reduce the visible buffer beneath the dashboard list while retaining comfortable page edges.
- Preserve the visible Filter and Add controls, list scrolling behavior, editor drawer, and responsive layout.
- Smoke test the adjusted list height at desktop and mobile viewport sizes.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Make the dashboard series-list region use available viewport space more efficiently while retaining persistent list controls.

## Impact

- Affected dashboard layout spacing and list-height CSS in `wwwroot/css/site.css`.
- No data model, persistence, route, interaction, or dependency changes.
