# Proposal

## Why

A reader needs a lightweight way to keep track of the series they are actively following without managing separate spreadsheets, notes, or bookmarks. The app should make current reading status visible at a glance and make it easy to update progress as new episodes or volumes are consumed.

## What Changes

- Add a personal series-tracking capability for managing a reading list
- Support adding a series, updating its current progress, and viewing all active series together
- Allow a user to mark a series as completed, on hold, or archived when they finish or drop it
- Keep the interface simple enough for a .NET developer to maintain without a complex front-end stack

## Capabilities

### New Capabilities
- `series-tracker`: personal series tracking with progress, status management, and archival lifecycle

### Modified Capabilities
- None

## Impact

- New ASP.NET Core application structure for a lightweight CRUD-style tracker
- SQLite database for local persistence and easy startup without external infrastructure
- Razor Pages or MVC views for server-rendered UI with minimal front-end complexity
- Simple domain model for series records and progress/status transitions
