---
name: renderer
description: Render C4 views to SVG with StacyClouds.C4Sharp.Renderer.
license: MIT
compatibility: C4Sharp.NET repository context.
metadata:
  author: StacyClouds
  version: "1.0"
---

Use this skill for rendering architecture diagrams.

## Install

```bash
dotnet add package StacyClouds.C4Sharp.Renderer
```

## Scope

- SVG rendering entry points
- Render pipeline integration in apps or build steps
- Output verification for diagram quality

## Prerequisite

The views to render are normally created with `StacyClouds.C4Sharp.Core`.

## Guidance flow

1. Confirm which views must be rendered.
2. Ensure model/view data is complete from Core.
3. Render and save SVG output.
4. Validate visual completeness and style expectations.

## Usage checklist

- Define output folder and file naming strategy.
- Render each target view consistently (context/container/component/etc.).
- Verify generated SVG files are non-empty and readable in browser/viewer.

## API correctness protocol

- Verify renderer API entry points against package source/tests before returning snippets.
- Prefer rendering guidance aligned with `docs/svg-rendering.md`.
- If an output option/parameter is uncertain, inspect implementation first and then provide guidance.

## Authoritative references

- `docs/svg-rendering.md`
- `StacyClouds.C4Sharp.Renderer/`
- `StacyClouds.C4Sharp.Renderer.Tests/`

## Expected output

- Rendering snippet with output path guidance
- Checklist for common rendering prerequisites (with verified API calls)
- Next-step recommendation for editor integration when interactive workflows are required
