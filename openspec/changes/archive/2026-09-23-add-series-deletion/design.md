# Design

## Context

The shared editor is rendered by `Pages/Index.cshtml` and already posts save requests through `IndexModel`. It has an edit-only Tracking section and a footer containing Cancel and Save. There is no delete handler or delete UI today.

## Goals / Non-Goals

**Goals:**
- Add a clear, edit-only destructive action.
- Require a native browser confirmation before sending a delete request.
- Permanently delete only the requested existing `SeriesItem` and return to the active filter.
- Cover successful deletion, cancellation, and missing-record handling with focused tests.

**Non-Goals:**
- No undo, archive, recycle bin, bulk deletion, or create-mode delete action.
- No changes to progress/status derivation or list ordering.
- No schema migration or external dependency.

## Decisions

- Place a destructive `Delete series` submit action in the edit drawer footer, visually separated from Save and omitted in create mode.
- Use the browser's confirmation dialog from the delete form submit event. It is accessible, blocks accidental submission, and needs no new modal state or script dependency.
- Add a dedicated POST handler that accepts the series ID, loads the entity by that ID, removes it, saves changes, and redirects with the current `StatusFilter`.
- Preserve standard Razor antiforgery protection by using a normal form post.
- Return `NotFound` when the requested series is absent to avoid ambiguous deletion behavior.

## Risks / Trade-offs

- [Risk] Deletion is irreversible.
  -> Mitigation: require confirmation and label the action as delete.
- [Risk] Multiple forms in the editor can make filter context easy to lose.
  -> Mitigation: include the active filter as a hidden value and redirect with it after deletion.
- [Risk] Native confirmation dialog appearance varies by browser.
  -> Mitigation: use it only as the safety gate; the visible delete action remains clear in the editor.

## Migration Plan

- Add the edit-only delete form/action and the delete POST handler.
- Add focused tests and browser smoke tests for confirmation, cancellation, and successful deletion.
- No data migration is required; rollback removes the delete UI and handler.
