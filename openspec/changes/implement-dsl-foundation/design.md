## Context

C4Sharp.NET already models workspaces, views, identifiers, and implied-relationship strategies, but those capabilities are exposed through the API rather than a DSL import boundary. The gap analysis identified the foundation phase as the highest-leverage first step because it creates the substrate for later includes, scripts, expressions, and ecosystem parity work.

## Goals / Non-Goals

**Goals:**
- Add a DSL import boundary for workspace, model, and view structures.
- Preserve explicit DSL identifiers and integrate with existing ID generation behavior.
- Respect configured implied-relationship strategy during import.
- Keep the current API-first model intact.

**Non-Goals:**
- Full parity with every Structurizr DSL directive.
- Plugin, script, include, workspace-extension, or archetype support.
- Changes to the underlying C4 model semantics beyond import behavior.

## Decisions

- Use an import-layer design rather than replacing the existing object model. This minimizes risk and lets the API remain the source of truth.
  - Alternative considered: a new DSL-only domain model. Rejected because it would duplicate the existing model and make round-tripping harder.
- Model DSL input as a dedicated boundary that maps into current workspace/model/view types.
  - Alternative considered: direct parser-to-model mutation in one pass. Rejected because it couples syntax handling to persistence and makes testing harder.
- Reuse the existing identifier generator and implied-relationship strategy seams instead of introducing new policy objects.
  - Alternative considered: DSL-specific strategy abstractions. Rejected because the current seams already cover the needed behavior and should remain canonical.

## Risks / Trade-offs

- [Parser scope creep] → Start with the foundation-only surface and defer advanced directives to later changes.
- [Identifier mismatch] → Keep explicit IDs authoritative and use existing generation only when input omits IDs.
- [Relationship over-generation] → Gate implied-relationship creation through the current strategy configuration and add focused tests around suppressed cases.
- [Round-trip gaps] → Document unsupported DSL features explicitly so later phases can extend the boundary without changing the base contract.

## Migration Plan

Introduce the import boundary behind the existing workspace/model APIs so current consumers continue to function unchanged. If the implementation needs to change import internals, keep the old API entry points stable and add rollback by retaining the prior code path until the new boundary is validated.

## Open Questions

- Which unsupported DSL directives should be explicitly rejected versus ignored in phase 1?
- Do we need a separate compatibility mode for legacy identifier behavior?

## Implementation Notes

- The first implementation uses a structured DSL-shaped import payload instead of a raw text parser.
- The import boundary preserves explicit identifiers and generates deterministic fallback identifiers for omitted values.
- DSL imports honor the configured implied-relationship strategy through the existing model seam.
