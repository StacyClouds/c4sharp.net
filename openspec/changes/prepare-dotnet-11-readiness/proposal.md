## Why

The library currently targets .NET 8, 9, and 10, but does not yet declare or validate .NET 11 support. Preparing now reduces release risk and ensures users can adopt .NET 11 without waiting for post-release fixes.

## What Changes

- Add .NET 11 to the multi-target framework matrix for library, test, and example projects.
- Define a compatibility baseline for .NET 11 builds/tests and document pass criteria.
- Address known framework-obsolescence warnings that are likely to become stricter under newer runtimes (for example PBKDF2 API usage).
- Update contributor and user documentation to reflect .NET 11 support and required SDKs.

## Capabilities

### New Capabilities
- `dotnet-11-readiness`: Establishes a verifiable process and requirements for adding, validating, and documenting .NET 11 support across the repository.

### Modified Capabilities
- None.

## Impact

- Affected code: project target frameworks across maintained `StacyClouds.C4Sharp.*` projects; crypto implementation in `StacyClouds.C4Sharp.Client/Encryption/AesEncryptionStrategy.cs`.
- Affected docs: `README.md`, `CONTRIBUTING.md`, and any CI/build guidance that declares supported SDKs.
- Affected tooling/CI: build and test matrix updates to include net11.0 where applicable.
