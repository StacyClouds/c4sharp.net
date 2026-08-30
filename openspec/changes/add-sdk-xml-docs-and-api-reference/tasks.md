## 1. Setup

- [x] 1.1 Enable `<GenerateDocumentationFile>true</GenerateDocumentationFile>` in `StacyClouds.C4Sharp.Core/StacyClouds.C4Sharp.Core.csproj`.
- [x] 1.2 Enable `<GenerateDocumentationFile>true</GenerateDocumentationFile>` in `StacyClouds.C4Sharp.Client/StacyClouds.C4Sharp.Client.csproj`.
- [x] 1.3 Enable `<GenerateDocumentationFile>true</GenerateDocumentationFile>` in `StacyClouds.C4Sharp.Renderer/StacyClouds.C4Sharp.Renderer.csproj`.
- [x] 1.4 Enable `<GenerateDocumentationFile>true</GenerateDocumentationFile>` in `StacyClouds.C4Sharp.Editor/StacyClouds.C4Sharp.Editor.csproj`.
- [x] 1.5 Add `docfx/` to `.gitignore` to exclude the DocFX working directory from source control.
- [x] 1.6 Install DocFX as a .NET tool and add it to `dotnet-tools.json` (or document the required version in `CONTRIBUTING.md`).

## 2. XML Doc Coverage — Core

- [ ] 2.1 Enumerate all public types and members in `StacyClouds.C4Sharp.Core` that are missing `<summary>` comments (`dotnet build -warnaserror:CS1591`).
- [ ] 2.2 Add XML doc comments to all public types: model elements (`Person`, `SoftwareSystem`, `Container`, `Component`, `DeploymentNode`, etc.), view types, configuration, styles, and DSL types.
- [ ] 2.3 Add XML doc comments to all public members: constructors, properties, methods, extension methods, enums, and constants.
- [ ] 2.4 Add `<param>`, `<returns>`, and `<exception>` tags to all public non-void methods with parameters.
- [ ] 2.5 Add `<remarks>` where behaviour is non-obvious (e.g., implied-relationship strategy effects, identifier generation rules).
- [ ] 2.6 Add `<summary>` to meaningful `internal` helpers in Core that AI assistants or maintainers would benefit from understanding.
- [ ] 2.7 Build Core and confirm zero CS1591 warnings.

## 3. XML Doc Coverage — Client

- [ ] 3.1 Enumerate all public types and members in `StacyClouds.C4Sharp.Client` that are missing `<summary>` comments.
- [ ] 3.2 Add XML doc comments to all public types and members (API client, encryption, serialisation, I/O helpers).
- [ ] 3.3 Add `<param>`, `<returns>`, and `<exception>` tags to all public non-void methods with parameters.
- [ ] 3.4 Add `<summary>` to meaningful `internal` helpers in Client.
- [ ] 3.5 Build Client and confirm zero CS1591 warnings.

## 4. XML Doc Coverage — Renderer

- [ ] 4.1 Enumerate all public types and members in `StacyClouds.C4Sharp.Renderer` that are missing `<summary>` comments.
- [ ] 4.2 Add XML doc comments to all public types and members.
- [ ] 4.3 Add `<param>`, `<returns>`, and `<exception>` tags to all public non-void methods with parameters.
- [ ] 4.4 Build Renderer and confirm zero CS1591 warnings.

## 5. XML Doc Coverage — Editor

- [ ] 5.1 Enumerate all public types and members in `StacyClouds.C4Sharp.Editor` that are missing `<summary>` comments.
- [ ] 5.2 Add XML doc comments to all public types and members.
- [ ] 5.3 Add `<param>`, `<returns>`, and `<exception>` tags to all public non-void methods with parameters.
- [ ] 5.4 Build Editor and confirm zero CS1591 warnings.

## 6. API Reference Generation

- [x] 6.1 Create a `docfx.json` configuration at the repository root (or `docs/docfx.json`) referencing all four library projects, configuring output to `docs/api/`, and setting `includePrivateMembers: false` and visibility to public-only.
- [ ] 6.2 Run DocFX to generate the API reference Markdown/HTML output into `docs/api/`.
- [ ] 6.3 Validate that the generated output renders correctly in the Jekyll site structure (check links and layout).
- [x] 6.4 Add or update the `nav` section in `docs/_config.yml` (or add a navigation link in `docs/index.md`) pointing to the new API reference.

## 7. Docs Site Update

- [x] 7.1 Add an `api-reference.md` (or `docs/api/index.md`) entry page that introduces the API reference section and links to each package's generated index.
- [x] 7.2 Update `docs/index.md` to include a link to the API reference in the "Next steps" or navigation area.
- [x] 7.3 Review and update `CONTRIBUTING.md` to document how to regenerate the API reference after code changes.

## 8. Build validation

- [ ] 8.1 Run `dotnet build StacyClouds.C4Sharp.slnx` and confirm no new errors or warnings.
- [ ] 8.2 Run `dotnet test StacyClouds.C4Sharp.slnx` and confirm all tests pass.
- [ ] 8.3 Run `dotnet build StacyClouds.C4Sharp.slnx -warnaserror:CS1591` and confirm zero missing-doc warnings across all packages.

## 9. Release flow automation

- [x] 9.1 Create or update the release GitHub Actions workflow (`.github/workflows/`) to include a doc-regeneration step: restore tools, build the solution, run DocFX to regenerate `docs/api/`.
- [x] 9.2 Add a step in the release workflow that creates a versioned release document (e.g., `docs/api/release-notes-<version>.md` or updates `docs/api/index.md` with the release version) from the tag or release metadata.
- [x] 9.3 Add a step that commits the regenerated `docs/api/` output back to the repository (or pushes it to the `gh-pages` branch, depending on the current Pages setup) so GitHub Pages redeploys automatically.
- [ ] 9.4 Validate the release workflow on a test tag to confirm that the docs are regenerated, committed, and the Pages site updates correctly.
