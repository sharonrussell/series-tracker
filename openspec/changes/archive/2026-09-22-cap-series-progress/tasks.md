# Tasks

## 1. Progress capping behavior

- [x] 1.1 Update dashboard progress increment logic so incrementing below the series length increases books read and verify with a handler test
- [x] 1.2 Ensure incrementing at or beyond the series length keeps books read capped at the series length, marks reading status completed, and verify with a boundary test
- [x] 1.3 Mark reading status completed when add or edit saves progress equal to series length and verify with handler tests
- [x] 1.4 Verify marking a series completed still sets books read to series length without changing publication completion state

## 2. Regression validation

- [x] 2.1 Run `dotnet test --nologo` and verify all tests pass
- [x] 2.2 Run `dotnet build --nologo` and verify it completes with no warnings
- [x] 2.3 Smoke test the dashboard `+1` action at the cap and verify the displayed percentage does not exceed 100% and status is completed
