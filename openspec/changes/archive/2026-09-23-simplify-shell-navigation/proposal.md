# Proposal

## Why

The app still shows default scaffold navigation and branding: redundant Dashboard navigation, a placeholder Privacy page, footer Privacy link, and `Series_Tracker` display text. This makes the app feel unfinished and visually busier than necessary for a single-screen personal tracker.

## What Changes

- Remove the Dashboard navigation link because the brand already links to the main tracker page.
- Remove the placeholder Privacy page and all Privacy navigation/footer links.
- Replace visible `Series_Tracker` text with `Series Tracker` in the document title, brand, and footer.
- Keep the theme toggle and dashboard route intact.
- Smoke test the dashboard shell after cleanup.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Simplify shell navigation and app display name.

## Impact

- Affected shared layout markup and scaffold Privacy page files.
- No data model, persistence, dashboard list, editor, or routing behavior changes beyond removing the Privacy page are expected.
