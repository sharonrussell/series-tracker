# Tasks

## 1. Derived status model

- [x] 1.1 Add a single derived-status calculation with precedence Dropped, Not started, Completed, Up to date, then Reading; verify unit tests cover all five states and boundary values.
- [x] 1.2 Recalculate and normalize status after create/edit/progress updates, including reducing books read from a previously completed series; verify page-model persistence tests pass.

## 2. UI and filtering

- [x] 2.1 Remove manual Reading/Completed choices from the editor while preserving Dropped override and automatic restoration; verify create/edit form behavior.
- [x] 2.2 Update dashboard row classes, status filters, progress notes, and the new muted blue-gray Not started styling; verify each state renders with the expected color and label.

## 3. Validation

- [x] 3.1 Add regression coverage for derived transitions, dropped override restoration, filters, and reduced progress; verify `dotnet test` and `dotnet build` pass.
- [x] 3.2 Refresh the running UI and smoke test all five states, progress reduction from completed, editor status controls, filters, desktop/mobile rows, and browser console cleanliness.
