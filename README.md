# C4Sharp.NET

C4Sharp.NET is a multi-targeted .NET library for building architecture workspaces in code, publishing them to Structurizr-compatible services, rendering diagrams to SVG, and adding interactive editing to Blazor applications.

For C4 model background and notation guidance, use the official [C4 Model website](https://c4model.com).

## Package overview

| Package | Purpose | Start here when you need to... |
|---|---|---|
| `StacyClouds.C4Sharp.Core` | Workspace, model, view, and styling APIs | Define people, software systems, containers, components, and views in .NET code |
| `StacyClouds.C4Sharp.Client` | Structurizr-compatible API client | Read or publish workspaces, preserve layout, and use optional client-side encryption |
| `StacyClouds.C4Sharp.Renderer` | SVG renderer | Generate standalone SVG diagrams from workspace views |
| `StacyClouds.C4Sharp.Editor` | Interactive Blazor editor components | Embed a browser-based layout editor for rendered workspace views |

## Installation

```bash
dotnet add package StacyClouds.C4Sharp.Core
dotnet add package StacyClouds.C4Sharp.Client
dotnet add package StacyClouds.C4Sharp.Renderer
dotnet add package StacyClouds.C4Sharp.Editor
```

Install only the packages your application needs. `Core` is the foundation package. `Client` and `Renderer` build on `Core`, and `Editor` builds on `Renderer`.

## Quick example

```csharp
using StacyClouds.C4Sharp;

Workspace workspace = new Workspace("Getting Started", "A simple architecture workspace.");
Model model = workspace.Model;

Person user = model.AddPerson("User", "Uses the system.");
SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "Provides the core capability.");
user.Uses(softwareSystem, "Uses");

SystemContextView view = workspace.Views.CreateSystemContextView(
    softwareSystem,
    "system-context",
    "A simple system context view.");
view.AddAllPeople();
view.AddAllSoftwareSystems();

Styles styles = workspace.Views.Configuration.Styles;
styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
```

## Documentation

- [Package map](/home/runner/work/c4sharp.net/c4sharp.net/docs/nuget.md)
- [Core package guide](/home/runner/work/c4sharp.net/c4sharp.net/docs/getting-started.md)
- [Client package guide](/home/runner/work/c4sharp.net/c4sharp.net/docs/api-client.md)
- [Renderer package guide](/home/runner/work/c4sharp.net/c4sharp.net/docs/svg-rendering.md)
- [Editor package guide](/home/runner/work/c4sharp.net/c4sharp.net/docs/razor-svg-editor.md)
- [Client-side encryption](/home/runner/work/c4sharp.net/c4sharp.net/docs/client-side-encryption.md)
- [Changelog](/home/runner/work/c4sharp.net/c4sharp.net/docs/changelog.md)

## Table of contents

- [NuGet packages for developers](/home/runner/work/c4sharp.net/c4sharp.net/docs/nuget.md)
- [Using `StacyClouds.C4Sharp.Core`](/home/runner/work/c4sharp.net/c4sharp.net/docs/getting-started.md)
- [Using `StacyClouds.C4Sharp.Client`](/home/runner/work/c4sharp.net/c4sharp.net/docs/api-client.md)
- [Using `StacyClouds.C4Sharp.Renderer`](/home/runner/work/c4sharp.net/c4sharp.net/docs/svg-rendering.md)
- [Using `StacyClouds.C4Sharp.Editor`](/home/runner/work/c4sharp.net/c4sharp.net/docs/razor-svg-editor.md)
- [Using client-side encryption](/home/runner/work/c4sharp.net/c4sharp.net/docs/client-side-encryption.md)
- [C4 Model reference](https://c4model.com)
