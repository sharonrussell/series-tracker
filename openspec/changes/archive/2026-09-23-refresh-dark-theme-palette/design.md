# Design

## Context

Dark mode is currently token-driven in `wwwroot/css/site.css`, using green-charcoal surfaces with coral accents. The theme toggle, persistence, responsive layout, and row-state presentation are already implemented; only the dark token values need to change.

## Goals / Non-Goals

**Goals:**
- Establish a coherent midnight-blue dark palette with muted blue accents.
- Keep text, controls, focus states, and status rows readable with comfortable contrast.
- Preserve the existing light palette and all theme behavior.

**Non-Goals:**
- No markup, JavaScript, data-model, or interaction changes.
- No new fonts, external UI libraries, gradients, or decorative background effects.
- No change to reading-status semantics or list ordering.

## Decisions

- Update only the dark theme custom properties so all existing shell, drawer, table, and control styles inherit the new palette consistently.
- Use deep ink-blue backgrounds, blue-gray surfaces and borders, soft blue-white text, and a muted periwinkle accent. This creates a deliberate dark counterpart to the light theme without using saturated navy.
- Retain distinct status-row hues: subdued amber for reading, deep teal for completed, and dusty berry for dropped. These preserve scanability without turning status colors into dominant accents.
- Validate the theme in desktop and mobile views, including the persisted toggle, editor drawer, scrollable list, and browser console.

## Risks / Trade-offs

- [Risk] Blue surfaces can feel cold or overly saturated.
  -> Mitigation: use desaturated ink and slate-blue values rather than bright navy.
- [Risk] Softer dark colors can reduce contrast for notes and borders.
  -> Mitigation: inspect all semantic text and interactive states in the browser before completion.
- [Risk] Token changes can expose color overrides outside the main dashboard.
  -> Mitigation: smoke test the shell, list, drawer, and row states in dark mode.

## Migration Plan

- Replace the existing dark-mode token values with the approved midnight-blue palette.
- Refresh the running app and smoke test dark mode across desktop and mobile viewports.
- Rollback restores the previous dark token block; no data migration is required.
