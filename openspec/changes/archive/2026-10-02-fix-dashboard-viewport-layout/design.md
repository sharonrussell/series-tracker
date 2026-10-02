# Design

## Context

The shared layout places the header, main content, and footer in normal document flow, with Bootstrap containers around both the navigation and page body. The dashboard adds its own max-width shell and horizontal padding. The series list also has a viewport-relative maximum height rather than flexing into remaining page space. See `proposal.md` for the motivation and the spec delta for the user-visible behavior.

## Goals / Non-Goals

**Goals:**
- Make the application shell fill the viewport and let the main region expand so the footer rests at the bottom on short pages without overlaying long pages.
- Let the dashboard list take remaining vertical space while retaining a bounded scroll region for large lists.
- Use one consistent horizontal alignment system for the dashboard heading, summary, and list across viewport sizes.
- Align the theme toggle's right edge with the dashboard Add control while keeping the toggle in the navbar on its separate row.

**Non-Goals:**
- Change the editor's internal scrolling, layout, or footer controls.
- Move the theme toggle out of the navbar or onto the dashboard control row, or redesign the dashboard's colors and typography.
- Change filter, row navigation, status, or summary behavior.

## Decisions

- Use a full-height flex-column application shell with a flexible main region instead of a fixed-position footer. A fixed footer could cover the last rows on small screens; normal flow lets long content push the footer below the viewport while short pages still anchor it at the bottom.
- Make the dashboard content a height-constrained flex column and let the list region grow with `min-height: 0` and internal overflow. This replaces the current fixed viewport cap as the primary sizing rule while preserving row scrolling.
- Establish a single shared content width and gutter for the dashboard header, summary, and list. Remove the competing nested horizontal padding for this page rather than compensating with separate offsets at each breakpoint.
- Position the navbar theme toggle against the shared dashboard content gutter so its right edge matches the Add control. Keep it vertically in the navbar; moving it beside the filter and Add control was rejected because the user wants the two controls on distinct rows.
- Keep the editor outside the dashboard's viewport-filling list layout; it should continue to size and scroll according to its current content.

## Risks / Trade-offs

- [Viewport units and flex sizing differ across short/mobile browser viewports] → Use dynamic viewport sizing with a minimum-height fallback and verify desktop and mobile layouts.
- [Expanding the list can make a sparse list appear overly tall] → Keep the list's existing surface framing and maintain a reasonable minimum/available-height balance; verify both empty and populated dashboards.