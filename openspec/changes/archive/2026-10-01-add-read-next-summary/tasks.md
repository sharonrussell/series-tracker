# Tasks

## 1. Recommendation Logic

- [x] 1.1 Derive eligible and finishable non-dropped, incomplete series from
      available unread titles using a supplied date; verify unit tests for
      final-book, multiple-book, unannounced, future, unknown, completed, and
      dropped cases.
- [x] 1.2 Rank finishable suggestions and the single read-next fallback by
      read/planned proportion, fewer remaining titles, update time, and stable
      ID; verify tests for ties, series-order title choice, one-winner selection
      without a count, empty results, and release-day rollover.
- [x] 1.3 Enforce a minimum planned length of two on new and edited series while
      preserving existing one-title data; verify default, validation,
      invalid-draft retention, and successful two-title saves in tests.

## 2. Dashboard Presentation

- [x] 2.1 Compute summary from the already-loaded unfiltered series on All and
      render one plain-text winner beneath `Up next`, with completion-aware copy
      or an empty message; verify dashboard tests for hidden summary on other
      filters and unchanged row navigation.
- [x] 2.2 Style a compact non-interactive summary consistent with light/dark
      themes and the existing list layout; verify desktop/mobile browser
      readability, editor numeric minimum, and no horizontal overflow.

## 3. Verification

- [x] 3.1 Run `dotnet build --nologo` and
      `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo`;
      verify the new and existing tests pass.
- [x] 3.2 Refresh the running UI and smoke test all summary branches, list
      scrolling, filtering, editor validation, both themes, mobile layout, and
      browser console; verify existing dashboard behaviors remain functional.
- [x] 3.3 Run `openspec validate add-read-next-summary --strict` and
      `openspec validate --specs --strict`; verify both the change and durable
      specs remain valid.
