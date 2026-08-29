---
title: Renderer package guide
---

# Renderer package guide

Install `StacyClouds.C4Sharp.Renderer` when you need standalone SVG documents for one or more workspace views.

```bash
dotnet add package StacyClouds.C4Sharp.Renderer
```

## Namespace

```csharp
using StacyClouds.C4Sharp.Renderer;
```

## Render a workspace

`SvgWorkspaceRenderer` returns a dictionary keyed by view key.

```csharp
IReadOnlyDictionary<string, string> diagrams = new SvgWorkspaceRenderer().Render(workspace);
File.WriteAllText("system-context.svg", diagrams["system-context"]);
```

This overload does not mutate `workspace`. It uses persisted element coordinates and relationship vertices when they exist, and falls back to a deterministic in-memory layout when they do not.

## Reuse layout from an earlier workspace

```csharp
IReadOnlyDictionary<string, string> diagrams = new SvgWorkspaceRenderer().Render(successor, predecessor);
SaveWorkspace(successor);
```

The two-workspace overload copies matching view geometry, element positions, connector vertices, routing, and label positions from `predecessor` into `successor` before rendering.

## Related topics

- `StacyClouds.C4Sharp.Editor` builds on this package for interactive browser editing.
- `StacyClouds.C4Sharp.Examples/SvgRenderingExample.cs` shows a complete rendering example.
