# SVG rendering

`StacyClouds.C4Sharp.Renderer` renders every view in a workspace as a
standalone SVG document. The returned dictionary is keyed by the view key.

```csharp
IReadOnlyDictionary<string, string> diagrams = new SvgWorkspaceRenderer().Render(workspace);
File.WriteAllText("system-context.svg", diagrams["system-context"]);
```

This overload does not mutate `workspace`. The renderer uses persisted element
coordinates and relationship connector vertices. When a new view has no
layout, it uses a deterministic in-memory grid without changing the workspace.

Container-view SVGs draw a lower-left-labeled boundary around the viewed
software system's visible containers. Component-view SVGs do the same for the
viewed container's visible components. People, software systems, and other
containers or components keep their own placement outside that scope boundary.

To reuse saved layout from an earlier workspace version, render the updated
successor against its predecessor:

```csharp
IReadOnlyDictionary<string, string> diagrams = new SvgWorkspaceRenderer().Render(successor, predecessor);
SaveWorkspace(successor); // persist copied layout for matching views and objects
```

The two-workspace overload intentionally mutates only `successor`: matching
view geometry, element coordinates, relationship connector vertices, routing,
and label positions are copied from `predecessor` before rendering. New
successor-only objects still use the deterministic in-memory grid, while
predecessor-only objects are not restored. Use `RelationshipView.AddVertex`,
`SetVertices`, `RemoveVertex`, and `ClearVertices` to edit connector bends.

`SvgRenderingExample.WriteSvgDocuments(outputDirectory)` writes the example
workspace's SVG documents to a directory.
