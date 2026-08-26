## Why

C4Sharp.NET already covers most of the underlying C4 model, but it still lacks a DSL-shaped import boundary for the foundation features that users expect from Structurizr DSL. Closing that gap now unlocks a path to round-trip workspace definitions, align identifier behavior, and preserve implied-relationship semantics without forcing users to stay in the API-first model.

## What Changes

- Add a DSL import boundary for workspace, model, and view structures.
- Add DSL-aware identifier handling so imported workspaces can preserve or regenerate IDs consistently.
- Add DSL-aware implied-relationship handling so imported workspaces follow the intended relationship strategy.
- Preserve the existing API-first model while enabling DSL-shaped inputs.
- **BREAKING**: none expected; this change should be additive.

## Capabilities

### New Capabilities
- `dsl-foundation`: Import and represent Structurizr DSL workspace/model/view structures, with identifier and implied-relationship semantics.

### Modified Capabilities
- None

## Impact

Affected areas include the core model, view serialization/import flow, identifier generation, implied-relationships strategy handling, and related examples/tests. This may also introduce a new DSL import surface and supporting docs for the first phase of parity work.
