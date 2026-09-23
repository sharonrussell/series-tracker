# Design

## Context

The current app has two presentation paths for the same series data: an inline add form embedded in the dashboard table and a separate edit page. The add form is a dense multi-column row, while the edit page is a generic stacked form and removes the user from the dashboard context. Existing Razor Page handlers already contain the persistence and validation rules, and the client script already owns row activation behavior.

## Goals / Non-Goals

**Goals:**
- Establish one shared dashboard editor for create and edit.
- Keep the dashboard visible while editing and preserve direct edit-link compatibility.
- Make create and edit field visibility intentional: new records default to Reading; existing records expose reading status.
- Improve grouping, spacing, responsive behavior, validation visibility, and keyboard accessibility.
- Keep add/edit persistence rules and current series data unchanged.

**Non-Goals:**
- No database migration or new domain model.
- No change to progress/status business rules.
- No new frontend framework or external dependency.
- No redesign of the dashboard table beyond the editor entry point and overlay behavior.

## Decisions

- Use a right-side drawer on desktop and a full-screen sheet on small viewports. This keeps list context visible while providing enough width for grouped form fields.
- Render one shared form partial or equivalent shared markup for both modes, with an explicit mode flag controlling whether reading status is visible and which handler/identifier is posted.
- Keep the existing Razor Page persistence handlers initially and adapt their binding/redirect behavior rather than introducing a service layer solely for the UI change.
- Preserve `/Series/Edit/{id}` as a compatibility entry point that resolves to the dashboard with the drawer open for the requested record.
- Use a backdrop, close button, Escape handling, and focus management so the drawer behaves as an accessible modal surface.
- Group fields as Series details, Progress, and Tracking, with Save and Cancel actions fixed at the drawer footer.
- Considered a modal centered over the dashboard, but rejected it because the form has enough fields that a side panel provides better scanability and preserves more dashboard context.

## Risks / Trade-offs

- [Risk] Moving edit into a drawer may make deep-link and browser-back behavior less obvious.
  → Mitigation: preserve direct edit URLs and update client navigation/history behavior as part of the implementation tests.
- [Risk] A unified form can accidentally expose reading status during creation.
  → Mitigation: make mode-specific field visibility explicit and test create-mode defaults separately from edit mode.
- [Risk] Focus trapping and responsive behavior can introduce accessibility regressions.
  → Mitigation: add keyboard smoke coverage for open, close, Escape, focus, and submit behavior at desktop and mobile widths.

## Migration Plan

- Adapt the dashboard entry point, shared editor markup, edit compatibility route, client drawer behavior, and focused tests.
- Remove the old inline add presentation after the shared editor is functional.
- Refresh the UI and smoke test create and edit flows on desktop and mobile.
- No data migration is required; rollback restores the existing handlers and page layouts.

## Open Questions

None. The drawer, responsive sheet, and create/edit status distinction were agreed during exploration.
