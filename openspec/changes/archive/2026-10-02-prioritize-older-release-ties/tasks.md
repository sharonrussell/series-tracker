# Tasks

## 1. Recommendation Ranking

- [x] 1.1 Insert the selected next Available unread title's release date (oldest first) after read/planned progress in the dashboard summary ranking; verify a focused test where the older release wins despite more remaining books or an older series update.
- [x] 1.2 Preserve finishable-series priority, higher progress, and remaining-books/update-time/ID fallbacks for equal dates; verify unit tests for each ordering level, in-series position selection, and a date update changing the next dashboard recommendation.

## 2. Verification

- [x] 2.1 Run `dotnet build --nologo` and `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo`; verify the new and existing tests pass.
- [x] 2.2 Refresh the local dashboard with equal-progress sample series and verify the older available next book wins, with filtering, row navigation, list scrolling, themes, mobile layout, and browser console unaffected.
- [x] 2.3 Run `openspec validate prioritize-older-release-ties --strict` and `openspec validate --specs --strict`; verify both change and main specs remain valid.