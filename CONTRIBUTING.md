# Contributing to C4Sharp.NET

Thank you for your interest in contributing to C4Sharp.NET! This document provides guidelines and instructions for contributing.

## Code of Conduct

### The Quick Version

All contributors, maintainers, and participants in this project are required to agree with the following code of conduct. Maintainers will enforce this code throughout the project. We expect cooperation from all participants to help ensure a safe and welcoming environment for everybody.

### Our Standards

This project is dedicated to providing a harassment-free experience for everyone, regardless of gender, gender identity and expression, age, sexual orientation, disability, physical appearance, body size, race, ethnicity, religion (or lack thereof), or technology choices. We do not tolerate harassment of project participants in any form. Sexual language and imagery is not appropriate for any project space, including code comments, issues, pull requests, discussions, or other online media. Project participants violating these rules may be sanctioned or removed from the project at the discretion of the project maintainers.

### Unacceptable Behavior

Harassment includes offensive verbal comments related to gender, gender identity and expression, age, sexual orientation, disability, physical appearance, body size, race, ethnicity, religion, technology choices, sexual images in public spaces, deliberate intimidation, stalking, following, harassing photography or recording, sustained disruption of discussions or other events, inappropriate contact, and unwelcome sexual attention.

Participants asked to stop any harassing behavior are expected to comply immediately.

If a participant engages in harassing behavior, the project maintainers may take any action they deem appropriate, including warning the offender or removal from the project.

### Reporting

If you are being harassed, notice that someone else is being harassed, or have any other concerns, please contact the project maintainers immediately by opening a private issue or emailing the maintainers directly.

Project maintainers will investigate all complaints and respond in a way that they deem appropriate to the circumstances. Maintainers are obligated to maintain confidentiality with regard to the reporter of an incident.

We value your participation and want to ensure everyone feels welcome and safe in our community.

### Scope

We expect all project participants to follow these rules in all project spaces, including GitHub repositories, issue trackers, pull requests, discussions, and any project-related communication channels.

## Getting Started

### Prerequisites

- .NET 8, 9, 10, or 11 SDK
- Visual Studio 2022, VS Code, or Rider
- Git

### Setting Up Development Environment

1. Fork the repository on GitHub
2. Clone your fork locally:
   ```bash
   git clone https://github.com/YOUR-USERNAME/c4sharp.net.git
   cd c4sharp.net
   ```

3. Add upstream remote:
   ```bash
   git remote add upstream https://github.com/StacyClouds/c4sharp.net.git
   ```

4. Restore dependencies:
   ```bash
   dotnet restore
   ```

5. Build the solution:
   ```bash
   dotnet build
   ```

6. Run tests:
   ```bash
   dotnet test
   ```

## How to Contribute

### Reporting Bugs

If you find a bug, please create an issue on GitHub with:
- Clear title and description
- Steps to reproduce the issue
- Expected behavior
- Actual behavior
- Environment details (.NET version, OS, etc.)
- Code sample if applicable

### Suggesting Features

Feature suggestions are welcome! Please create an issue with:
- Clear description of the feature
- Use case or scenario
- Example of how it would be used
- Any relevant references (C4 DSL, other implementations)

### Contributing Code

1. **Create an issue first** - Discuss your changes before starting work
2. **Create a feature branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make your changes**:
   - Follow existing code style and conventions
   - Add or update tests as needed
   - Update documentation if applicable
   - Keep commits focused and atomic
   - Write clear commit messages

4. **Ensure tests pass**:
   ```bash
   dotnet test
   ```

5. **Create a pull request**:
   - Provide a clear description of changes
   - Reference any related issues
   - Ensure CI builds pass
   - Be responsive to feedback

## Coding Guidelines

### Style

- Follow standard C# naming conventions
- Use tabs for indentation (not spaces)
- Use meaningful variable and method names
- Keep methods focused and concise
- Add XML documentation comments for public APIs

### Testing

- Write unit tests for new features
- Maintain or improve code coverage
- Tests should be fast and reliable
- Use descriptive test names
- Follow Arrange-Act-Assert pattern

### Commit Messages

- Use present tense ("Add feature" not "Added feature")
- Be descriptive but concise
- Reference issues when applicable (#123)
- First line should be 50 characters or less
- Add details in the body if needed

### Versioning and Release Impact

Publish versions are generated automatically by the deploy workflow as:
`<major>.<minor>.<run_number>`.

To control **major** and **minor** values, use Conventional Commit prefixes:

- **Patch-level change (default contribution path):** `fix:`, `perf:`, `refactor:`
- **Minor release:** `feat:`
- **Major release:** add `!` after the type/scope (for example `feat!:` or
  `refactor(core)!:`) or include a `BREAKING CHANGE:` footer in the commit body

If you do not use one of these commit formats, your change may not produce the
expected major/minor release line in publish runs.

Example:
```
Add support for custom color schemes (#123)

- Implement ColorScheme class
- Add tests for color validation
- Update documentation with examples
```

## Pull Request Process

1. Ensure your code builds without warnings
2. All tests must pass
3. Update documentation as needed
4. Add an entry to CHANGELOG.md if applicable
5. Request review from maintainers
6. Address review feedback promptly
7. Squash commits if requested
8. Maintainers will merge when approved

### What reviewers need from contributors

- A clear problem statement in the PR description
- Why the change is needed and what behavior it changes
- Test evidence (commands and results)
- Notes about compatibility, migration, or breaking changes when relevant
- Linked issues/discussions for context

## Development Workflow

### Building

```bash
# Build all projects
dotnet build

# Build in Release mode
dotnet build --configuration Release

# Build specific project
dotnet build StacyClouds.C4Sharp.Core/StacyClouds.C4Sharp.Core.csproj
```

### Testing

```bash
# Run all tests
dotnet test

# Run tests for specific framework
dotnet test --framework net11.0

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test
dotnet test --filter "FullyQualifiedName~TestName"
```

### Mutation Testing (Stryker)

```bash
# Install the Stryker .NET global tool (one-time)
dotnet tool install --global dotnet-stryker

# Run mutation testing with repository config
bash scripts/run-stryker.sh

# Run mutation testing for the DSL foundation scope
dotnet stryker --config-file stryker-dsl-config.json
```

Current baseline scope is business-logic encryption code under
`StacyClouds.C4Sharp.Client/Encryption/**/*.cs` with a break threshold of 80%.

The DSL foundation scope covers `StacyClouds.C4Sharp.Core/Dsl/DslWorkspaceImporter.cs`
and `StacyClouds.C4Sharp.Core/Dsl/DslIdGenerator.cs`.

### Creating NuGet Packages

```bash
# Packages are created automatically on build
dotnet build --configuration Release

# Find packages in:
# StacyClouds.C4Sharp.Core/bin/Release/StacyClouds.C4Sharp.Core.*.nupkg
# StacyClouds.C4Sharp.Client/bin/Release/StacyClouds.C4Sharp.Client.*.nupkg
```

## Project Structure

```
c4sharp.net/
├── StacyClouds.C4Sharp.Core/           # Core model library
├── StacyClouds.C4Sharp.Core.Tests/     # Core library tests
├── StacyClouds.C4Sharp.Client/         # API client library
├── StacyClouds.C4Sharp.Client.Tests/   # Client library tests
├── StacyClouds.C4Sharp.Examples/       # Example applications
├── docs/                       # Documentation and website
└── README.md                   # Project readme
```

## .NET 11 readiness validation

Use this command set to validate the .NET 11 support gate for the maintained C4Sharp solution:

```bash
dotnet restore StacyClouds.C4Sharp.slnx -p:TargetFramework=net11.0
dotnet build StacyClouds.C4Sharp.slnx -p:TargetFramework=net11.0
dotnet test StacyClouds.C4Sharp.slnx -p:TargetFramework=net11.0
```

## Questions?

- Check existing [issues](https://github.com/StacyClouds/c4sharp.net/issues)
- Create a new issue for questions

## License

By contributing, you agree that your contributions will be licensed under the Apache License 2.0.

Thank you for contributing to C4Sharp.NET! 🎉
