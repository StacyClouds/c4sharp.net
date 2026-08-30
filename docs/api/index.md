---
title: API Reference
---

# C4Sharp.NET API Reference

This section contains the full public API reference for all C4Sharp.NET packages,
generated from XML documentation comments in the source code.

## Packages

| Package | Description |
|---------|-------------|
| [StacyClouds.C4Sharp.Core](core/) | Core model types — workspaces, elements, relationships, views, styles, and DSL import |
| [StacyClouds.C4Sharp.Client](client/) | Structurizr API client — publish, fetch, and encrypt workspaces |
| [StacyClouds.C4Sharp.Renderer](renderer/) | SVG rendering — render workspace views to standalone SVG |
| [StacyClouds.C4Sharp.Editor](editor/) | Blazor editor — interactive layout editing for SVG diagrams |

## How this reference is generated

The API reference is regenerated automatically during each release:

1. The release workflow builds the solution with `GenerateDocumentationFile` enabled.
2. DocFX reads the XML documentation files and source projects.
3. The generated HTML site is committed to `docs/api/` and GitHub Pages redeploys.

To regenerate locally:

```bash
dotnet restore
dotnet build StacyClouds.C4Sharp.slnx -c Release
dotnet docfx docfx.json
```

See [CONTRIBUTING.md](https://github.com/StacyClouds/c4sharp.net/blob/main/CONTRIBUTING.md) for full instructions.

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
