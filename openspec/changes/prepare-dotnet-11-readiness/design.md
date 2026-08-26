## Context

The maintained C4Sharp projects currently multi-target `net8.0;net9.0;net10.0` across library, tests, and examples. A .NET 11 SDK is available in the environment, but no C4Sharp project declares `net11.0`, so forced net11 builds fail due to missing assets targets. The C4Sharp client codebase also emits cryptography obsolescence warnings in `AesEncryptionStrategy` that represent likely forward-compatibility risk as frameworks evolve.

## Goals / Non-Goals

**Goals:**
- Add `net11.0` to the supported target framework matrix for all relevant projects.
- Establish an explicit readiness gate: restore/build/test succeed for net11-enabled projects.
- Reduce framework-forward compatibility risk by replacing obsolete PBKDF2 constructor usage with modern APIs.
- Update user and contributor docs to reflect the new support matrix.

**Non-Goals:**
- Introducing new runtime features that require .NET 11-only APIs.
- Dropping support for currently supported target frameworks.
- Broad warning-cleanup across unrelated legacy compiler warnings.

## Decisions

### 1. Keep multi-targeting and append `net11.0` rather than replacing existing TFMs
- **Decision:** Extend each project from `net8.0;net9.0;net10.0` to `net8.0;net9.0;net10.0;net11.0`.
- **Rationale:** Preserves compatibility expectations for existing consumers while enabling early validation on .NET 11.
- **Alternative considered:** Retarget only latest runtime.
  - **Rejected because:** Would be a breaking support contraction with little short-term benefit.

### 2. Treat readiness as a repository-wide contract, not just package compile success
- **Decision:** Require restore/build/test coverage for net11-enabled project types (core libs, client, tests, examples).
- **Rationale:** .NET readiness is incomplete if tests/examples drift from runtime behavior.
- **Alternative considered:** Validate only library projects.
  - **Rejected because:** Misses regressions in test and example surfaces that consumers rely on.

### 3. Replace obsolete PBKDF2 constructor usage in AES encryption strategy
- **Decision:** Migrate from obsolete `Rfc2898DeriveBytes` constructors to modern PBKDF2 API shape.
- **Rationale:** Existing SYSLIB warnings already indicate insecure/default-obsolete paths; modernization reduces future break risk and improves cryptographic clarity.
- **Alternative considered:** Keep legacy constructors with warning suppressions.
  - **Rejected because:** Defers risk and normalizes security-related technical debt.

### 4. Update docs and contribution guidance in lockstep with target changes
- **Decision:** Update support matrix language and command examples where runtime versions are listed.
- **Rationale:** Prevents mismatch between declared and actual support.
- **Alternative considered:** Defer doc updates until GA.
  - **Rejected because:** Creates immediate contributor confusion and inconsistent onboarding.

## Risks / Trade-offs

- **[Dependency/tooling incompatibility with net11 previews]** → Mitigate by validating with current SDK in CI/local and documenting preview caveat until GA.
- **[Crypto behavior drift during PBKDF2 API migration]** → Mitigate with compatibility-focused tests that verify encrypt/decrypt roundtrip and deterministic key derivation constraints where applicable.
- **[Increased CI time due to additional TFM]** → Mitigate by keeping targeted validation commands and avoiding redundant matrix duplication.
- **[Warning noise obscuring true regressions]** → Mitigate by scoping this change to net11-critical warnings and deferring unrelated warning cleanup.

## Migration Plan

1. Add `net11.0` to target framework declarations across project files.
2. Update cryptography implementation to modern PBKDF2 API and keep behavior-compatible outcomes.
3. Run restore/build/test on the expanded framework set.
4. Update README/CONTRIBUTING support statements and developer prerequisites.
5. Rollback path: remove `net11.0` from target frameworks and revert crypto migration if an unrecoverable compatibility issue is discovered.

## Open Questions

- Should CI fully enforce net11 gates immediately while the SDK remains preview, or should enforcement begin at GA?
- Are any downstream package dependencies expected to introduce net11-specific constraints before GA?
