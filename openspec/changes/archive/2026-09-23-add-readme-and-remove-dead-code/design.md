# Design

## Context

The repository has no root README. The supported interaction path is the shared dashboard editor through `IndexModel.OnPostSaveAsync`, but the codebase also contains legacy add/update handlers and related tests. The stylesheet contains selectors for retired inline and pill-based UI that do not appear in the current dashboard markup.

## Goals / Non-Goals

**Goals:**
- Create a practical root README for local contributors.
- Remove implementation and tests that are demonstrably unreachable from supported workflows.
- Retain behavior covered by the current dashboard, direct edit redirect, persistence normalization, and derived-status model.

**Non-Goals:**
- No product behavior, schema, dependency, or visual redesign changes.
- No speculative refactoring or renaming.
- No removal of legacy compatibility routes unless reference analysis proves they are unreachable.

## Decisions

- Document only verified project facts: .NET 10 prerequisites, SQLite/EF Core persistence, local run and test commands, dashboard workflow, and project layout.
- Treat the shared editor/save handler as the supported mutation path. Remove legacy handlers only after confirming there are no Razor forms, JavaScript calls, or supported routes using them; remove tests that exclusively cover those handlers at the same time.
- Remove unused CSS selectors only after confirming they are absent from Razor markup and JavaScript selectors. Preserve generic Bootstrap override and active theme selectors.
- Keep the legacy edit GET redirect unless the audit establishes that it is unused, because it is an explicit compatibility path.

## Risks / Trade-offs

- [Risk] A legacy path may be used indirectly.
  -> Mitigation: search Razor, JavaScript, tests, and routes before deletion; run the full test suite and browser smoke test.
- [Risk] README guidance can drift from the application.
  -> Mitigation: derive commands and persistence notes from current project and launch configuration.
- [Risk] Removing tests can reduce meaningful coverage.
  -> Mitigation: remove only tests exclusively tied to retired handlers; retain current shared-editor and model behavior tests.

## Migration Plan

- Add the README first from verified repository facts.
- Remove confirmed unused handlers, tests, and CSS selectors together.
- Run build, full tests, and UI smoke checks; rollback restores the removed legacy code and styles if an unsupported dependency is discovered.
