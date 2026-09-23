# Tasks

## 1. Shared editor foundation

- [x] 1.1 Define the shared create/edit form model and validation contract, preserving existing add/edit persistence rules; verify existing handler regression tests still pass.
- [x] 1.2 Replace the inline add row and separate edit form markup with one shared editor surface that groups Series details, Progress, and Tracking fields; verify create mode hides reading status and defaults it to Reading while edit mode shows the current status.

## 2. Drawer interaction and routing

- [x] 2.1 Add dashboard drawer open/close behavior for Add and row activation, including backdrop, close button, Escape handling, focus management, and accessible labels; verify keyboard and mouse navigation.
- [x] 2.2 Preserve direct `/Series/Edit/{id}` links by resolving them to the dashboard with the shared editor open for the requested series; verify deep-link navigation and browser-back/cancel behavior.

## 3. Responsive presentation

- [x] 3.1 Style the desktop editor as a right-side drawer with a stable footer and style small viewports as a full-screen sheet without horizontal scrolling; verify at desktop and mobile viewport sizes.
- [x] 3.2 Show validation errors inside the shared editor without losing entered values; verify invalid title, author, series length, released count, and books-read submissions.

## 4. Validation

- [x] 4.1 Add or update unit and page-model regression coverage for create/edit mode differences, shared validation, and persistence; verify `dotnet test` and `dotnet build` pass.
- [x] 4.2 Refresh the running UI and smoke test create, edit, cancel, direct-link, keyboard, desktop, and mobile flows; verify the dashboard remains usable and the browser console is clean.
