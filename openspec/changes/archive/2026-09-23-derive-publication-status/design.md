# Design

## Context

The app stores `CompletionState` separately from series length and released count, which allows inconsistent combinations such as all books read while publication remains ongoing. The derived reading-status work already moved reader state toward numeric derivation; publication state should follow the same pattern using availability data.

## Goals / Non-Goals

**Goals:**
- Derive publication status from released count and series length.
- Remove manual publication-state selection from create and edit flows.
- Keep reading status derived from progress, released count, series length, and Dropped override.
- Normalize counts when books read reaches the configured series length.
- Preserve existing validation that released count cannot exceed series length and books read cannot exceed released count.

**Non-Goals:**
- No database migration is required.
- No change to the meaning of series length, released so far, or books read.
- No new user preference or external dependency.
- No change to Dropped override behavior.

## Decisions

- Treat `CurrentReleasedCount == TotalProgress` as fully released; otherwise the publication is ongoing.
- Keep the existing `CompletionState` field as stored compatibility data, but update it from the derived publication status during create/edit save paths.
- Remove publication status controls from the editor and display the derived value where publication status is needed.
- When books read is set to total series length, normalize released count to total series length before deriving publication and reading status.
- Keep validation order explicit: clamp or reject invalid numeric inputs first, then derive publication status, then derive reading status.
- Considered deleting `CompletionState`, but rejected it because removing storage is unnecessary and would create migration work without user-facing benefit.

## Risks / Trade-offs

- [Risk] Existing saved `CompletionState` values may disagree with numeric counts until records are edited or normalized.
  -> Mitigation: derive display state from counts and update stored compatibility value during save/load normalization.
- [Risk] Users may still expect to mark publication status manually when release schedules are uncertain.
  -> Mitigation: keep series length and released so far editable; those values are the explicit source of publication truth.
- [Risk] Automatically setting released count to length when books read reaches length can surprise users.
  -> Mitigation: apply it only at the full-length boundary where the alternative would be invalid or contradictory.

## Migration Plan

- Add publication-status derivation helper and normalize save paths.
- Remove manual publication-state field from the editor and update tests.
- Refresh the UI and smoke test ongoing, fully released, completed reading, and reduced-progress cases.
- No schema migration is required; rollback restores manual publication selection.

## Open Questions

None. Publication status derives from released count versus series length, and full progress implies fully released publication.
