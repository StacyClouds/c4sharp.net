---
title: API Reference
---

# C4Sharp.NET API Reference

This section contains the full public API reference for all C4Sharp.NET packages,
generated from XML documentation comments in the source code.

## Browse the reference

- [Full API index (Table of Contents)](toc.html)
- [StacyClouds.C4Sharp.Workspace](StacyClouds.C4Sharp.Workspace.html) — top-level entry point
- [StacyClouds.C4Sharp.Model](StacyClouds.C4Sharp.Model.html) — the C4 model root
- [StacyClouds.C4Sharp.Api.StructurizrClient](StacyClouds.C4Sharp.Api.StructurizrClient.html) — publish and fetch workspaces
- [StacyClouds.C4Sharp.Renderer.SvgWorkspaceRenderer](StacyClouds.C4Sharp.Renderer.SvgWorkspaceRenderer.html) — SVG rendering

## Packages covered

| Package | Entry namespace |
|---------|----------------|
| StacyClouds.C4Sharp.Core | `StacyClouds.C4Sharp` |
| StacyClouds.C4Sharp.Client | `StacyClouds.C4Sharp.Api` |
| StacyClouds.C4Sharp.Renderer | `StacyClouds.C4Sharp.Renderer` |
| StacyClouds.C4Sharp.Editor | `StacyClouds.C4Sharp.Editor` |

## How this reference is generated

The API reference is regenerated automatically during each release:

1. The release workflow builds the solution with `GenerateDocumentationFile` enabled.
2. DocFX extracts metadata from the project source (`dotnet docfx metadata`).
3. DocFX builds the HTML pages (`dotnet docfx build`) into `docs/api/`.
4. The output is committed back to the repository and GitHub Pages redeploys.

To regenerate locally:

```bash
dotnet tool restore
dotnet build StacyClouds.C4Sharp.slnx -c Release -p:TargetFramework=net10.0
dotnet docfx metadata docfx.json
dotnet docfx build docfx.json
```

See [CONTRIBUTING.md](https://github.com/StacyClouds/c4sharp.net/blob/main/CONTRIBUTING.md#xml-documentation-and-api-reference) for full instructions.

## XML documentation standards

All public API members carry XML documentation comments following this style:

- `<summary>` — what the type or member does
- `<param>` — description of each parameter
- `<returns>` — what is returned (non-void methods)
- `<exception>` — exceptions that may be thrown
- `<remarks>` — additional detail for non-obvious behaviour
- `<example>` — usage example for key entry-point classes

> **Note:** Internal members also carry `<summary>` comments for IntelliSense and AI-tooling
> benefit, but they are excluded from this published reference.
