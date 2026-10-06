# Spec Delta

## ADDED Requirements

### Requirement: Local application quality checks gate change completion

The project development workflow SHALL require every check enforced by
Application Quality to run and pass locally against the final implementation
state before an implemented OpenSpec change is declared complete or archived.
The checks SHALL include dependency restore, Release build, the complete test
suite, formatting verification without source changes, and strict main-spec
validation, with the same targets, options, and ordering as CI. Strict
validation of the active change SHALL also pass before completion. This gate
SHALL apply to every implemented change, including specification-only changes,
and SHALL supplement existing change-specific verification requirements.

#### Scenario: Complete a verified change

- **WHEN** an implemented change is ready for completion
- **THEN** every CI-equivalent check and strict active-change validation has
  passed locally against its final implementation state, with the commands and
  outcomes recorded in the change's verification summary

#### Scenario: Formatting failure blocks completion

- **WHEN** local formatting verification finds a missing final newline or
  another formatting violation
- **THEN** the change remains incomplete until the violation is corrected and
  the required checks pass without disabling formatting enforcement

#### Scenario: An unrun check blocks completion

- **WHEN** any required check is skipped, unavailable, or lacks a recorded local
  result
- **THEN** the missing check is reported as a blocker and the change is not
  declared complete or archived

#### Scenario: Any failing check blocks completion

- **WHEN** any required local check exits unsuccessfully
- **THEN** its failure is recorded and completion and archive remain blocked
  until the defect is resolved and the required verification succeeds

#### Scenario: Edits invalidate earlier verification

- **WHEN** implementation, workflow, or specification changes are made after a
  successful local verification pass
- **THEN** the required checks are rerun against the updated final state before
  completion or archive

#### Scenario: CI checks evolve

- **WHEN** Application Quality adds or changes a check
- **THEN** the local completion checklist is updated to match and completion
  requires a passing local result for the updated check

#### Scenario: Planning readiness is not implementation completion

- **WHEN** proposal, design, specification, and task artifacts exist but the
  change has not been implemented and locally verified
- **THEN** the change may be described as ready for implementation but not as an
  implemented, verified, or archive-ready change

#### Scenario: Verify a specification-only change

- **WHEN** an implemented change updates only specifications or development
  guidance
- **THEN** all CI-equivalent local checks and strict active-change validation
  still pass before it is declared complete or archived
