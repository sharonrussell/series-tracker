# Design

## Context

The application currently stores planned length, released count, and read count directly on `SeriesItem`, derives status from those integers, and edits the values in a dashboard drawer. The new behavior needs ordered child records, richer client interaction, and a deliberate reset of the existing SQLite schema because aggregate counts cannot be reconstructed as named titles. The project continues to use Razor Pages, EF Core, SQLite, `EnsureCreated`, and the existing light/dark visual language without migrations or new UI dependencies.

## Goals / Non-Goals

**Goals:**

- Make Add and Edit use the same spacious, accessible form and title-row interactions.
- Keep title state internally valid by construction and derive all aggregate counts from child records.
- Keep dashboard rows compact while making the next actionable title visible.
- Preserve server-side validation and make JavaScript a progressive enhancement for live summaries and row manipulation.
- Perform the approved one-time data reset deterministically, without resetting compatible databases on later starts.

**Non-Goals:**

- Importing or synthesizing title names from existing aggregate data.
- Tracking editions, release dates, ISBNs, formats, or per-title notes.
- Automatically retrieving series metadata from an external service.
- Showing title lists or per-title controls on the dashboard.
- Introducing EF Core migrations or a frontend framework.

## Decisions

### Store ordered title entities and derive aggregates

Add a child entity for a known series title with its own identifier, parent series identifier, title text, zero-based position, and `Upcoming`, `Released`, or `Read` state. Configure a required one-to-many relationship with cascade deletion and a unique parent/position index. `SeriesItem` retains title, author, planned length, the manual Dropped override, and timestamps; released count, read count, unannounced count, publication state, and non-dropped reading status become derived values.

A single title-state enum encodes the invariant that a read title is released. Released count includes both Released and Read states, while read count includes only Read. This is preferable to two booleans, which admit the invalid combination `Read = true` and `Released = false`.

Alternative considered: keep persisted aggregate counts as a cache. Rejected because it creates two sources of truth and requires synchronization on every title mutation.

### Keep planned length independent from known titles

Planned length remains required and must be at least one. The ordered title count may range from zero through planned length; the difference is displayed as unannounced without creating placeholder records. A save is rejected when known title count exceeds planned length or any retained row has blank text.

Alternative considered: create unnamed title slots up to planned length. Rejected because placeholders clutter editing, complicate validation, and are not real known titles.

### Use dedicated Create and Edit pages with a shared form partial

The dashboard Add control navigates to a dedicated Create page, and activating a dashboard row navigates to a dedicated Edit page. Both render one shared form partial and view model so field ordering, validation, title controls, summary, and responsive behavior remain identical. Edit additionally exposes the existing delete action and Dropped toggle.

The page is organized as:

1. A compact header with Back and the Add/Edit title.
2. Series details with title, author, and planned length.
3. A live summary for planned, known, unannounced, released, and read counts.
4. Ordered title rows with a text input, three-state segmented control, up/down controls, and remove control.
5. A sticky action footer with Cancel and Add series or Save changes.

On narrow viewports, each title row stacks its input over the state control and row actions. Stable control dimensions and wrapping prevent horizontal overflow. Familiar icons are used for reorder and remove actions with accessible labels and tooltips.

Alternative considered: expand the existing dashboard drawer. Rejected because a variable-length title list would compete with the dashboard viewport and produce awkward nested scrolling, especially on mobile.

### Bind title drafts as an indexed collection and validate on the server

The shared form model binds an ordered collection of title drafts. Save handlers normalize positions from submitted order, validate all metadata and rows, replace the parent title collection transactionally, and re-render the submitted draft unchanged when validation fails. Client-side code manages add/remove/reorder actions, reindexes field names, enforces one selected state per row, and updates summary counts immediately; server behavior remains authoritative if JavaScript is unavailable.

Alternative considered: separate endpoints for each title mutation. Rejected because the application already uses explicit Save/Cancel semantics and atomic form submission avoids partially saved series edits.

### Derive dashboard content using deterministic precedence

Each dashboard row contains series title and author, `<read> / <planned> read`, and exactly one secondary line. The line is selected in this order:

1. `Dropped` for a dropped series.
2. `Next: <title>` for the earliest Released title.
3. `Upcoming: <title>` for the earliest Upcoming title.
4. `Completed` when all planned titles are Read.
5. `Up to date` when all released titles are Read but planned work remains.
6. `No titles announced` when no known title exists.

The query eagerly loads titles in position order before deriving counts, status, filters, sort priority, and secondary text. The dashboard keeps the existing two-column desktop structure and stacks progress beneath identity on mobile. It does not add badges, progress bars, title previews, or action columns.

Alternative considered: show next title alongside availability and unannounced counts. Rejected because multiple competing notes make scanning harder; full counts remain in the editor.

### Reset only legacy aggregate databases

Before calling `EnsureCreated`, startup inspects SQLite schema identity. If the legacy `Series` table exists but the new title table or expected title-based schema marker does not, the application deletes and recreates the database. A database already carrying the title-based schema is retained. This replaces the legacy additive column compatibility routine and makes the approved destructive transition explicit and idempotent.

The reset applies only to the configured tracker database. In-memory test databases create the new schema normally.

Alternative considered: preserve series metadata while discarding counts. Rejected by the selected clean-reset policy and because partial preservation adds migration code for data the user chose not to retain.

## Risks / Trade-offs

- **Existing tracker data is lost on first launch after upgrade** -> Make the reset condition narrowly detect the legacy schema and document the breaking behavior in the change and release notes.
- **Replacing a child collection can accidentally lose data on validation failure** -> Validate the bound draft before mutating tracked entities and save parent/title changes in one transaction.
- **Dynamic form indices can bind incorrectly after reorder or removal** -> Reindex every row and its labels after each client operation, plus cover non-contiguous edits with handler tests.
- **A long series can make the editor tall** -> Use a full page, compact rows, a sticky action footer, and normal document scrolling rather than nested scrolling.
- **Derived values may trigger repeated enumeration or database queries** -> Load each series with its titles once and derive values in memory from the loaded ordered collection.
- **Status wording can become noisy** -> Enforce one secondary dashboard line with the documented precedence.

## Migration Plan

1. Introduce the title entity, relationship, title-state enum, and derived series calculations.
2. Replace startup compatibility updates with legacy-schema detection and the one-time reset path.
3. On first application start against the aggregate schema, delete the configured SQLite database and create the title-based schema.
4. Build the shared form and Create/Edit pages, then update dashboard links and summaries.
5. Verify a second startup preserves newly entered title-based records.

Rollback requires restoring the previous application version together with a backup of the previous database. Data entered after the reset is not backward compatible with the aggregate schema.
