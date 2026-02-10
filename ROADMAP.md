# C4Sharp.NET Roadmap

This document outlines planned improvements and enhancements for C4Sharp.NET.

## Completed ✅

### Version 2.0 (Current)
- ✅ Updated to support .NET 8, 9, and 10 (multi-targeting)
- ✅ Migrated from netstandard2.0 and netcoreapp3.1
- ✅ Updated all NuGet package dependencies to latest versions
- ✅ Updated test frameworks (xUnit, Microsoft.NET.Test.Sdk)
- ✅ Addressed security vulnerability in Newtonsoft.Json (upgraded to 13.0.3)
- ✅ Rebranded from Structurizr to C4Sharp.NET
- ✅ Updated NuGet package metadata (owner: StacyClouds)
- ✅ Created project website with GitHub Pages
- ✅ Updated README with modern branding and documentation
- ✅ All 448 unit tests passing on .NET 8, 9, and 10

## Planned Improvements

### High Priority

#### 1. Code Quality Improvements
- Fix compiler warnings (unused variables, missing GetHashCode implementations)
- Add XML documentation comments for public APIs
- Improve code consistency and style
- Add code analysis rules (StyleCop, Roslyn analyzers)

#### 2. System.Text.Json Migration
Migrate from Newtonsoft.Json to System.Text.Json for better performance and .NET integration.
- **Rationale**: System.Text.Json is the modern .NET standard and offers better performance
- **Challenge**: Requires handling of circular references and complex object graphs
- **Impact**: Breaking change - would require major version bump
- **Status**: Deferred - requires extensive testing and validation

#### 3. C4 DSL Gap Analysis
Compare against the latest C4 DSL specification and identify missing features:
- Review [Structurizr DSL](https://docs.structurizr.com/dsl) latest features
- Identify features present in DSL but missing in C4Sharp.NET
- Prioritize features based on user needs
- Create implementation plan

### Medium Priority

#### 4. Enhanced Documentation
- Add more code examples for common scenarios
- Create tutorial series (beginner to advanced)
- Add API reference documentation
- Create video tutorials
- Add diagram examples with rendered output

#### 5. Performance Optimizations
- Profile serialization/deserialization performance
- Optimize memory usage for large models
- Consider lazy loading for complex relationships
- Add benchmarks to track performance improvements

#### 6. Modern .NET Features
- Utilize C# 12/13 features where appropriate
- Consider nullable reference types throughout codebase
- Use collection expressions and other modern patterns
- Evaluate source generators for boilerplate reduction

### Low Priority

#### 7. Additional Export Formats
- Export to PlantUML format
- Export to Mermaid format
- Export to draw.io format
- Support for other diagram formats

#### 8. Validation and Linting
- Add model validation rules
- Warn about common mistakes
- Provide suggestions for improvements
- Add design pattern detection

#### 9. CLI Tool
- Create command-line tool for common operations
- Support for bulk operations
- Integration with CI/CD pipelines
- Template generation

## Feature Requests

Feature requests can be submitted via [GitHub Issues](https://github.com/StacyClouds/c4sharp.net/issues).

When submitting a feature request, please include:
- Clear description of the feature
- Use case or scenario
- Example of desired behavior
- Any relevant references to C4 DSL or other implementations

## Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## Version History

### 2.0.0 (2026)
- Multi-framework support (.NET 8, 9, 10)
- Updated dependencies
- Security fixes
- Rebranding to C4Sharp.NET

### 1.1.1 (Previous)
- Legacy Structurizr for .NET release
- Targeted netstandard2.0 and netcoreapp3.1
