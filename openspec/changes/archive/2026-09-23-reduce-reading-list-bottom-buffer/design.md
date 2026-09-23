# Design

## Context

The dashboard list already scrolls independently through `.table-wrap`, but its maximum height is limited to `58dvh` while the page shell reserves a large lower padding. This leaves unused viewport space below the list on taller screens.

## Goals / Non-Goals

**Goals:**
- Let the scrollable list use more available viewport height.
- Retain a modest lower page edge and the existing minimum list height.
- Keep Filter and Add outside the list scroll region.
- Preserve responsive behavior on smaller screens.

**Non-Goals:**
- No full-screen list takeover.
- No table markup, JavaScript, editor, or data changes.
- No changes to list filtering, ordering, or row semantics.

## Decisions

- Adjust only the dashboard shell spacing and list-region viewport constraint.
- Use dynamic viewport units with existing bounded height behavior so the list grows on larger screens without becoming unbounded.
- Keep the scroll container as the list owner; the page header controls remain outside it and visible.
- Validate desktop and mobile separately because available viewport height and browser chrome differ.

## Risks / Trade-offs

- [Risk] A taller list can push the footer below the initial viewport.
  -> Mitigation: retain a small lower buffer and validate desktop/mobile framing.
- [Risk] Increasing height can cause overflow on small screens.
  -> Mitigation: preserve the minimum height and smoke test narrow viewports.

## Migration Plan

- Reduce the dashboard's lower layout buffer and increase the bounded list viewport allowance.
- Refresh the app and verify list scrolling, persistent controls, footer spacing, and horizontal overflow at desktop and mobile sizes.
- Rollback restores the prior CSS spacing and height values; no data migration is required.
