# Tasks

## 1. Quality baseline

- [x] 1.1 Add repository-level .NET formatting and analyzer configuration with a clean baseline; verify `dotnet format --verify-no-changes` and `dotnet build --nologo` succeed locally.
- [x] 1.2 Update README quality guidance with the enforced local commands; verify each documented command matches the repository configuration.

## 2. Continuous integration

- [x] 2.1 Add a dedicated application quality workflow for push and pull request changes that installs .NET 10, restores, builds, tests, verifies formatting, and validates OpenSpec specs; verify the existing Copilot setup workflow remains unchanged.
- [x] 2.2 Configure workflow path filters so application code, tests, project configuration, OpenSpec content, and the workflow itself trigger quality validation; verify workflow YAML syntax and command paths.

## 3. Validation

- [x] 3.1 Run the enforced local build, test, format, and OpenSpec validation commands and verify they all pass.
