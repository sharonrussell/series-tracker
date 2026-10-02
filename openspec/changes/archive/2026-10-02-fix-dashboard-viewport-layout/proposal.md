# Proposal

## Why

The dashboard currently leaves excess space below the reading list on tall viewports, while the footer follows the content instead of sitting at the bottom on short pages. The page heading and list also appear horizontally offset because the dashboard and shared layout use nested containers with different gutters.

## What Changes

- Keep the shared footer at the bottom of the viewport when page content is shorter than the available height.
- Let the dashboard reading-list region use the available vertical space, retaining internal scrolling for long lists and avoiding an oversized empty bottom gap.
- Align the dashboard header, summary, and list to consistent left and right page edges across desktop and mobile sizes.
- Preserve existing editor layout, filtering, row navigation, themes, and responsive behavior outside these dashboard adjustments.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Specify balanced viewport use, footer placement, and consistent dashboard alignment.

## Impact

- Shared layout structure in `Pages/Shared/_Layout.cshtml`.
- Dashboard markup in `Pages/Index.cshtml` and layout rules in `wwwroot/css/site.css`.
- Dashboard layout smoke tests and responsive verification.