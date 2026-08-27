# Editor package guide

Install `StacyClouds.C4Sharp.Editor` to embed interactive workspace layout editing in a Blazor application.

```bash
dotnet add package StacyClouds.C4Sharp.Editor
```

`StacyClouds.C4Sharp.Editor` depends on the renderer package and expects a `Workspace` instance from the core package.

## Namespace

```razor
@using StacyClouds.C4Sharp.Editor
```

## Add static assets

Reference the packaged stylesheet and script from the host page.

```html
<link rel="stylesheet" href="_content/StacyClouds.C4Sharp.Editor/c4sharp-editor.css" />
<script src="_content/StacyClouds.C4Sharp.Editor/c4sharp-editor.js"></script>
```

## Enable an interactive render mode

The editor components use Blazor interop for dragging elements and editing connector vertices, so the host page must render them with an interactive Blazor render mode.

## Use `WorkspaceEditor`

`WorkspaceEditor` renders a thumbnail navigator plus the selected interactive view.

```razor
<WorkspaceEditor Workspace="workspace"
                 InitialViewKey="system-context"
                 @bind-SelectedViewKey="selectedViewKey"
                 LayoutChanged="OnLayoutChanged"
                 SaveRequested="SaveWorkspace" />
```

## Use `ViewEditor`

Use `ViewEditor` when the host already controls navigation and only needs a single editable view surface.

## Persistence responsibilities

Dragging updates `ElementView.X` and `ElementView.Y`. Connector edits update the underlying `RelationshipView`. `LayoutChanged` reports in-memory changes immediately, and `SaveRequested` lets the host decide how to persist the updated workspace.

See `/home/runner/work/c4sharp.net/c4sharp.net/StacyClouds.C4Sharp.Renderer.Web/Components/Pages/Editor.razor` for a complete host example.
