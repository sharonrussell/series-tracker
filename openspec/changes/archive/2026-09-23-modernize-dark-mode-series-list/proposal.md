# Proposal

## Why

The current interface is visually dated and built around repeated light-theme color literals, which makes it difficult to evolve consistently and offers no dark-mode option. The dashboard also spends separate columns on title and author, creating unnecessary horizontal structure in the primary list.

## What Changes

- Modernize the visual system around semantic theme tokens, clearer hierarchy, refined spacing, and consistent surface/control styling.
- Add a visible sun/moon theme toggle in the application shell.
- Use the system color preference as the initial theme when no user choice exists, then persist explicit user choices locally.
- Support accessible light and dark themes across the shell, dashboard, editor drawer, forms, table rows, status colors, and overlays.
- Consolidate the dashboard `Title` and `Author` columns into a single `Series` cell with a prominent title and secondary author text.
- Preserve existing progress indicators, row-status colors, filtering, drawer behavior, and row navigation.
- Refresh the UI and smoke test theme switching and list readability at desktop and mobile sizes.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Add persisted theme selection and consolidate dashboard series identity into one readable list column.

## Impact

- Affected shared layout markup, dashboard markup, CSS theme tokens, and client-side theme persistence/toggle behavior.
- Existing progress, status, filtering, editor, and navigation behavior remains in place.
- No data model, persistence migration, or external dependency is expected.
