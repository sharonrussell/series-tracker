# Design

## Context

The shared layout still contains default Razor Pages scaffold elements: a `Dashboard` nav link, a placeholder `Privacy` page and footer link, and visible `Series_Tracker` branding. The app now behaves as a single-purpose tracker with the dashboard as the main route and a shell-level theme toggle.

## Goals / Non-Goals

**Goals:**
- Replace visible `Series_Tracker` branding with `Series Tracker`.
- Remove redundant Dashboard navigation while keeping the brand link to the root tracker page.
- Remove the scaffolded Privacy page from navigation and footer.
- Preserve the theme toggle and existing dashboard/editor behavior.

**Non-Goals:**
- No change to routes used by the dashboard, editor drawer, or data operations.
- No change to the project namespace or assembly name.
- No new legal/privacy content.

## Decisions

- Treat this as shell cleanup rather than data or dashboard behavior work.
- Keep the brand link in the navbar as the only navigation path back to the tracker root.
- Remove the Privacy page files because the page contains only scaffold placeholder text and the app has no current privacy-policy content to show.
- Keep the document title suffix user-facing and readable as `Series Tracker` without changing the project name.

## Risks / Trade-offs

- [Risk] Removing Privacy could be undesirable if the app is later deployed publicly.
  -> Mitigation: a real privacy policy can be reintroduced later as a separate content change.
- [Risk] Removing the Dashboard link reduces explicit navigation text.
  -> Mitigation: the brand link remains in the conventional home position and the app only has one primary view.

## Migration Plan

- Update the shared layout shell text and links.
- Delete scaffold Privacy page files.
- Refresh the UI and smoke test the shell, brand link, theme toggle, and absence of Privacy links.

## Open Questions

None.
