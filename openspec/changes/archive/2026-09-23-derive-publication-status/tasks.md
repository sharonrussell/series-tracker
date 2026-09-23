# Tasks

## 1. Publication derivation

- [x] 1.1 Add publication-status derivation from released count versus series length and verify unit tests cover ongoing, fully released, and full-progress boundary cases.
- [x] 1.2 Normalize create/edit save paths so books read at series length forces released count to series length, derives publication status as fully released, and then derives reading status; verify page-model persistence tests pass.

## 2. Editor and dashboard behavior

- [x] 2.1 Remove manual publication-state selection from the shared editor and display derived publication status where useful; verify create and edit modes no longer expose the manual selector.
- [x] 2.2 Update derived reading-status logic to depend on derived publication status rather than the manually stored completion flag; verify completed and up-to-date transitions.

## 3. Validation

- [x] 3.1 Add regression coverage for publication derivation, inconsistent stored completion-state values, full-progress normalization, and reduced progress; verify `dotnet test` and `dotnet build` pass.
- [x] 3.2 Refresh the running UI and smoke test create/edit flows, fully released derivation, ongoing derivation, completed reading transition, reduced progress, and browser console cleanliness.
