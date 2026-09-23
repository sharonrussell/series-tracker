# Proposal

## Why

The application builds and tests locally, but those checks are not automatically enforced for normal pull requests or pushes. The repository also lacks a shared formatting and analyzer baseline, allowing quality regressions to reach review without consistent feedback.

## What Changes

- Add a dedicated application CI workflow that runs restore, build, unit tests, and OpenSpec validation for application changes.
- Add repository-level .NET formatting and analyzer configuration with a clean initial baseline.
- Treat selected build-quality issues as enforceable without turning every existing analyzer suggestion into immediate blocking churn.
- Keep the existing Copilot setup workflow independent from application quality checks.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

None. This change enforces existing quality practices without changing product behavior.

## Impact

- Affected GitHub Actions workflows and repository-level .NET editor/analyzer configuration.
- CI will require the .NET 10 SDK and run the existing test project plus OpenSpec spec validation.
- No runtime application, persistence, API, or user-interface changes.
