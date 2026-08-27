---
name: core
description: Build and evolve C4 workspaces, models, and views with StacyClouds.C4Sharp.Core.
license: MIT
compatibility: C4Sharp.NET repository context.
metadata:
  author: StacyClouds
  version: "1.0"
---

Use this skill for Core package work.

## Install

```bash
dotnet add package StacyClouds.C4Sharp.Core
```

## Scope

- Workspace, model, and view creation
- Element and relationship styling
- Diagram structure for context, container, component, dynamic, and deployment views

## Minimal usage workflow

1. Create a workspace and model.
2. Add people/software systems/containers/components to the model.
3. Create one or more views (for example, system context or container).
4. Apply tags and styles for readability.
5. Hand off to `renderer` for SVG output or `client` for publishing.

## Guidance flow

1. Confirm the architecture scope being modeled.
2. Build the model entities first, then views.
3. Apply styles and tags after structure is complete.
4. Validate that output is ready for renderer/client workflows.

## Common package hand-offs

- Use `renderer` skill when the user asks for SVG output.
- Use `client` skill when the user asks to push/pull workspaces via Structurizr API.

## Expected output

- Minimal model + view workflow (and snippet when requested)
- Notes on which Core APIs to extend
- Follow-up recommendation for `renderer` or `client` when needed
