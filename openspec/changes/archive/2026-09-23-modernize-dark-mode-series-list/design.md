# Design

## Context

The application currently uses repeated literal colors across a light-only stylesheet, Bootstrap light navbar classes, and a dashboard with separate title and author columns. The shared editor drawer introduced by the previous change is another major surface that must participate in theming.

## Goals / Non-Goals

**Goals:**
- Establish semantic CSS theme tokens for surfaces, text, borders, controls, accents, overlays, and row-status colors.
- Provide a visible sun/moon toggle in the application shell.
- Use `prefers-color-scheme` as the initial fallback and local storage for explicit user choice.
- Consolidate dashboard identity into a `Series` cell with title and author hierarchy.
- Preserve existing progress, status, filtering, editor, navigation, and accessibility behavior.
- Verify light/dark themes at desktop and mobile sizes.

**Non-Goals:**
- No new theme framework or external dependency.
- No server-side user preference or account synchronization.
- No redesign of the underlying series data model.
- No change to the meaning of row status colors or progress indicators.

## Decisions

- Use a small set of semantic custom properties on the document root and a dark-theme attribute/class rather than duplicating entire stylesheets. This makes future visual changes consistent and keeps the theme switch instant.
- Resolve the effective theme before normal page paint when possible, then let the toggle update the root theme state and local storage. System preference remains the fallback only when no explicit choice exists.
- Keep the theme control in the shared application shell so it works on the dashboard, editor drawer, and direct edit compatibility routes.
- Merge title and author into one dashboard cell, using a strong title line and a quieter author line; preserve the existing accessible row label and navigation target.
- Re-tune status-row backgrounds and progress-note colors per theme rather than relying on opacity overlays that can lose contrast.
- Considered a dark-only redesign, but rejected it because the user requested a mode toggle and the app should respect existing light-mode preference.

## Risks / Trade-offs

- [Risk] Theme initialization after first paint can cause a flash of the wrong theme.
  → Mitigation: add a small early theme initialization path and keep the default token set stable while the preference resolves.
- [Risk] Dark status-row colors can collapse distinctions between reading, completed, and dropped states.
  → Mitigation: define separate dark-theme row tokens and verify contrast and distinguishability in browser screenshots.
- [Risk] Consolidating columns reduces independent author sorting or scanning affordances.
  → Mitigation: preserve clear two-line hierarchy and retain author text in every row.

## Migration Plan

- Introduce semantic theme variables and shell toggle markup/behavior.
- Update dashboard identity markup and responsive table styles.
- Refresh the UI and smoke test light, dark, system-preference, desktop, and mobile states.
- No data migration is required; removing the theme preference falls back to system preference.

## Open Questions

None. The theme fallback, persistence, toggle, and consolidated series cell were agreed during exploration.
