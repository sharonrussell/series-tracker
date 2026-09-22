# Design

## Context

See `proposal.md` for motivation. The dashboard action row currently renders repeated controls as Bootstrap-style buttons. The add control already uses a symbol-style plus button with accessible label/title attributes.

## Goals / Non-Goals

**Goals:**

- Convert row edit, complete, and drop actions to compact symbol-only controls.
- Keep `+1` compact and aligned with the symbol controls.
- Preserve accessible names through `aria-label`, `title`, or equivalent markup.
- Preserve existing form/link destinations and page handler behavior.

**Non-Goals:**

- Do not change status, progress, edit, complete, or drop behavior.
- Do not add a frontend icon library or external dependency.
- Do not redesign the whole table or dashboard layout.

## Decisions

- Use simple text symbols for the controls in this change.
  - Rationale: The app has no icon dependency today, and the requested symbols can be represented without adding packages.
  - Alternative considered: add an icon library. Rejected because this small Razor Pages app does not currently need the dependency or build changes.

- Add a reusable compact action-button style.
  - Rationale: Symbol controls should have stable dimensions so the action column remains aligned and easy to scan.
  - Alternative considered: reuse existing text button styles only. Rejected because symbol-only buttons need explicit sizing to avoid inconsistent row layout.

- Keep visible symbol text and add accessible labels.
  - Rationale: Symbol-only controls need nonvisual names so keyboard and assistive technology users can understand each action.
  - Alternative considered: visible text plus symbols. Rejected because the request specifically asks for symbols instead of text labels.

## Risks / Trade-offs

- Symbol meaning may be ambiguous -> Use recognizable symbols and add accessible labels/titles.
- Browser rendering of emoji-style symbols can vary -> Prefer plain symbols that render broadly, or use HTML entities if needed.
- Compact controls can become hard to tap -> Set fixed dimensions and sufficient spacing.

## Migration Plan

1. Update dashboard row actions to use symbol content for edit, complete, and drop.
2. Add accessible labels/titles to edit, progress, complete, and drop controls.
3. Add or update compact action button CSS.
4. Run `dotnet test --nologo` and `dotnet build --nologo`.
5. Smoke test dashboard HTML to verify `Edit` and `Drop` are no longer visible button text while accessible labels remain.
