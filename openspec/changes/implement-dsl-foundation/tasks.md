## 1. TDD

- [x] 1.1 Add failing tests for DSL workspace, model, and view import behavior.
- [x] 1.2 Add failing tests for explicit identifier preservation and generated identifier fallback.
- [x] 1.3 Add failing tests for implied-relationship creation and suppression.

## 2. Check the tests

- [x] 2.1 Review the existing test fixtures and DSL-adjacent coverage to avoid duplication.
- [x] 2.2 Confirm the targeted tests fail before implementation starts.

## 3. Code

- [x] 3.1 Add the DSL import entry point for workspace, model, and view structures.
- [x] 3.2 Map supported DSL workspace and view constructs into existing C4Sharp.NET model types.
- [x] 3.3 Preserve explicit DSL identifiers and wire omitted identifiers through the existing generator.
- [x] 3.4 Apply the configured implied-relationship strategy during DSL import.

## 4. Test and Stryker

- [x] 4.1 Run the targeted test suite and confirm the new behavior passes.
- [x] 4.2 Run Stryker against the DSL foundation scope.
- [x] 4.3 Tune implementation or tests if mutation coverage reveals a gap.

## 5. Documentation

- [x] 5.1 Update the DSL gap analysis and user-facing docs to reflect the foundation capability.
- [x] 5.2 Update the README or roadmap links if the new surface needs discoverability.

## 6. Architecture

- [x] 6.1 Reconcile the implementation with the existing API-first model and import boundary design.
- [x] 6.2 Capture any architecture follow-up items needed for the next DSL phase.

## 7. Hondo review

- [x] 7.1 Run the Hondo review step on the completed change.
- [x] 7.2 Address any review feedback that requires follow-up work.
