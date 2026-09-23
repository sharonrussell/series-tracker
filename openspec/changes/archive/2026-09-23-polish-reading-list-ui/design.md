# Design

## Context

The app already has the core information architecture in place: minimal shell, `Reading List` page heading, consolidated series rows, dark-mode support, and a shared editor drawer. The remaining gap is visual polish: the current CSS is functional but still uses a utilitarian default font stack, strong blue accents, and simple card/table treatments.

## Goals / Non-Goals

**Goals:**
- Make the app feel clean, modern, calm, and responsive.
- Use softer light and dark palettes that avoid harsh contrast while keeping accessibility.
- Improve spacing, typography, row rhythm, controls, and editor presentation.
- Add one or two subtle reading-themed whimsical touches.
- Preserve current domain behavior and dark-mode toggle behavior.

**Non-Goals:**
- No new frontend framework or external UI library.
- No changes to persistence, routes, filters, progress, derived status, or editor semantics.
- No decorative illustration-heavy redesign.
- No landing page or marketing-style hero.

## Decisions

- Refine existing CSS tokens rather than replacing the UI architecture, because the current Razor structure is already aligned with the product workflow.
- Use a warmer neutral background and softened accent colors instead of the current sharper blue-centered palette.
- Improve typography through a more intentional font stack using available system fonts; avoid adding hosted font dependencies.
- Make the list feel less table-like through row spacing, clearer row hover/focus treatments, and better progress-note hierarchy while retaining table semantics.
- Constrain the list to an independently scrollable region with sticky column headings, while keeping the dashboard filter and Add controls outside that scroll region.
- Keep whimsy subtle: a small bookish glyph, bookmark-like accent, or page-edge detail is acceptable; animated or illustrative decoration is not.
- Verify light and dark modes separately because row-state colors and note colors can lose contrast in one theme even when they work in the other.

## Risks / Trade-offs

- [Risk] Too much whimsy can make the app feel less focused.
  -> Mitigation: limit playful details to small, nonessential visual accents and keep controls conventional.
- [Risk] Softer colors can reduce contrast.
  -> Mitigation: validate text, progress notes, row states, and controls in both themes through browser smoke tests.
- [Risk] Visual polish can accidentally change layout behavior.
  -> Mitigation: preserve table semantics and run desktop/mobile smoke checks for overflow and row navigation.

## Migration Plan

- Refine CSS tokens, typography, spacing, controls, row treatments, and editor surfaces.
- Add an independently scrolling list region that keeps dashboard controls visible.
- Add restrained reading-themed accent treatment.
- Refresh UI and smoke test light/dark, desktop/mobile, row navigation, editor drawer, filter, and theme toggle.
- No data migration or rollback script is required; rollback restores the prior stylesheet.

## Open Questions

None. The desired direction is clean, responsive, soft, modern, and lightly whimsical.
