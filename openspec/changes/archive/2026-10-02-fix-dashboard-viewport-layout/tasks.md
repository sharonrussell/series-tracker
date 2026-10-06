# Tasks

## 1. Dashboard Viewport Layout

- [x] 1.1 Make the shared page shell fill the viewport and keep the footer at
      the bottom on short pages without overlaying long content; verify short
      and long pages at desktop and mobile sizes.
- [x] 1.2 Unify dashboard content width and gutters for the heading, summary,
      and list; verify their left and right edges align at desktop and mobile
      breakpoints.
- [x] 1.3 Let the dashboard reading-list region fill available vertical space
      and scroll internally for long lists while keeping the filter and Add
      controls usable; verify with empty, short, and long lists.
- [x] 1.4 Keep the theme toggle in the navbar on its own row and align its right
      edge with the dashboard Add control; verify both positions at desktop and
      mobile breakpoints.

## 2. Verification

- [x] 2.1 Run `dotnet build --nologo` and
      `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo`;
      verify both pass.
- [x] 2.2 Smoke-test desktop/mobile layouts, light/dark themes, editor
      usability, scrolling, footer placement, navbar/Add alignment, and
      browser-console cleanliness; run
      `openspec validate fix-dashboard-viewport-layout --strict` and
      `openspec validate --specs --strict`.
