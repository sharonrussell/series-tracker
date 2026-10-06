# Design

## Context

See `proposal.md` for motivation and `specs/series-tracker/spec.md` for the completion contract. Application Quality restores and builds the .NET 10 xUnit project in Release, runs its tests, verifies formatting at the repository root, and validates main specifications. The test project references the application, so its build covers both projects.

GitHub run `37023597772` passed build and all 50 tests before formatting reported FINALNEWLINE for `Data/DatabaseInitializer.cs`, `Models/SeriesTitle.cs`, `Pages/Series/Create.cshtml.cs`, and `Pages/Series/SeriesEditorPageModel.cs`. A local byte inspection confirmed each ends with `}` rather than a newline. `.editorconfig` already requires final newlines. Main-spec validation was skipped after formatting failed.

README currently lists a different local build/test sequence and omits explicit restore. OpenSpec configuration has general testing context but no mandatory all-checks completion gate. Existing spec requirements include UI smoke testing, which this change supplements rather than replaces.

## Goals / Non-Goals

**Goals:**
- Repair the observed failure with formatting-only changes.
- Make local completion evidence reproducible from the existing CI command sequence.
- Place persistent task-generation and completion guidance in project-owned OpenSpec configuration.

**Non-Goals:**
- Change application behavior, database schema, UI, dependencies, formatter rules, or the Copilot setup workflow.
- Introduce a custom OpenSpec schema, Git hooks, a verification service, or an automatic CLI archive guard.
- Broaden the existing root-project formatting target or claim macOS verification is identical to an Ubuntu runner.

## Decisions

### 1. Repair source files, not the formatting gate

Add final newlines to the four diagnosed files, preserving all other content. Immediately rerun the existing formatting verifier. Suppressing FINALNEWLINE or removing formatting from CI would hide the defect and violate the completion contract.

### 2. Use CI as the command baseline

Document and execute these commands from the repository root in this order:

```bash
dotnet restore SeriesTracker.Tests/SeriesTracker.Tests.csproj
dotnet build SeriesTracker.Tests/SeriesTracker.Tests.csproj --configuration Release --no-restore --nologo
dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --configuration Release --no-build --nologo
dotnet format --verify-no-changes --no-restore
openspec validate --specs --strict
```

Then run `openspec validate <change-name> --strict` for the active change. Provide .NET 10 and an installed OpenSpec CLI as local prerequisites; CI provisioning steps are environment setup, not additional application checks. Do not substitute Debug builds or filtered tests. Keep workflow steps unchanged unless verification identifies an additional defect in these exact checks.

Retaining the existing sequence is smaller than adding a shared script for five commands. Future workflow changes must update the documented local sequence and completion checklist in the same change.

### 3. Put durable workflow guidance in project configuration

Update `openspec/config.yaml` with task artifact rules requiring explicit final-verification tasks and supported `operations.apply.guidance` and `operations.archive.guidance` requiring passing results before sign-off. Align its testing context and README with the command baseline. Do not edit generated skill or prompt files, which are reusable workflow definitions rather than project policy.

This is a required development-process gate enforced by the developer or agent following project guidance. OpenSpec artifact existence and advisory operation guidance do not themselves execute commands or mechanically prevent a user from invoking archive. Automatic enforcement would require a separate, explicitly scoped tool change.

### 4. Record evidence and treat omissions as blockers

Use a short verification summary in the active change's existing `tasks.md` to record executed commands, outcomes, and the implementation state tested. Record unavailable tools, skipped checks, and failures as blockers rather than successful completion. Keep the final verification task unchecked until every required command passes. After subsequent implementation, workflow, or spec edits, rerun the sequence and update the summary; evidence-only task annotations do not require an endless re-verification loop.

Planning artifacts being ready is not implementation completion. The checks are mandatory for every implemented change, even spec-only changes, but generating this proposal does not authorize implementation or require a code verification pass now.

## Risks / Trade-offs

- Guidance is not a technical archive lock: require explicit evidence review before archive and never claim the CLI automatically enforces local execution.
- CI and local environments can differ: use the same command targets and options with .NET 10 locally, then inspect the next GitHub run after publication; do not report remote success before it occurs.
- A later check may expose a previously hidden failure: run the full sequence after the newline repair and address only defects necessary for the scoped quality gate.
- Unpinned OpenSpec installation and .NET SDK patches may drift: record tool versions with verification evidence when relevant; dependency pinning is outside this change.

## Migration Plan

1. Add the four final newlines and verify formatting without changing source.
2. Update README and project OpenSpec guidance, retaining existing change-specific checks.
3. Run and record the full local sequence and strict active-change validation before implementation sign-off.
4. After publication, inspect Application Quality without triggering or claiming an unobserved remote result.
5. Before archive, review the evidence and rerun checks if the tested implementation state changed. If spec synchronization changes main specifications, validate the resulting main specs before reporting archive completion.

There is no data migration or deployment change. Rollback can remove the added project guidance and revert only this change's formatting edits; it must not weaken the existing CI or editor rules.