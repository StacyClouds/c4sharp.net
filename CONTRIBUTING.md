# Contributing to C4Sharp.NET

Thank you for your interest in contributing to C4Sharp.NET! This document provides guidelines and instructions for contributing.

## Code of Conduct

Please review and follow our [Code of Conduct](https://swetugg.se/codeofconduct).

## Getting Started

### Prerequisites

- .NET 8, 9, or 10 SDK
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

## Development Workflow

### Building

```bash
# Build all projects
dotnet build

# Build in Release mode
dotnet build --configuration Release

# Build specific project
dotnet build Structurizr.Core/Structurizr.Core.csproj
```

### Testing

```bash
# Run all tests
dotnet test

# Run tests for specific framework
dotnet test --framework net10.0

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test
dotnet test --filter "FullyQualifiedName~TestName"
```

### Creating NuGet Packages

```bash
# Packages are created automatically on build
dotnet build --configuration Release

# Find packages in:
# Structurizr.Core/bin/Release/
# Structurizr.Client/bin/Release/
```

## Project Structure

```
c4sharp.net/
├── Structurizr.Core/          # Core model library
├── Structurizr.Core.Tests/    # Core library tests
├── Structurizr.Client/         # API client library
├── Structurizr.Client.Tests/   # Client library tests
├── Structurizr.Examples/       # Example applications
├── docs/                       # Documentation and website
└── README.md                   # Project readme
```

## Questions?

- Check existing [issues](https://github.com/StacyClouds/c4sharp.net/issues)
- Create a new issue for questions

## License

By contributing, you agree that your contributions will be licensed under the Apache License 2.0.

Thank you for contributing to C4Sharp.NET! 🎉
