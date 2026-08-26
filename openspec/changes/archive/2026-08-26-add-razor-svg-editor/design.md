## Context

`StacyClouds.C4Sharp.Renderer` is a multi-targeted .NET library that produces standalone SVG documents from workspace views. The existing web project only embeds those documents in an iframe. Users need an interactive editor, but adding Razor dependencies to the renderer would make a lightweight library harder to consume and may be incompatible with non-web solutions.

## Goals / Non-Goals

**Goals:**

- Publish a separate multi-targeted Razor class-library NuGet package.
- Render a selected workspace view as the renderer's SVG output.
- Persist element drag positions and ordered relationship vertices directly on existing view objects.
- Keep the renderer usable without ASP.NET Core, Blazor, or editor static assets.

**Non-Goals:**

- Automatic layout, undo/redo, editing model element metadata, or creating/removing relationships.
- Persisting a workspace to disk or a remote Structurizr service.
- Supporting arbitrary third-party SVG documents.

## Decisions

### Separate Razor class library package

Create `StacyClouds.C4Sharp.Editor` using `Microsoft.NET.Sdk.Razor`, package metadata consistent with the existing libraries, and a project reference to the renderer. The renderer remains a pure .NET dependency. A Razor class library is selected over extending the web project so applications can reuse the component in their own Blazor hosting model.

### Additive SVG interaction identifiers

The renderer will emit data attributes for the workspace view key, element ID, and relationship ID. This keeps SVG rendering authoritative and lets the optional package locate exact workspace objects. Recreating SVG in the editor was rejected because it would duplicate style and layout logic.

### Composed workspace and view editor components

Expose `WorkspaceEditor` as the primary component. It accepts a workspace, renders a collapsible side panel containing scaled SVG thumbnails for all view keys, and loads the selected view into an interactive editing surface. Retain a focused `ViewEditor` component for hosts that intentionally want to embed a single view without navigation chrome. The workspace component uses the same renderer output for the navigator and active surface.

### JavaScript pointer handling with .NET workspace mutation

The Razor component will render the supplied workspace SVG and use a small scoped static asset for SVG coordinate conversion and pointer events. JavaScript sends element IDs/coordinates and relationship IDs/double-click coordinates to the component; the component mutates `ElementView.X/Y` or calls `RelationshipView.SetVertices`. It raises `LayoutChanged` after each mutation and `SaveRequested` only when the user invokes Save. This preserves the user-owned workspace instance and makes saving an application concern.

### Vertex insertion is segment-aware

On a relationship double-click, the component finds the closest segment in the source → existing vertices → destination polyline and inserts the new `Vertex` at that index. Appending was rejected because it could alter an already routed connector's visual path.

## Risks / Trade-offs

- [Interactive behavior needs JavaScript] → Ship minimal namespaced static assets and document the required script/style references.
- [An editor needs a connected Blazor render mode] → Document that the component requires an interactive render mode; displaying the SVG still works in static output.
- [Workspaces can contain many views] → Use scaled SVG documents for thumbnails and render only the selected full-size editor surface.
- [Rendered SVG identifiers become public markup] → Use additive `data-c4-*` attributes with escaped IDs and retain existing SVG shapes/labels.
- [Viewer and editor packages may target different frameworks] → Multi-target the Razor package to the same supported frameworks as the renderer and validate every target.
