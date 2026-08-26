## 1. Implementation

- [x] Add predecessor-aware renderer tests for layout transfer, removal, deterministic new elements, unmatched views, and null inputs.
- [x] Add the documented `Render(Workspace workspace, Workspace predecessor)` overload using the Core layout-copy API.
- [x] Update SVG rendering documentation with both overload contracts and a predecessor example.

## 2. Validation

- [x] Run `dotnet test StacyClouds.C4Sharp.Renderer.Tests/StacyClouds.C4Sharp.Renderer.Tests.csproj`.
- [x] Run `dotnet restore StacyClouds.C4Sharp.slnx`.
- [x] Run `dotnet build StacyClouds.C4Sharp.slnx`.
- [x] Run `dotnet test StacyClouds.C4Sharp.slnx`.
- [x] Confirm `openspec status --change "preserve-layout-when-rendering-successor"` reports all apply-required artifacts complete.
