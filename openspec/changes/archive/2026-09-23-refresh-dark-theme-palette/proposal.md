# Proposal

## Why

The current dark theme uses a green-charcoal surface and warm coral accent that feels visually disconnected from the calmer light theme. A muted midnight-blue palette will make dark mode feel more intentional while retaining its readable, low-glare character.

## What Changes

- Replace dark-theme surface, border, text, accent, and state-row colors with a muted midnight-blue palette.
- Preserve the existing light theme, persisted theme toggle, responsive layout, dashboard behavior, and editor behavior.
- Verify dark-mode contrast, responsive presentation, theme toggling, and browser-console cleanliness after the palette refresh.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Refine the observable dark-mode visual palette while preserving accessible, readable application surfaces and existing theme behavior.

## Impact

- Affected dark-mode CSS theme tokens in `wwwroot/css/site.css`.
- No data model, persistence, route, interaction, or external dependency changes.
