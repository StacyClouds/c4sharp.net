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

## Package map

- `StacyClouds.C4Sharp.Core`: model and view authoring primitives.
- `StacyClouds.C4Sharp.Client`: Structurizr API integration and encryption.
- `StacyClouds.C4Sharp.Renderer`: SVG rendering workflows.
- `StacyClouds.C4Sharp.Editor`: editor-oriented integration surface.

## Facilitation flow

1. Ask what outcome the developer wants (generate model, render SVG, publish, build editor).
2. Map outcome to required and optional packages.
3. Suggest install commands.
4. Offer the next focused skill (`core`, `client`, `renderer`, or `editor`) for deeper guidance.

## Output format

- Scenario summary
- Recommended package list (required/optional)
- Install command snippet
- Suggested next skill
