# Tasks

## 1. Title-Based Domain And Persistence

- [x] 1.1 Add the ordered title entity and Upcoming/Released/Read state, update the `SeriesItem` relationship and planned-length fields, and verify EF model tests cover required fields, cascade deletion, and unique per-series positions.
- [x] 1.2 Replace persisted aggregate calculations with derived known, unannounced, released, read, publication, reading-status, and next-title helpers, and verify focused unit tests cover every state and precedence branch.
- [x] 1.3 Configure the EF Core title relationship and ordered loading in `SeriesTrackerDbContext`, and verify an in-memory SQLite test can create, retrieve in order, and cascade-delete a series with titles.
- [x] 1.4 Replace legacy additive startup updates with narrowly scoped legacy-schema detection and one-time database reset, and verify integration tests show legacy data is reset once while compatible title-based data survives subsequent startup checks.

## 2. Shared Add And Edit Flow

- [x] 2.1 Create the shared series form model and server validation for metadata, planned length, ordered title drafts, and title states, and verify tests reject blank title rows and known-title counts above planned length without losing draft values.
- [x] 2.2 Build dedicated Create and Edit Razor Pages around one shared form partial, including prepopulation, transactional title replacement, cancel navigation, Dropped handling on edit, and existing delete behavior; verify page-handler tests cover successful create, edit, cancel, delete, and validation paths.
- [x] 2.3 Add accessible title-row controls for adding, removing, and moving rows and selecting one state, and verify keyboard labels, tooltips, boundary-disabled reorder buttons, and posted ordering in focused UI or handler tests.
- [x] 2.4 Add progressive client behavior to reindex rows and update planned, known, unannounced, released, and read summaries immediately, and verify the form still posts a valid ordered collection when JavaScript is unavailable.
- [x] 2.5 Style the shared form for clear desktop hierarchy, compact stacked mobile rows, light/dark themes, and a sticky action footer, and verify no text or controls overlap or cause horizontal overflow at desktop and mobile widths.

## 3. Compact Dashboard

- [x] 3.1 Update dashboard queries to load ordered titles and apply All, To read, Up to date, Completed, and Dropped filters from derived state, and verify focused tests cover each filter and actionable-first ordering.
- [x] 3.2 Replace aggregate dashboard rendering with compact identity, `<read> / <planned> read`, and exactly one secondary line using Dropped/Next/Upcoming/Completed/Up to date/No titles announced precedence, and verify rendering tests cover each fallback.
- [x] 3.3 Route the dashboard Add control to Create and clickable rows to Edit while preserving keyboard navigation and accessible names, and verify both mouse and keyboard flows reach the expected page.
- [x] 3.4 Refine dashboard responsive styling so title details remain absent, desktop rows retain the two-column scan pattern, and mobile progress stacks without overflow; verify both themes at desktop and mobile widths.

## 4. Verification

- [x] 4.1 Run `dotnet build --nologo` and resolve any errors introduced by the change.
- [x] 4.2 Run `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo` and verify all domain, persistence, page-handler, filtering, and rendering tests pass.
- [x] 4.3 Start the app with a legacy database, verify the approved reset and new schema, create and edit a series, restart, and verify title-based data persists without another reset.
- [x] 4.4 Smoke test desktop and mobile Add, Edit, title ordering, state changes, live counts, dashboard next-title summaries, filtering, deletion, theme switching, list scrolling, keyboard operation, and browser-console cleanliness.
- [x] 4.5 Run `openspec validate redesign-series-add-edit-with-title-tracking --strict` and verify the completed implementation still satisfies the change contract.
