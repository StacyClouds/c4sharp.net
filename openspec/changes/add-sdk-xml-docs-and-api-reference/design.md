## Context

C4Sharp.NET is a multi-package .NET SDK. The four library packages share a common convention: XML documentation drives IntelliSense tooltips and can be processed by static-site tools to produce browsable API reference pages. The project already publishes a GitHub Pages site under `docs/` using Jekyll with the Cayman theme. The goal of this design is to close the XML doc coverage gap and attach a generated API reference to the existing site without requiring a complex build pipeline.

## Goals / Non-Goals

**Goals:**
- Achieve full XML doc coverage on public members across all four library packages.
- Cover meaningful `internal` helpers where the comments aid maintainers or AI assistants.
- Emit `.xml` documentation files alongside `.dll` outputs for all library projects.
- Generate static HTML API reference pages from those XML files and place them under `docs/api/`.
- Wire the API reference into the existing Jekyll navigation.

**Non-Goals:**
- Switching to a different documentation site generator (e.g., DocFX replacing Jekyll entirely).
- Generating interactive playgrounds or live code samples.
- Documenting test projects or the `Examples` project.
- Changing any runtime logic or public API signatures.

## Decisions

- **Use DocFX** to generate the API reference. DocFX reads XML doc files and produces Markdown or HTML that integrates with the existing Jekyll site. The generated pages go into `docs/api/` and are committed to the repository so GitHub Pages serves them without a custom CI step.
  - Alternative considered: xmldoc2md. Lighter weight but produces less structured output and has poorer cross-reference support.
  - Alternative considered: full DocFX site replacing Jekyll. Rejected because the existing hand-authored guides are well-maintained and replacing the site generator is out of scope.
- **Enable `<GenerateDocumentationFile>true</GenerateDocumentationFile>`** in each library `.csproj`. This is the standard .NET property for emitting XML doc output.
- **Suppress CS1591** (missing XML comment warning) as a CI-breaking error only after coverage is complete, to avoid noise during incremental work. Once coverage is complete, re-enable it as a warning in the project settings.
- **Comment style**: use `<summary>` for all types and members, `<param>` and `<returns>` for all non-void methods, `<exception>` where an exception is documented, `<remarks>` for non-obvious behaviour, and `<example>` only for public entry-point classes where a short usage snippet meaningfully helps.

## Risks / Trade-offs

- [Volume] Writing accurate comments for ~96 Core files and ~19 Client files (plus Renderer and Editor) is significant effort. Mitigate by working package by package, validating with a build warning count before moving on.
- [Accuracy drift] AI-generated or bulk-added comments can be vague. Mitigate by reviewing generated drafts against source logic and existing tests.
- [DocFX output size] Generated HTML can be large. Mitigate by committing only the essential output (`docs/api/`) and adding `docfx/` working directory to `.gitignore`.

## Migration Plan

No migration needed for end users; this change adds content without removing anything. The `docs/api/` directory is new, so no existing links break.

## Open Questions

- Should the DocFX generation be added to CI (e.g., triggered on merge to main) or treated as a manual step committed with each documentation change?
- Should internal members be included in the published API reference, or only public members?

## Implementation Notes

- Work package by package: Core → Client → Renderer → Editor.
- For each package: enumerate undocumented public members, add XML comments, rebuild, check remaining warning count.
- Run `dotnet build -warnaserror:CS1591` at the end of each package pass to confirm zero missing-doc warnings.
- Use DocFX `docfx.json` at repository root (or `docs/docfx.json`) pointing at the four library projects. Output goes to `docs/api/`.
- Add a "API Reference" nav link in `docs/index.md` pointing to `api/`.
