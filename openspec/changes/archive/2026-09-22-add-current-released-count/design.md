# Design

## Context

See `proposal.md` for motivation. The app currently stores `CurrentProgress` as books read and `TotalProgress` as total series length. Add/edit validation allows books read up to total length, and dashboard `+1` increments are capped by total length. That works for completed publications, but ongoing publications need a separate available/released count.

## Goals / Non-Goals

**Goals:**

- Persist current released count separately from total series length.
- Enforce `CurrentReleasedCount <= TotalProgress`.
- Enforce `CurrentProgress <= CurrentReleasedCount` across add, edit, and `+1` update paths.
- Show released count in dashboard, add row, and edit page.
- Keep automatic reading completion tied to total series length and publication completion state.

**Non-Goals:**

- Do not rename existing `CurrentProgress` or `TotalProgress` storage fields in this change.
- Do not integrate with an external publication metadata source.
- Do not infer future release schedules.

## Decisions

- Add `CurrentReleasedCount` as an integer on `SeriesItem`.
  - Rationale: It represents available material and gives validation a clear cap distinct from total series length.
  - Alternative considered: reuse `TotalProgress` for released count. Rejected because the request explicitly separates current released count from series length.

- Use `Released so far` as the UI label.
  - Rationale: It is short, readable, and avoids overloading `current` with books read.
  - Alternative considered: `Current released count`. Rejected because it is too technical for form/table labels.

- Compute progress percentage against released count.
  - Rationale: The dashboard `Progress` column should communicate reading progress against currently available content. This also makes 100% mean the user has read all released material.
  - Alternative considered: keep percentage against total series length. Rejected because that would continue treating unreleased books as unread progress.

- Keep reading status completion based on total length plus publication completion state.
  - Rationale: Reading all released books in an ongoing publication means up to date, not necessarily completed.
  - Alternative considered: complete when books read equals released count. Rejected because it would incorrectly complete ongoing series.

- Repair existing data by setting released count to a safe value.
  - Rationale: Existing records do not have released count. For completed publications, released count can be total length. For ongoing publications, released count must be at least books read and no more than total length.
  - Alternative considered: default all released counts to total length. Rejected because ongoing series would lose the distinction this feature adds.

## Risks / Trade-offs

- Existing ongoing series may initially show released count equal to books read -> This is safe and can be edited upward by the user.
- If total length is reduced below released count -> Clamp released count and books read to stay valid.
- More numeric fields can make the dashboard dense -> Keep labels concise and align the table columns.

## Migration Plan

1. Add `CurrentReleasedCount` to the model and EF mapping/startup repair.
2. Initialize existing records with a valid released count.
3. Update validation to enforce released count and progress invariants.
4. Update add/edit forms and dashboard display.
5. Update progress increment logic to cap at released count.
6. Update tests for validation, defaults, migration assumptions, progress percentage, add/edit mapping, and increment behavior.
7. Run `dotnet test --nologo`, `dotnet build --nologo`, and a local smoke test of add/edit/progress flows.

Rollback is to revert the model/UI changes and restore the previous local database file if needed.
