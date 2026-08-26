## Why

The SVG renderer makes workspace views portable, but users cannot directly organise a rendered diagram. A reusable Razor editor enables layout adjustments in applications that need them without forcing Blazor dependencies or static assets on renderer-only consumers.

## What Changes

- Add a separately packable `StacyClouds.C4Sharp.Editor` Razor class library.
- Provide a workspace SVG editor component with a collapsible thumbnail navigator for selecting workspace views.
- Persist element positions when a user drags and drops diagram elements.
- Insert ordered connector vertices when a user double-clicks a relationship arrow.
- Raise distinct layout-changed and save-requested callbacks so the host controls workspace persistence.
- Update the existing web project to demonstrate the optional editor package.
- Add documentation for installing and hosting the editor component.

## Capabilities

### New Capabilities

- `razor-svg-workspace-editing`: Optional Razor component package for displaying and interactively editing persisted C4 workspace layout.

### Modified Capabilities

- `svg-workspace-rendering`: Rendered SVG identifies diagram elements and relationships so optional interactive consumers can safely map user input back to workspace layout objects.

## Impact

- Adds a new NuGet package with a dependency on `StacyClouds.C4Sharp.Renderer`; the renderer package remains independent of ASP.NET Core and Razor.
- Updates SVG output with additive identifiers for elements and relationships.
- Updates the renderer web demonstration and user-facing documentation.
