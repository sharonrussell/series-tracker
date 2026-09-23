# Proposal

## Why

Users can drop a series but cannot remove an entry they no longer want to retain. A guarded delete action lets users permanently clean up incorrect, duplicate, or unwanted records.

## What Changes

- Add an edit-only delete action to the shared series editor.
- Require explicit confirmation before permanently deleting a series.
- Remove the confirmed series record from SQLite and return to the active dashboard filter.
- Preserve the record when deletion is cancelled or dismissed.
- Add focused page-model coverage and browser smoke tests for deletion and cancellation.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Allow a user to permanently delete an existing series through the shared editor after explicit confirmation.

## Impact

- Affected shared editor markup, client-side confirmation behavior, `IndexModel` post handling, and page-model tests.
- Deletes are permanent; no recycle bin, undo workflow, or data migration is included.
- No external dependency changes.
