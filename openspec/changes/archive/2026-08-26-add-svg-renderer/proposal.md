## Why

C4Sharp.NET can define and persist architecture views, but consumers must use
another tool to see them as diagrams. A built-in SVG renderer will make every
workspace view portable and inspectable while preserving the layout already
stored in the model. The existing relationship layout data also needs a public
editing API so callers can deliberately route connectors around diagram
elements.

## What Changes

- Add a `StacyClouds.C4Sharp.Renderer` class library that produces an SVG for
  every supported view in a workspace.
- Render persisted element coordinates, relationship routing vertices, labels,
  titles, dimensions, and view styles as SVG.
- Add public Core APIs to create and edit ordered relationship connector
  vertices without relying on internal serialization members.
- Add renderer and Core tests, an example, and user documentation.

## Capabilities

### New Capabilities

- `svg-workspace-rendering`: Render each supported workspace view as a
  standalone SVG that reflects its model and persisted layout.
- `relationship-layout-editing`: Publicly create and edit relationship
  connector vertices while preserving layout serialization compatibility.

### Modified Capabilities

- None.

## Impact

- Adds a multi-targeted renderer project, a corresponding test project, and
  references in the maintained solution.
- Adds public APIs to the Core relationship-layout model; the existing
  serialized workspace format remains compatible.
- Adds documentation and examples for SVG output and connector editing.
