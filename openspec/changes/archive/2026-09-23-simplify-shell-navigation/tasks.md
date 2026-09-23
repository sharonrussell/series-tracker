# Tasks

## 1. Shell cleanup

- [x] 1.1 Update shared layout display text from `Series_Tracker` to `Series Tracker`, remove the redundant Dashboard link and duplicate visible page title, and verify the brand link still opens the tracker root.
- [x] 1.2 Remove Privacy navigation/footer links and delete the scaffold Privacy page files; verify no visible Privacy links remain in the shell.

## 2. Validation

- [x] 2.1 Run `dotnet test` and `dotnet build` to verify the shell cleanup does not break the app.
- [x] 2.2 Refresh the running UI and smoke test the brand link, theme toggle, simplified shell, and absence of scaffold Privacy content.
