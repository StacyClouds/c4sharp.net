# NuGet packages for developers

This documentation set is organised around the published NuGet packages.

## Package overview

| Package | Depends on | Use it for | Guide |
|---|---|---|---|
| `StacyClouds.C4Sharp.Core` | - | Creating workspaces, models, views, styles, and documentation objects in code | [Core package guide](getting-started.md) |
| `StacyClouds.C4Sharp.Client` | `StacyClouds.C4Sharp.Core` | Reading and writing workspaces through the Structurizr-compatible API | [Client package guide](api-client.md) |
| `StacyClouds.C4Sharp.Renderer` | `StacyClouds.C4Sharp.Core` | Rendering workspace views to standalone SVG documents | [Renderer package guide](svg-rendering.md) |
| `StacyClouds.C4Sharp.Editor` | `StacyClouds.C4Sharp.Renderer` | Embedding an interactive Blazor editor for workspace layouts | [Editor package guide](razor-svg-editor.md) |

## Install packages

```bash
dotnet add package StacyClouds.C4Sharp.Core
dotnet add package StacyClouds.C4Sharp.Client
dotnet add package StacyClouds.C4Sharp.Renderer
dotnet add package StacyClouds.C4Sharp.Editor
```

Install only the packages you need:

- Start with `Core` when your application creates or transforms workspaces.
- Add `Client` when you need to download or publish workspaces.
- Add `Renderer` when you need SVG output in a console app, service, or pipeline.
- Add `Editor` when a Blazor host needs interactive layout editing in the browser.

## Common package combinations

- `Core` only: model a workspace and hand it to another process.
- `Core` + `Client`: build a workspace in code and publish it.
- `Core` + `Renderer`: generate SVG artifacts locally.
- `Core` + `Renderer` + `Editor`: render and edit layouts inside a Blazor app.
- `Core` + `Client` + `Renderer`: generate diagrams locally and publish the same workspace remotely.

## Source locations

- Core: `/home/runner/work/c4sharp.net/c4sharp.net/StacyClouds.C4Sharp.Core`
- Client: `/home/runner/work/c4sharp.net/c4sharp.net/StacyClouds.C4Sharp.Client`
- Renderer: `/home/runner/work/c4sharp.net/c4sharp.net/StacyClouds.C4Sharp.Renderer`
- Editor: `/home/runner/work/c4sharp.net/c4sharp.net/StacyClouds.C4Sharp.Editor`
