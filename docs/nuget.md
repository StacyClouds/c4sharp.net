# NuGet packages for developers

This page is the package map for developers building diagram-as-code solutions with C4Sharp.NET.

## Package overview

| Package | Purpose | Typical usage |
|---|---|---|
| `StacyClouds.C4Sharp.Core` | C4 model and view primitives | Build workspaces, systems, containers, components, and views |
| `StacyClouds.C4Sharp.Client` | Structurizr API client and encryption support | Push/pull workspaces to Structurizr cloud/on-prem |
| `StacyClouds.C4Sharp.Renderer` | SVG rendering support | Render diagrams to SVG in build pipelines or apps |
| `StacyClouds.C4Sharp.Editor` | Editor-focused helpers and integration surface | Support editor-style experiences around workspaces |

## Install packages

```bash
dotnet add package StacyClouds.C4Sharp.Core
dotnet add package StacyClouds.C4Sharp.Client
dotnet add package StacyClouds.C4Sharp.Renderer
dotnet add package StacyClouds.C4Sharp.Editor
```

Install only what you need. Most projects start with `StacyClouds.C4Sharp.Core`, then add `Client`, `Renderer`, or `Editor` based on delivery needs.

## Package source locations

- Core: `/StacyClouds.C4Sharp.Core`
- Client: `/StacyClouds.C4Sharp.Client`
- Renderer: `/StacyClouds.C4Sharp.Renderer`
- Editor: `/StacyClouds.C4Sharp.Editor`

## Building packages locally

To create local multi-target NuGet packages:

1. Open a terminal in the repository root.
2. Restore the maintained solution:
   ```bash
   dotnet restore StacyClouds.C4Sharp.slnx
   ```
3. Build the maintained solution:
   ```bash
   dotnet build StacyClouds.C4Sharp.slnx
   ```
4. Pack the project you want to publish:
   ```bash
   dotnet pack StacyClouds.C4Sharp.Core/StacyClouds.C4Sharp.Core.csproj -c Release
   ```

Repeat step 4 for `Client`, `Renderer`, and `Editor` when required.