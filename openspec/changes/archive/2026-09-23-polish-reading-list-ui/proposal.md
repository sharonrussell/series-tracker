# Proposal

## Why

The app is functionally focused, but the current UI still feels utilitarian and slightly plain. A visual polish pass can make the reading list feel clean, modern, responsive, and approachable without changing tracking behavior.

## What Changes

- Refine the visual system with softer, less harsh light and dark palettes.
- Improve spacing, typography, row rhythm, controls, and card/drawer presentation.
- Retain the existing dark mode toggle and theme persistence behavior.
- Add a small whimsical accent appropriate to a reading tracker without making the interface decorative or distracting.
- Preserve current dashboard, filtering, editor, progress, and derived-status behavior.
- Keep the filter and Add controls visible while the series list scrolls independently.
- Smoke test the polished UI in light and dark modes at desktop and mobile sizes.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Improve visual presentation and responsive polish while preserving existing behavior.

## Impact

- Affected CSS theme tokens, layout spacing, independently scrolling table/list presentation, controls, editor drawer styling, and possibly minimal markup for a decorative/semantic accent.
- No data model, persistence, validation, routing, or status-derivation changes are expected.
- No new external UI dependency is expected.
