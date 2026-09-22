# Design

## Context

The dashboard currently renders a publication-state pill alongside row colors and progress notes. Row color already communicates reading status, while the publication-state pill adds a second status-like signal to a compact table. Dropped rows also need a clear non-progress display because their reading progress is no longer an active reading cue.

## Goals / Non-Goals

**Goals:**
- Remove the series-state column from the dashboard list.
- Make row color the sole visual reading-status indicator in the list.
- Show `Dropped` in the progress area for dropped series.
- Preserve progress fractions, `to read` availability, `Up to date`, row navigation, and accessibility.
- Refresh the UI and smoke test the changed dashboard.

**Non-Goals:**
- No changes to series editing fields or persistence.
- No changes to row-color definitions or status-filter behavior.
- No new status icons, dependencies, or API surface.

## Decisions

- Remove the series-state column and its pill from the list rather than restyling it, because row color is already the established reading-status signal.
- Branch the progress-cell display by reading status: dropped rows show `Dropped`; other rows retain their existing progress and availability cues.
- Keep the row's accessible edit label and keyboard/click navigation unchanged.
- Add focused tests for the dropped display decision and retain the existing progress/status regression coverage.
- Considered adding a compact status icon instead of removing the pill, but rejected it because it would preserve duplicate status communication and require additional accessibility labeling.

## Risks / Trade-offs

- [Risk] Color-only status may be less obvious for users who do not distinguish the row palette.
  → Mitigation: preserve the existing high-contrast row colors and use `Dropped` text for the exceptional non-reading state.
- [Risk] Removing the publication-state column reduces visible metadata in the list.
  → Mitigation: publication state remains available on the edit screen, while the dashboard remains focused on reading status and progress.

## Migration Plan

- Update the dashboard markup and tests.
- Refresh the running UI and smoke test normal, up-to-date, completed, and dropped rows.
- No data migration or rollback script is required; reverting the presentation changes restores the previous list layout.

## Open Questions

None.
