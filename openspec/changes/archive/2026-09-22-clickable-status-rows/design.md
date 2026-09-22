# Design

## Context

See `proposal.md` for motivation. The dashboard currently renders a table with detailed progress columns and an actions column containing edit, progress, complete, and drop controls. The edit page already supports changing series details, released count, progress, and reading status.

## Goals / Non-Goals

**Goals:**

- Make each dashboard row navigate to the existing edit page.
- Remove the actions column and all dashboard row action controls.
- Remove detailed progress columns for series length, released so far, and books read from the dashboard list.
- Keep a summary progress fraction visible.
- Color rows by reading status: completed green, reading amber, dropped red.
- Keep all mutation workflows on the edit page.

**Non-Goals:**

- Do not remove progress, status, released count, or metadata editing from the edit page.
- Do not add client-side routing or JavaScript-only navigation.
- Do not change add-row behavior.

## Decisions

- Implement row navigation with a normal edit link available for the whole row.
  - Rationale: The behavior should work without custom JavaScript and should remain accessible to keyboard and assistive technology users.
  - Alternative considered: JavaScript `onclick` on table rows. Rejected because native links provide better accessibility and simpler behavior.

- Keep the dashboard as a table but reduce columns.
  - Rationale: The data is still tabular, but removing detailed progress columns and actions makes it easier to scan.
  - Alternative considered: convert to cards. Rejected because it is a larger layout change than requested.

- Use row-level CSS classes based on reading status.
  - Rationale: Status color should be tied to the row state and remain easy to maintain with the existing status enum.
  - Alternative considered: only color the status pill. Rejected because the request specifically says to color the row.

## Risks / Trade-offs

- Clickable table rows can be awkward if implemented with invalid nested links -> Use a valid link pattern and avoid nested interactive controls in the row.
- Strong row colors can reduce text readability -> Use light green/amber/red backgrounds with sufficient contrast.
- Removing quick actions adds one click for progress/drop/complete -> This is intentional because changes should happen on the edit screen.

## Migration Plan

1. Remove dashboard action forms and action column.
2. Remove dashboard columns for series length, released so far, and books read.
3. Make dashboard rows navigate to the edit page with an accessible label.
4. Add row status color classes for completed, reading, and dropped.
5. Run `dotnet test --nologo` and `dotnet build --nologo`.
6. Smoke test dashboard HTML for row links, removed columns/actions, and status row classes.
