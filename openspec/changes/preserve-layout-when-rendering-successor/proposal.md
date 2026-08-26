## Why

Rendering an updated workspace currently cannot reuse edits saved with its prior
version. Applications need a way to carry forward compatible diagram layout
while keeping the existing renderer's non-mutating behavior available.

## What Changes

- Add an additive predecessor-aware SVG renderer overload.
- Copy reusable layout from a predecessor into the updated successor before
  rendering it.
- Document the distinct mutation contracts of the two overloads.

## Scope

`workspace` is the updated, persistable successor. `predecessor` is read-only
layout input. Matching views, elements, and relationships reuse their saved
layout; objects absent from the successor are never restored.

## Compatibility

`Render(Workspace)` remains non-mutating. The new overload is additive and
intentionally mutates only its successor argument.

## Affected Capabilities

- `svg-workspace-rendering`
