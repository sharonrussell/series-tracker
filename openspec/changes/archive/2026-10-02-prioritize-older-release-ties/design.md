# Design

## Context

See [proposal.md](proposal.md) for the motivation and [specs/series-tracker/spec.md](specs/series-tracker/spec.md) for the behavior contract. The dashboard currently chooses the earliest Available unread title by series position, then ranks eligible series by read/planned proportion, fewer remaining books, last update, and ID. Finishable series are preferred after ranking.

## Goals / Non-Goals

**Goals:**
- Let release age resolve equal-progress recommendations without changing which titles are eligible or which book comes next within a series.
- Preserve deterministic selection and the priority of finishable series over nonfinishable ones.

**Non-Goals:**
- Change row ordering or filters, fetch publication metadata, assign urgency scores, or alter the summary copy.

## Decisions

### Compare the selected next book's date

Insert an ascending release-date comparison after read/planned progress and before the existing remaining-book, update-time, and ID comparisons. Use the next Available unread title already selected by series position rather than the oldest of all unread titles, which could recommend a different book than the one shown. An eligible selected title necessarily has a release date on or before the captured dashboard date; future and undated titles remain ineligible. Keep finishable-series preference as a separate selection step after ranking so an older nonfinishable release cannot displace a finishable series.

### Keep the existing fallbacks

When the selected dates match, prefer fewer unread planned titles, then the most recently updated series, then stable ID order. This retains predictable behavior when books share a release date. Tests should use equal progress with conflicting remaining counts or update times to prove release age takes precedence, plus equal-date ties to prove the old fallbacks still apply. A higher progress proportion must continue to win regardless of release age within the same finishability group.

## Risks / Trade-offs

- [A very old unread book wins a tie repeatedly] -> This is the intended catch-up bias; reading or changing its date recomputes the suggestion on the next dashboard view.
- [Different titles in one series have dates out of reading order] -> Compare the next book in series order, not a later book with an older date.
- [No valid release date exists] -> Existing availability filtering excludes the title; do not synthesize a date or modify persisted metadata.

## Migration Plan

No storage migration is required. Deployment and rollback affect only transient recommendation ordering; existing titles, dates, and progress remain unchanged.