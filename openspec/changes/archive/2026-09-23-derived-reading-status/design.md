# Design

## Context

Reading status is currently persisted and manually exposed alongside progress, which allows stale combinations such as Completed after books read is reduced. The dashboard already derives some presentation cues from progress, but status classes and save handlers still trust the stored status value.

## Goals / Non-Goals

**Goals:**
- Establish one deterministic derived-status function for dashboard display and persistence updates.
- Keep Dropped as the only manual override.
- Add Not started as a first-class status with muted blue-gray row styling.
- Recalculate status after every progress, released-count, series-length, publication-state, or dropped-override change.
- Preserve existing progress validation, filters, editor flow, and dark/light theme behavior.

**Non-Goals:**
- No database migration is required.
- No change to the meaning of books read, released count, or series length.
- No separate workflow for Up to date beyond derived display/filter behavior.
- No new external dependency or API surface.

## Decisions

- Centralize precedence in the model/domain boundary: manual Dropped first, then zero progress, completed publication at full length, ongoing publication caught up to releases, otherwise Reading.
- Treat derived statuses as the source for dashboard row classes, filters, and saved status values so UI and persistence agree.
- Remove manual Completed/Reading/Not started selection from the editor; keep only a Dropped override control, with an explicit way to restore automatic derivation.
- Preserve existing numeric progress and released-count validation before deriving status.
- Use a muted blue-gray row token for Not started in both light and dark themes.
- Considered deriving status only at render time, but rejected it because filters and later edits would continue to observe stale persisted values.

## Risks / Trade-offs

- [Risk] Existing persisted statuses may not match the new derived state until a record is read or updated.
  -> Mitigation: derive display/filter state from current values and normalize status during application load or save without requiring a schema migration.
- [Risk] Removing manual Completed selection changes the edit workflow.
  -> Mitigation: make the derivation rules visible through row color and progress notes, while preserving Dropped as the deliberate exception.
- [Risk] Up to date may be confused with Reading because both are ongoing.
  -> Mitigation: retain the existing `Up to date` progress note and distinct derived state/filter behavior.

## Migration Plan

- Add the derived-status calculation and normalize existing records during load/save.
- Update editor status controls, dashboard row classes, filters, and tests.
- Refresh the UI and smoke test all five states plus transitions after reducing progress.
- No database migration is required; rollback restores the previous persisted-status behavior.

## Open Questions

None. The precedence order, Dropped override, and Not started color were agreed during exploration.
