# Tasks

## 1. Documentation

- [x] 1.1 Add a root `README.md` covering purpose, prerequisites, local run/test commands, persistence, supported dashboard workflow, and project structure; verify the documented commands match project and launch configuration.

## 2. Dead-code cleanup

- [x] 2.1 Audit legacy handlers, compatibility routes, tests, and CSS selectors for production references; remove only items proven unused and remove tests that exclusively exercise retired handlers; verify no supported dashboard or direct-edit behavior is removed.
- [x] 2.2 Remove confirmed unused CSS selectors while preserving active dashboard, editor, theme, and Bootstrap override styles; verify stylesheet references are absent from Razor markup and JavaScript.

## 3. Validation

- [x] 3.1 Run `dotnet build --nologo`, `dotnet test SeriesTracker.Tests/SeriesTracker.Tests.csproj --nologo`, `openspec validate add-readme-and-remove-dead-code --strict`, and a diff whitespace check.
- [x] 3.2 Refresh the running UI and smoke test dashboard loading, filter, Add/Edit drawer, theme toggle, list scrolling, direct edit redirect, and browser-console cleanliness.
