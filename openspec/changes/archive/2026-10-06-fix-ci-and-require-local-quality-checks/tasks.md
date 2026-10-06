# Tasks

## 1. Repair the Formatting Failure

- [x] 1.1 Add only the missing final newlines to `Data/DatabaseInitializer.cs`, `Models/SeriesTitle.cs`, `Pages/Series/Create.cshtml.cs`, and `Pages/Series/SeriesEditorPageModel.cs`; immediately verify with `dotnet format --verify-no-changes --no-restore` and confirm the diff contains no behavioral edits or weakened formatting rules.

## 2. Establish the Persistent Completion Gate

- [x] 2.1 Update README prerequisites and Quality Checks to list .NET 10 and the OpenSpec CLI, the exact five CI commands from `design.md` in CI order, strict active-change validation, and completion/archive blockers; verify every command's target and flags against `.github/workflows/application-quality.yml` and ensure the requirement covers spec-only changes and reruns after edits.
- [x] 2.2 Update `openspec/config.yaml` testing context, task artifact rules, and supported apply/archive operation guidance to require the complete local checklist, recorded evidence, and blocked sign-off for failed, skipped, or unavailable checks while preserving existing UI-specific verification; verify the YAML loads through `openspec context --json` and inspect `openspec instructions tasks --change fix-ci-and-require-local-quality-checks --json` for the task rule, then review the operation guidance for matching completion and archive obligations.
- [x] 2.3 Review README and OpenSpec guidance against every scenario in the delta spec, including changing CI checks, stale evidence, and planning readiness; verify there are no exceptions permitting completion with missing results and no edits to generated prompts/skills, runtime behavior, or unrelated workflows.

## 3. Verify the Final Implementation State

- [x] 3.1 Run the complete CI-equivalent sequence locally from the repository root after all implementation and guidance edits: `dotnet restore SeriesTracker.Tests/SeriesTracker.Tests.csproj`, `dotnet build SeriesTracker.Tests/SeriesTracker.Tests.csproj --configuration Release --no-restore --nologo`, `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --configuration Release --no-build --nologo`, `dotnet format --verify-no-changes --no-restore`, and `openspec validate --specs --strict`, followed by `openspec validate fix-ci-and-require-local-quality-checks --strict`; verify all six commands succeed and rerun the sequence after any subsequent implementation, workflow, or specification edits.
- [x] 3.2 Append a concise local verification summary here containing commands, outcomes, the implementation state tested, and relevant tool versions; verify every required check has a passing local result before marking final verification complete, otherwise leave it unchecked and explicitly record blockers. Distinguish local success from unobserved GitHub success and require evidence review again before archive.

## Verification Summary

Verified locally on 2026-10-06 on macOS against HEAD `3d4343e01e8ff181c496c76ec5924cfc96f2161f` plus this change's working-tree edits: final-newline-only repairs to the four specified C# files, README quality instructions, and OpenSpec project guidance. The workflow and formatter rules were unchanged. The full sequence ran after all implementation and guidance edits; only evidence and checkbox annotations were added afterward.

Tools: .NET SDK `10.0.301`, test runtime `.NET 10.0.9`, OpenSpec `1.13.1`, and Node.js `26.9.0`. CI uses Ubuntu and Node.js 22; these local results do not assert identical environments or remote success.

| Command | Local outcome |
| --- | --- |
| `dotnet restore SeriesTracker.Tests/SeriesTracker.Tests.csproj` | Passed, exit 0 |
| `dotnet build SeriesTracker.Tests/SeriesTracker.Tests.csproj --configuration Release --no-restore --nologo` | Passed, exit 0 |
| `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --configuration Release --no-build --nologo` | Passed, exit 0; 50 succeeded, 0 failed, 0 skipped |
| `dotnet format --verify-no-changes --no-restore` | Passed, exit 0; no source changes |
| `openspec validate --specs --strict` | Passed, exit 0; 1 spec valid |
| `openspec validate fix-ci-and-require-local-quality-checks --strict` | Passed, exit 0 |

Additional checks passed: README command parity with all five CI checks; OpenSpec context loading and CLI exposure of task rules and apply/archive guidance; editor diagnostics; newline-only source diff review; and `git diff --check`. All delta scenarios were reviewed against README and project guidance. No local blockers remain.

Before archive, review this evidence again and rerun the complete sequence if implementation, workflow, or specification state changed. Validate resulting main specs after synchronization. After publication, inspect the next Application Quality run and report its observed result separately; no commit, push, or remote rerun was performed here.

Archive verification on 2026-10-06: synced the quality-gate requirement and all eight scenarios into the main `series-tracker` specification, preserving all existing content. Re-ran all six commands listed above against this synced state in the required order; each exited 0, with 50 tests passed and none failed or skipped. Strict main-spec validation passed with a non-blocking informational note about requirement length. Exact delta comparison confirmed nothing remains to sync, and `git diff --check` passed. Only this evidence annotation followed the verification run; remote CI remains unverified.