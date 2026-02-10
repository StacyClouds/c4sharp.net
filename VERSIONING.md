# Automated Versioning

This repository uses [MinVer](https://github.com/adamralph/minver) for automated semantic versioning based on Git tags.

## How It Works

MinVer automatically calculates the package version by:
1. Looking for Git tags with the format `v{major}.{minor}.{patch}` (e.g., `v1.1.1`)
2. If on a tagged commit, uses that version exactly
3. If commits exist after the latest tag, generates a pre-release version with format: `{major}.{minor}.{patch}-alpha.{commit-count}.{commit-hash}`

## Creating a New Release

To create a new release:

1. **Update the changelog** in `docs/changelog.md` with the new version and changes
2. **Commit all changes** to the repository
3. **Create and push a Git tag**:
   ```bash
   git tag v{major}.{minor}.{patch}
   git push origin v{major}.{minor}.{patch}
   ```
4. **Build and pack the release**:
   ```bash
   dotnet clean -c Release
   dotnet build -c Release
   ```
   
   The NuGet packages will be created in `{Project}/bin/Release/` with the version from the tag.

## Version Examples

- **Tagged commit** `v1.2.0` → Package version: `1.2.0`
- **3 commits after** `v1.2.0` → Package version: `1.2.1-alpha.0.3.abc1234` (pre-release)
- **No tags** → Uses `MinVerMinimumMajorMinor` setting (currently `1.1`)

## Configuration

The following MinVer settings are configured in the `.csproj` files:

- **MinVerTagPrefix**: `v` - Tags must start with "v" (e.g., v1.0.0)
- **MinVerMinimumMajorMinor**: `1.1` - Minimum version if no tags are found

## CI/CD Integration

When building in CI/CD pipelines, MinVer automatically detects the Git context and calculates the appropriate version. No manual version updates in project files are needed.

### AppVeyor Example

The existing `appveyor.yml` configuration can be simplified since MinVer handles versioning automatically:

```yaml
build_script:
- ps: dotnet build -c Release
- ps: dotnet pack -c Release
```

## Benefits

- **No manual version updates** in project files
- **Automatic pre-release versions** for non-tagged commits
- **Consistent versioning** across all packages
- **Git is the single source of truth** for versions
- **Follows semantic versioning** conventions
