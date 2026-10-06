# Proposal

## Why

When equally progressed series have available unread books, the current
read-next summary prefers fewer remaining titles or recent edits. Prioritizing
the older next release instead helps the user catch up on books that have been
waiting longer.

## What Changes

- Break ties in read-to-planned progress using the release date of each eligible
  series' next available unread title, oldest first.
- Keep finishable-series priority and highest progress proportion ahead of the
  new date comparison; retain the existing remaining-books, update-time, and
  stable-ID fallbacks when release dates tie.
- Leave the series rows, per-series title order, summary wording, and filtering
  unchanged.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Update the read-next summary's equal-progress ranking rule.

## Impact

- Recommendation ordering in the dashboard model and focused tie-break tests.
- No new data field, schema change, external service, or UI control is needed.
