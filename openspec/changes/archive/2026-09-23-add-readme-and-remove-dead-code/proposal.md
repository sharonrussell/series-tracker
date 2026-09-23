# Proposal

## Why

The repository has no root README, making local setup and the tracker workflow harder to discover. It also retains legacy dashboard handlers, tests, and styling that no longer support the shared-editor workflow, which increases maintenance cost and obscures the current code path.

## What Changes

- Add a root README covering the app purpose, prerequisites, local run/test commands, core workflow, persistence, and project structure.
- Audit and remove code and CSS that are proven unused by the current shared-editor dashboard workflow.
- Remove tests that exist solely for retired handlers and retain coverage for the supported save, delete, filtering, and derived-status behavior.
- Preserve externally observable application behavior and validate the cleanup with build, tests, and browser smoke checks.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

None. This change documents the existing application and removes unused implementation paths without changing the durable product contract.

## Impact

- Affected root documentation, legacy PageModel handlers/tests, and unused CSS selectors.
- No data model, persistence, route, API, or user-facing behavior changes are intended.
- No external dependencies are added.
