# Design

## Context

See `proposal.md` for motivation. The dashboard currently posts progress increments to the index page handler. Inline add and edit flows already validate or clamp manually entered progress against series length, but increment actions need the same invariant.

## Goals / Non-Goals

**Goals:**

- Keep `CurrentProgress` less than or equal to `TotalProgress` when dashboard increment actions run.
- Mark reading status completed when progress reaches the configured series length.
- Preserve existing behavior for ordinary increments, completed status updates, edit saves, and inline add validation.
- Cover boundary behavior with focused tests.

**Non-Goals:**

- Do not change the data model or database schema.
- Do not add new progress controls or change the visible dashboard layout.

## Decisions

- Clamp incremented progress in the dashboard update handler.
  - Rationale: The dashboard `+1` action is the path that can currently exceed the series length, so the invariant belongs at the point where the mutation occurs.
  - Alternative considered: hide or disable `+1` at the UI when progress is complete. Rejected for this change because server-side capping still needs to exist for correctness and is sufficient to satisfy the requirement.

- Keep completed status behavior unchanged.
  - Rationale: The app already sets progress to total length when reading status becomes completed, which is consistent with the cap.

- Infer completed reading status when saved progress reaches total length.
  - Rationale: A series with books read equal to series length is complete from the user's reading-progress perspective, and automatic completion prevents a contradictory active-at-100% state.
  - Alternative considered: keep completion explicit. Rejected because the requested behavior makes completion a consequence of reaching the progress cap.

## Risks / Trade-offs

- A user may click `+1` repeatedly at the cap and see no visible change -> The dashboard will remain accurate at 100% with completed reading status.
- Users may intentionally want a 100% series to remain active -> They can still edit status later if needed, but reaching the series length defaults to completed.
- Future progress update paths could bypass this handler -> Add tests around the handler now and keep using shared clamping if more update paths are introduced later.

## Migration Plan

1. Update dashboard progress increment logic to clamp at series length and mark completed when progress reaches the cap.
2. Apply the same completion rule when add/edit saves progress equal to the series length.
3. Add tests for below-cap, exact-cap, and at-cap increment behavior.
4. Run `dotnet test --nologo` and `dotnet build --nologo`.

Rollback is to revert the handler and tests; no persisted schema changes are involved.
