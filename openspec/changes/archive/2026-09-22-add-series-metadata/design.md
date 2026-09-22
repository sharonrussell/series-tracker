# Design

## Context

The current Razor Pages app stores series in a single `SeriesItem` EF Core entity with title, optional notes, progress counts, reading status, and timestamps. The dashboard inline-add form captures title and progress, the edit page manages title/progress/status/notes, and the archived page displays retained records. See `proposal.md` for motivation and `specs/series-tracker/spec.md` for behavior changes.

## Goals / Non-Goals

**Goals:**

- Add persisted author metadata for each series.
- Add a publication completion state that is distinct from the existing user reading status.
- Keep create, edit, active dashboard, and archived display behavior consistent.
- Preserve existing reading status values and progress behavior.

**Non-Goals:**

- Do not change archive semantics or replace the existing reading status enum.
- Do not introduce a separate author table or external metadata lookup.
- Do not add sorting, searching, or filtering by author/publication state in this change.

## Decisions

- Add `Author` as a required string on `SeriesItem`.
  - Rationale: Author is core display metadata and should be captured alongside title for new entries.
  - Alternative considered: make author optional. Rejected because optional metadata would not solve the similar-title identification problem consistently.

- Add a new enum for publication state with `Ongoing` and `Completed` values.
  - Rationale: Publication state describes whether the series itself is complete, while `SeriesStatus` describes the user's reading lifecycle. Keeping them separate avoids ambiguous meanings for `Completed`.
  - Alternative considered: add values to `SeriesStatus`. Rejected because it would mix user progress state with publication state.

- Update the existing SQLite schema through EF Core model changes and local database recreation/migration appropriate for the current lightweight app.
  - Rationale: The app currently initializes SQLite directly and has no established migration history. A local MVP can use the simplest compatible persistence update, with tests covering defaults and validation.
  - Alternative considered: introduce a full migration workflow immediately. This may be useful later, but adds process weight before there is a migration pattern in the project.

- Add metadata controls to the existing Razor Pages rather than introducing a frontend framework.
  - Rationale: The app is server-rendered and intentionally simple for a .NET developer with limited frontend experience.
  - Alternative considered: use client-side dynamic editing. Rejected because the existing page-handler pattern already supports the desired behavior.

## Risks / Trade-offs

- Existing local records may not have author or publication state values -> Use safe defaults or a simple local migration path so existing data remains readable.
- Adding fields to the inline row could make the dashboard table too wide -> Adjust responsive styling and keep field labels concise.
- The word `Completed` appears in both reading status and publication state -> Label the new value clearly as series completion/publication state in UI and tests.

## Migration Plan

1. Add the model properties and enum with defaults for new instances.
2. Update database initialization/migration behavior so existing local databases receive usable values.
3. Update create/edit/display pages to bind and show the metadata.
4. Add tests for defaults, validation, and add/edit mapping.
5. Run `dotnet test --nologo`, `dotnet build --nologo`, and an app smoke check.

Rollback is to revert the model/UI changes and restore the previous SQLite database file from local backup if needed.
