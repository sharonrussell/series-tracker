# Design

## Context

The repository currently has one GitHub Actions workflow that only installs and verifies OpenSpec for Copilot setup. Local development uses .NET 10 and a standalone test project, but there is no repository-wide formatting or analyzer policy and no workflow that validates application changes.

## Goals / Non-Goals

**Goals:**
- Enforce build, test, formatting, analyzer, and OpenSpec validation for normal application changes.
- Establish an explicit, low-churn .NET style and analyzer baseline.
- Keep the Copilot setup workflow narrowly focused on its existing responsibility.

**Non-Goals:**
- No runtime behavior, data, UI, or dependency changes.
- No coverage threshold gate or browser automation in CI in this change.
- No immediate enforcement of every optional analyzer suggestion.

## Decisions

- Add a separate application quality workflow triggered on pushes and pull requests that affect application code, tests, project configuration, OpenSpec files, or the workflow itself.
- Use the .NET 10 SDK in CI and run restore, build, the existing test project, `dotnet format --verify-no-changes`, and strict OpenSpec validation.
- Add `.editorconfig` and project analyzer settings that enable .NET analyzers and make compiler warnings errors, while keeping nonessential analyzer rules at warning or suggestion severity until the codebase adopts them deliberately.
- Keep existing Copilot setup validation unchanged rather than coupling application quality checks to an agent-support workflow.

## Risks / Trade-offs

- [Risk] Analyzer or formatter enforcement may reveal baseline violations.
  -> Mitigation: establish the configuration and resolve the initial baseline in the same change before enabling CI enforcement.
- [Risk] CI can run unnecessarily on documentation-only updates.
  -> Mitigation: use path filters that include quality-relevant files and exclude unrelated repository content.
- [Risk] Tooling behavior can differ across developer machines.
  -> Mitigation: pin the CI SDK major version and document the same commands in the README.

## Migration Plan

- Add the editor/analyzer policy and resolve resulting baseline diagnostics.
- Add the application quality workflow and verify it locally through the same commands.
- Preserve the existing Copilot setup workflow.
- Rollback removes the new workflow and policy files; no runtime migration is required.
