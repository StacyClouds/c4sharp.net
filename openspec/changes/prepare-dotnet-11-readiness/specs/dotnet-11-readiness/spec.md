## ADDED Requirements

### Requirement: Project targets include .NET 11
The repository MUST include `net11.0` in the `TargetFrameworks` list for maintained `StacyClouds.C4Sharp` library, test, and example projects that are currently part of the supported runtime matrix.

#### Scenario: Multi-target project declarations include net11.0
- **WHEN** maintainers inspect project files that currently target `net8.0;net9.0;net10.0`
- **THEN** each relevant project includes `net11.0` in its `TargetFrameworks` declaration

### Requirement: .NET 11 restore/build/test validation is defined and executable
The repository SHALL define and run validation commands that confirm restore/build/test complete successfully for the net11-enabled project set.

#### Scenario: Validation commands succeed with .NET 11 SDK
- **WHEN** maintainers run the documented readiness validation commands using an installed .NET 11 SDK
- **THEN** restore/build/test complete successfully for the targeted solutions or projects without net11 target-resolution errors

### Requirement: Encryption key derivation uses non-obsolete PBKDF2 APIs
The `StacyClouds.C4Sharp.Client` encryption implementation MUST avoid obsolete `Rfc2898DeriveBytes` constructor usage and use supported PBKDF2 APIs compatible with current target frameworks.

#### Scenario: Encryption code no longer emits PBKDF2 obsolescence warnings for supported target frameworks
- **WHEN** maintainers build the C4Sharp client project across supported target frameworks
- **THEN** build output does not include PBKDF2 constructor obsolescence warnings from `StacyClouds.C4Sharp.Client/Encryption/AesEncryptionStrategy.cs`

### Requirement: Public support matrix documentation includes .NET 11
User and contributor documentation MUST state .NET 11 support consistently wherever supported SDK/runtime versions are listed.

#### Scenario: Documentation support matrix is aligned with project targets
- **WHEN** maintainers review runtime support sections in top-level documentation files
- **THEN** the documented support matrix includes .NET 11 and does not contradict project target declarations
