# Design

## Context

The dashboard derives `UpToDate` when the user has read every released book in an ongoing series. Its row currently shares `--row-reading` with actionable `Reading` rows, even though the progress note already identifies it as `Up to date`. Not-started, completed, and dropped rows already have their own visual treatments.

## Goals / Non-Goals

**Goals:**
- Give up-to-date rows a distinct, calm cool-blue treatment in both themes.
- Preserve amber as the signal that unread released books are available.
- Retain independent treatments for not started, completed, and dropped states.

**Non-Goals:**
- No change to status derivation, availability counts, filters, or progress notes.
- No new labels, controls, or markup.
- No palette redesign outside the up-to-date row token and selector.

## Decisions

- Add a dedicated `--row-uptodate` token to each theme and use it only for `.status-row-uptodate`.
- Use a light cool-blue surface in light mode and a muted slate-blue surface in dark mode, chosen to remain distinct from muted blue-gray not-started rows and midnight-blue page surfaces.
- Keep existing completed green, dropped berry/red, and reading amber tokens unchanged.
- Verify all five row states in both themes, plus hover/focus visibility and text contrast.

## Risks / Trade-offs

- [Risk] The cool-blue up-to-date row can be confused with not started.
  -> Mitigation: choose clearly different blue value families and retain the existing `Up to date` progress note.
- [Risk] A theme token change can lose contrast in dark mode.
  -> Mitigation: browser smoke test computed styles and visual rendering in both themes.

## Migration Plan

- Add the up-to-date row tokens and update its selector.
- Refresh the dashboard and smoke test row states, theme toggle, and console output.
- Rollback restores the prior shared reading-row token; no data migration is needed.
