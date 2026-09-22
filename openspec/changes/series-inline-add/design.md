# Design

## Context

The current dashboard presents a list of tracked series and uses a separate add form to create new entries. This keeps the app functional but introduces extra motion and a less compact personal inventory experience. The new behavior should keep the dashboard as the main place for reading and creating entries without a larger redesign.

## Goals / Non-Goals

**Goals:**
- Add a lightweight inline-create flow from the active dashboard.
- Keep the new entry consistent with the existing data model and progress rules.
- Preserve current dashboard behaviors like filtering, editing, and archiving.

**Non-Goals:**
- Replacing the entire dashboard with a JavaScript-heavy frontend.
- Introducing a separate create page or new storage model.
- Reworking the archive and status flows beyond what is required for the inline add row.

## Decisions

- Use a single Add button above the list as the entry point to create a draft row inline.
- Keep the row inside the existing dashboard form structure so the page model can handle a single save action and reuse the current database logic.
- Reuse the existing `SeriesItem` fields and validation semantics; the inline row will supply the title, total count, and current progress values already tracked by the app.
- Offer a cancel action for the draft row to avoid accidental creation and keep the dashboard stable.

## Risks / Trade-offs

- [UI density increases] → Keep the inline row visually distinct and short to avoid making the list harder to scan.
- [Draft creation can be mistaken for a final save] → Use a deliberate Save and Cancel pattern with clear labels and a single row context.
- [Existing dashboard logic may assume a fixed list state] → Submit the inline create through the same model-binding flow and refresh the list after successful save.

## Migration Plan

- No database migration is required because the existing model already stores the fields needed for the new row.
- The change is purely a dashboard UX enhancement and should be deployed as part of the existing Razor Pages app.
- Existing series data and current workflows remain unchanged; the new behavior is additive.

## Open Questions

- None at this time. The requested behavior is specific enough to proceed without product ambiguity.
