# Design

## Context

The repository is currently a minimal OpenSpec scaffold with no application code yet. The goal is to create a simple, low-maintenance personal series tracker built with a .NET-first stack and a deliberately small front end.

## Goals / Non-Goals

**Goals:**
- Provide a single dashboard listing all tracked series
- Allow quick progress updates without complex workflows
- Support lifecycle states for reading, completed, and archived series
- Keep the implementation approachable for a .NET developer with limited front-end experience

**Non-Goals:**
- Multi-user accounts or authentication
- Cloud sync or remote storage
- Social features, reviews, or recommendation feeds
- Complex analytics or AI-based recommendations

## Decisions

### Decision: Use ASP.NET Core with Razor Pages and SQLite
The project will use a simple server-rendered ASP.NET Core app with Razor Pages for UI and SQLite for local persistence. This reduces front-end complexity while keeping data access straightforward for a .NET developer. It also avoids requiring external infrastructure for a first version.

**Alternatives considered:**
- React or full SPA: more UI complexity than needed for the MVP
- SQL Server or Postgres: unnecessary operational overhead for local personal tracking
- Console app or file-only storage: less user-friendly and harder to maintain as the feature set grows

### Decision: Treat progress as a generic current/total value
Each series will store a current progress integer and a total progress integer to support episode, volume, or chapter tracking in a single, flexible model. This keeps the app simple and allows future enhancements without redesigning the domain model.

**Alternatives considered:**
- Hard-coded episode-only tracking: too restrictive for anime, manga, comics, and other series types
- Separate content-type models: adds complexity early without clear user value

### Decision: Use simple status lifecycle states
The app will use a small status model: active, completed, dropped, and archived. Archive is treated as a persisted end state for any series that is no longer being tracked actively.

**Alternatives considered:**
- Freeform tags only: harder to reason about and less consistent in UI
- Separate tables for each lifecycle: over-engineers the MVP

## Risks / Trade-offs

- [Data model simplicity] → Mitigation: keep progress generic and store status explicitly to avoid overfitting to a single media type
- [Limited UI polish] → Mitigation: prioritize clarity and fast interactions over visual complexity
- [Local-only storage] → Mitigation: document that the first version is single-user and local to one machine

## Migration Plan

- Create the initial ASP.NET Core project structure
- Add the data model for series records and status metadata
- Configure SQLite and run initial database migrations
- Build the dashboard, add/edit flow, and progress update actions
- Validate the app works end-to-end with a small set of sample series entries

## Open Questions

- None identified for the MVP scope. The design resolves the main product decisions by using a generic progress model and a simple status lifecycle.
