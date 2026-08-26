# AGENTS.md

## Overview

C4Sharp.NET is a multi-targeted .NET library for creating C4 architecture
models, diagrams, and Structurizr-compatible workspaces. The actively
maintained solution is `StacyClouds.C4Sharp.slnx`; the `safe/` directory is a
reference copy of the upstream Structurizr code and should not be changed
unless a task explicitly requires it.

## Repository layout

- `StacyClouds.C4Sharp.Core/` — model, views, configuration, documentation,
  and DSL foundation code.
- `StacyClouds.C4Sharp.Client/` — Structurizr API client, encryption, I/O,
  and utilities.
- `StacyClouds.C4Sharp.Core.Tests/` and `StacyClouds.C4Sharp.Client.Tests/`
  — xUnit/Shouldly test projects that mirror production areas.
- `StacyClouds.C4Sharp.Examples/` — executable usage examples.
- `docs/` — user-facing documentation.
- `openspec/` — OpenSpec change proposals and specifications.

## Development conventions

- Target frameworks are `net8.0`, `net9.0`, `net10.0`, and `net11.0`. Keep
  public changes compatible with every target unless the task states
  otherwise.
- Follow the existing C# style: tabs for indentation, standard .NET naming,
  focused methods, and XML documentation for public APIs.
- Place tests beside their feature area and use descriptive xUnit test names.
  Write tests in Arrange-Act-Assert form and use Shouldly assertions where
  established in the project.
- Treat the `safe/` sources as reference material, not as the primary build or
  test target.
- Preserve unrelated working-tree changes. Do not reformat or update generated
  `bin/`, `obj/`, or Stryker output directories.

## Validation

Run the smallest relevant validation first:

```bash
dotnet test StacyClouds.C4Sharp.Core.Tests/StacyClouds.C4Sharp.Core.Tests.csproj
dotnet test StacyClouds.C4Sharp.Client.Tests/StacyClouds.C4Sharp.Client.Tests.csproj
```

Before handing off cross-project changes, validate the maintained solution:

```bash
dotnet restore StacyClouds.C4Sharp.slnx
dotnet build StacyClouds.C4Sharp.slnx
dotnet test StacyClouds.C4Sharp.slnx
```

For .NET 11-specific work, use the commands below:

```bash
dotnet restore StacyClouds.C4Sharp.slnx -p:TargetFramework=net11.0
dotnet build StacyClouds.C4Sharp.slnx -p:TargetFramework=net11.0
dotnet test StacyClouds.C4Sharp.slnx -p:TargetFramework=net11.0
```

## Changes and documentation

- Add or update tests for behavior changes and update `docs/` or `README.md`
  when public behavior changes.
- Version and NuGet package metadata live in the library `.csproj` files;
  update them only for an intentional release task.
- Use concise, present-tense commit subjects (50 characters or fewer when
  practical), scoped to the change.
