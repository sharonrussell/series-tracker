# Proposal

## Why

Application Quality repeatedly fails because four C# files violate the existing
final-newline formatting rule, despite passing build and tests. Local
verification is currently recommended before pull requests but does not require
every CI check to pass before an implemented OpenSpec change is declared
complete or archived.

## What Changes

- Add the missing final newlines in the four files identified by the failed CI
  run without changing application behavior or weakening formatting enforcement.
- Require local restore, Release build, tests, formatting verification, and
  strict main-spec validation using the same commands and order as Application
  Quality before implementation sign-off or archive.
- Require strict validation of the active change in addition to the CI checks,
  with recorded command outcomes and blockers for failed, skipped, or
  unavailable checks.
- Align README quality instructions and OpenSpec task, apply, and archive
  guidance with the completion gate; planning readiness alone is not
  implementation completion.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `series-tracker`: Add a development-process requirement that all application
  quality checks pass locally on the final implementation state before a change
  is considered complete or archived.

## Impact

- Source formatting only: `Data/DatabaseInitializer.cs`,
  `Models/SeriesTitle.cs`, `Pages/Series/Create.cshtml.cs`, and
  `Pages/Series/SeriesEditorPageModel.cs`.
- Completion guidance: `README.md` and `openspec/config.yaml`, using supported
  artifact rules and operation guidance rather than editing generated prompts or
  skills.
- The existing `.github/workflows/application-quality.yml` remains the command
  baseline; no CI checks are removed or suppressed.
- No new runtime dependencies, API changes, UI changes, database changes, or
  modifications to the unrelated Copilot setup workflow.
