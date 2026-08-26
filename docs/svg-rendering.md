# SVG rendering

`StacyClouds.C4Sharp.Renderer` renders every view in a workspace as a
standalone SVG document. The returned dictionary is keyed by the view key.

```csharp
IReadOnlyDictionary<string, string> diagrams = new SvgWorkspaceRenderer().Render(workspace);
File.WriteAllText("system-context.svg", diagrams["system-context"]);
```

The renderer uses persisted element coordinates and relationship connector
vertices. When a new view has no layout, it uses a deterministic in-memory
grid without changing the workspace. Use `RelationshipView.AddVertex`,
`SetVertices`, `RemoveVertex`, and `ClearVertices` to edit connector bends.

`SvgRenderingExample.WriteSvgDocuments(outputDirectory)` writes the example
workspace's SVG documents to a directory.
