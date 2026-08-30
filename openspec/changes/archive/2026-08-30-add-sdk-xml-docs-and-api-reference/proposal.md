## Why

C4Sharp.NET exposes a rich public API across four packages (Core, Client, Renderer, Editor), but a large proportion of types, members, and internal helpers still lack XML documentation comments. This creates a poor experience for both human developers relying on IntelliSense and for AI coding assistants that use inline documentation to understand and suggest correct API usage. Without reliable `<summary>`, `<param>`, `<returns>`, and `<exception>` comments, the SDK is harder to learn, audit, and extend safely.

A second gap exists in the `docs/` GitHub Pages site: it covers high-level guides but has no API reference section derived from the actual source code. Developers have no single place to look up every type and member without reading the source.

## What Changes

- Add or complete `<summary>`, `<param>`, `<returns>`, `<exception>`, `<remarks>`, and `<example>` XML doc comments on all public and meaningful internal members across `StacyClouds.C4Sharp.Core`, `StacyClouds.C4Sharp.Client`, `StacyClouds.C4Sharp.Renderer`, and `StacyClouds.C4Sharp.Editor`.
- Enable XML doc file generation in all library `.csproj` files so the compiler emits `.xml` artefacts.
- Generate a static API reference from those XML files and add it to the `docs/` folder for publication via GitHub Pages.
- Add a navigation entry in `docs/index.md` (and `_config.yml` if needed) pointing to the new API reference section.
- **BREAKING**: none; this change is purely additive.

## Capabilities

### New Capabilities
- `xml-doc-coverage`: Every public type and member in all four packages carries accurate XML documentation for IntelliSense and AI tooling.
- `api-reference-docs`: A browsable API reference section published to the GitHub Pages site under `docs/api/`.

### Modified Capabilities
- None to runtime behaviour; XML doc generation settings are a build-time addition.

## Impact

Affected areas:
- All `.cs` source files in `StacyClouds.C4Sharp.Core`, `StacyClouds.C4Sharp.Client`, `StacyClouds.C4Sharp.Renderer`, and `StacyClouds.C4Sharp.Editor`.
- The four library `.csproj` files (XML doc generation property).
- The `docs/` folder (new `api/` subdirectory and updated navigation).
- The CI/build pipeline if doc generation is wired into it.

This is a high-volume, low-risk change: no logic changes, only documentation additions and build configuration.
