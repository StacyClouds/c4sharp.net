## 1. Expand target framework matrix

- [x] 1.1 Add `net11.0` to `TargetFrameworks` in all primary library projects (`StacyClouds.C4Sharp.Core`, `StacyClouds.C4Sharp.Client`).
- [x] 1.2 Add `net11.0` to `TargetFrameworks` in all corresponding test and example projects.
- [x] 1.3 Run restore/build for `StacyClouds.C4Sharp.sln` and resolve any net11 target-resolution issues.

## 2. Modernize cryptography for forward compatibility

- [x] 2.1 Refactor `StacyClouds.C4Sharp.Client/Encryption/AesEncryptionStrategy.cs` to use non-obsolete PBKDF2 APIs instead of obsolete constructors.
- [x] 2.2 Add or update tests to verify encryption/decryption compatibility and expected behavior after PBKDF2 migration.
- [x] 2.3 Confirm client project builds without PBKDF2 obsolescence warnings across supported target frameworks.

## 3. Update documentation and contribution guidance

- [x] 3.1 Update `README.md` support statements and feature list to include .NET 11.
- [x] 3.2 Update `CONTRIBUTING.md` prerequisites and sample commands to include .NET 11.
- [x] 3.3 Review other version-declaration docs and align support matrix wording with actual targets.

## 4. Establish readiness validation gate

- [x] 4.1 Define and document the net11 readiness validation command set (restore/build/test scope).
- [x] 4.2 Execute validation commands and capture outcomes for maintainer sign-off.
- [x] 4.3 Finalize change by confirming all requirements in `specs/dotnet-11-readiness/spec.md` are satisfied.
