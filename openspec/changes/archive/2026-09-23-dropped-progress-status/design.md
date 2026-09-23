# Design

## Context

The dashboard already uses a compact progress cell with a primary fraction and optional secondary notes such as `to read` and `Up to date`. Dropped rows currently replace the fraction with `Dropped`, which loses useful reading history and differs from the other row states.

## Goals / Non-Goals

**Goals:**
- Keep the numeric progress fraction visible for dropped rows.
- Display `Dropped` beneath it using the existing secondary-note treatment.
- Preserve color-only status, row navigation, accessibility, and all non-dropped progress states.
- Refresh the UI and smoke test the result.

**Non-Goals:**
- No changes to stored series data or status transitions.
- No changes to row colors, filters, edit fields, routes, or APIs.
- No new UI component or dependency.

## Decisions

- Keep the existing progress fraction as the primary value for every row, including dropped rows.
- Render `Dropped` as a secondary note in the same progress cell rather than as a replacement value or separate column.
- Reuse the existing secondary-note styling so dropped rows remain visually consistent with `to read` and `Up to date`.
- Add focused model/view regression coverage for the dropped display while preserving current status and availability tests.
- Considered a separate dropped-status pill, but rejected it because the list intentionally uses row color as the status indicator and the progress cell already supports compact secondary context.

## Risks / Trade-offs

- [Risk] The dropped label may be less prominent than a dedicated status pill.
  → Mitigation: retain the dropped row color and use clear text directly beneath the progress fraction.
- [Risk] Users may interpret dropped progress as active reading progress.
  → Mitigation: the secondary `Dropped` note makes the status explicit without hiding the historical fraction.

## Migration Plan

- Update the progress-cell rendering and focused regression tests.
- Refresh the browser and smoke test reading, completed, up-to-date, and dropped rows.
- No data migration is required; reverting the presentation change restores the prior dropped-row output.

## Open Questions

None.
