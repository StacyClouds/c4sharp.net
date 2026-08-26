# Razor SVG editor

Install `StacyClouds.C4Sharp.Editor` to add the optional interactive editor to a Blazor application. This package depends on the renderer; applications that only render SVG do not need to reference it.

Add the package static assets to the host page:

```html
<link rel="stylesheet" href="_content/StacyClouds.C4Sharp.Editor/c4sharp-editor.css" />
<script src="_content/StacyClouds.C4Sharp.Editor/c4sharp-editor.js"></script>
```

Use `WorkspaceEditor` for a collapsible thumbnail navigator and a selected interactive view. Use `ViewEditor` when the host already manages view navigation. Both components require an interactive Blazor render mode for drag and double-click editing.

```razor
<WorkspaceEditor Workspace="workspace"
                 InitialViewKey="system-context"
                 @bind-SelectedViewKey="selectedViewKey"
                 LayoutChanged="OnLayoutChanged"
                 SaveRequested="SaveWorkspace" />
```

Dragging updates the existing `ElementView.X` and `ElementView.Y` values. Double-clicking an arrow inserts an ordered `Vertex` into its `RelationshipView`. `LayoutChanged` is raised after either operation. `SaveRequested` is raised only when the user selects Save: the component does not write files or call the Structurizr API, so the host decides how to persist the updated workspace.

`SelectedViewKeyChanged` is raised when the user selects a thumbnail. Bind `SelectedViewKey` to let the host observe and control which view is loaded in the editor pane.
