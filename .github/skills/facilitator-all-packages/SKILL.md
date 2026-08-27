---
name: facilitator-all-packages
description: Facilitate package selection across Core, Client, Renderer, and Editor for diagram-as-code workflows.
license: MIT
compatibility: C4Sharp.NET repository context.
metadata:
  author: StacyClouds
  version: "1.0"
---

Use this skill when a developer needs guidance across all C4Sharp.NET packages.

## Goals

- Identify the minimum package set needed for a scenario.
- Explain package responsibilities and composition options.
- Recommend a practical starting path for implementation.
- Provide concrete package usage steps, not only package names.

## Package map

- `StacyClouds.C4Sharp.Core`: model and view authoring primitives.
- `StacyClouds.C4Sharp.Client`: Structurizr API integration and encryption.
- `StacyClouds.C4Sharp.Renderer`: SVG rendering workflows.
- `StacyClouds.C4Sharp.Editor`: editor-oriented integration surface.

## Installation quick start

```bash
dotnet add package StacyClouds.C4Sharp.Core
dotnet add package StacyClouds.C4Sharp.Client
dotnet add package StacyClouds.C4Sharp.Renderer
dotnet add package StacyClouds.C4Sharp.Editor
```

Install the smallest set required for the scenario:

- Core only: model and views in code.
- Core + Renderer: generate SVG outputs.
- Core + Client: publish/retrieve workspaces.
- Core + Editor (+ optional Renderer/Client): interactive editing workflows.

## Scenario-to-package guide

| Scenario | Required packages | Optional packages |
|---|---|---|
| Build a C4 model in C# | Core | - |
| Render architecture diagrams to SVG | Core, Renderer | - |
| Publish model to Structurizr | Core, Client | Renderer |
| Build an editor-like app | Core, Editor | Renderer, Client |

## API correctness protocol for agents

- Do not assume method/type names from memory alone.
- Ground package usage guidance in repository docs and package tests.
- If an API detail is uncertain, inspect the corresponding package source and tests before returning a snippet.

## Authoritative references

- `docs/nuget.md`
- `docs/getting-started.md`
- `docs/api-client.md`
- `docs/svg-rendering.md`
- `docs/razor-svg-editor.md`

## Facilitation flow

1. Ask what outcome the developer wants (generate model, render SVG, publish, build editor).
2. Map outcome to required and optional packages.
3. Provide install commands and a minimal usage path.
4. Validate API details against references before giving package-specific snippets.
5. Offer the next focused skill (`core`, `client`, `renderer`, or `editor`) for deeper guidance.

## Output format

- Scenario summary
- Recommended package list (required/optional)
- Install command snippet
- First implementation step (what code to write first)
- API verification note (what doc/source/test references were used)
- Suggested next skill
