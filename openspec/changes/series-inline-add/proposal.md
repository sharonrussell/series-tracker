# Proposal

## Why

The current series tracker requires a separate add form, which makes the dashboard feel heavier than needed for a simple personal reading list. A lightweight inline add flow keeps the user in context and reduces the friction of recording a new series while they are already scanning the list.

## What Changes

- Add an Add button above the list of series in the dashboard.
- When the button is clicked, insert a new editable row into the list with fields for title, series length, and books read so far.
- Allow the user to save the draft entry or cancel it without leaving the current page.
- Keep the existing task flow, status tracking, archive actions, and filtering behavior intact.

## Capabilities

### New Capabilities
- None

### Modified Capabilities
- `series-tracker`: Update the existing series management workflow so a user can create a new series entry inline from the dashboard instead of using a separate add form.

## Impact

- Razor Pages dashboard and form handling in `Pages/Index.cshtml` and `Pages/Index.cshtml.cs`
- Existing series data model and validation in `Models/SeriesItem.cs`
- Database persistence and progress tracking remain supported through the same model and EF Core context
