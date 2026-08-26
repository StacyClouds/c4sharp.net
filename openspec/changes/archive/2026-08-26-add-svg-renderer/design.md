## Context

Core workspaces already retain view-level geometry: element coordinates,
relationship routing vertices, routing mode, label position, dimensions, and
styles. There is no local diagram renderer, and the internal constructors and
setters for relationship vertices prevent normal library consumers from editing
connector geometry. The renderer must target .NET 8 through 11 and must not
alter the workspace while rendering it.

## Goals / Non-Goals

**Goals:**

- Provide a standalone SVG for every view held by a workspace, including
  filtered views.
- Use persisted layout as the source of truth so rendered output matches an
  edited workspace.
- Make relationship connector vertices publicly editable through deliberate,
  safe Core APIs.
- Produce deterministic, XML-safe SVG without requiring a running Structurizr
  service or a browser.

**Non-Goals:**

- Implement a full automatic-layout engine or interactive diagram editor.
- Rasterize SVG output or render to PDF/PNG.
- Replicate every visual detail of the Structurizr web UI in the first release.
- Change the serialized workspace schema.

## Decisions

### Separate renderer library

Add a multi-targeted `StacyClouds.C4Sharp.Renderer` library that references
Core, plus a renderer test project. The public entry point will render a
workspace to a collection keyed by view key, with each value containing a
complete SVG document. Keeping this separate avoids adding rendering concerns
or dependencies to the model library and gives applications control over file
storage.

An alternative was to add SVG methods to `Workspace` or `View`. That would
couple the domain model to a specific output format and make future renderers
harder to add.

### Render all workspace views through a normalized render model

The renderer will collect static, dynamic, deployment, and filtered views in
a stable order. It will normalize each source into renderable elements and
relationships before producing SVG. A filtered view will use its base view's
geometry and include/exclude its elements by tag according to its filter mode;
relationships with an omitted endpoint will not be rendered.

This avoids duplicating SVG generation across view classes. It also makes the
view key the stable output identifier. Duplicate view keys are already
prevented by `ViewSet`.

### Preserve explicit geometry; use a deterministic fallback

Element coordinates and relationship vertices will be rendered exactly as
persisted. If a view has no meaningful element layout, the renderer will place
elements in a documented deterministic grid so that newly-created workspaces
still yield legible SVG. The fallback must not mutate the source workspace.
View dimensions take precedence for the SVG viewport; otherwise the renderer
will derive a padded viewport from the rendered bounds.

An automatic graph layout was considered, but it would introduce substantial
complexity and could diverge from an editor's intended layout.

### Relationship vertex editing API

Promote useful `Vertex` construction to the public API and add explicit
methods on `RelationshipView` to add, replace, remove, and clear the ordered
vertex list. Collection inputs and returned collections will be copied so that
callers cannot accidentally mutate internal state through aliases. Existing
JSON/data-contract member names and their hydration behavior remain unchanged.

Making the `Vertices` setter public was rejected because it would expose the
internal list contract without an operation-oriented API or predictable
defensive copying semantics.

### SVG safety and style resolution

Generate SVG with an XML writer or equivalent escaping mechanism. Resolve
element and relationship styles by tags, applying Core's existing style data
and documented renderer defaults where a property is absent. Renderers will
support the shapes represented by the Core shape enumeration, with a clear
fallback for any unsupported future shape. Relationship paths will include
source, ordered vertices, and destination, and will render arrowheads, labels,
and dynamic order metadata.

## Risks / Trade-offs

- [Coordinates default to zero for new elements] → Detect unlaid-out views and
  use a deterministic in-memory fallback grid.
- [Shape fidelity differs from the Structurizr web UI] → Specify stable Core
  style behavior and cover the supported shapes with renderer tests.
- [Filtered-view base view is missing after malformed deserialization] → Fail
  with a clear rendering exception that identifies the filtered view key.
- [Large labels or diagrams exceed the viewport] → Derive bounds with padding
  when dimensions are absent and test labels requiring XML escaping.
- [Public layout APIs could weaken encapsulation] → Use defensive copies and
  validate null vertex arguments and list operations.

## Migration Plan

The renderer is additive. Existing workspaces continue to deserialize because
their serialized element and relationship layout members are unchanged.
Consumers can adopt public vertex editing APIs incrementally; no source
migration is required. If a renderer defect is discovered, applications can
remain on their current external rendering path because Core behavior is
unchanged apart from additive APIs.

## Open Questions

- None for the first implementation; renderer defaults will be documented and
  covered by tests so they can be configured in a later additive change.
