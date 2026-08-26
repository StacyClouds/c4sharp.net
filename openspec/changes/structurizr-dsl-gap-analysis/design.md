## Context

C4Sharp.NET is an API-first C4 model library. The current codebase already covers foundational model and view primitives, including system/container/component/deployment views, filtered views, styles, themes, documentation helpers, and encrypted workspace transport. However, it does not yet have a single authoritative comparison against the Structurizr DSL feature surface, and there is no published plan that ranks the missing DSL areas by value and dependency.

## Goals / Non-Goals

**Goals:**
- Produce a trustworthy DSL-vs-code gap matrix rooted in the current repository.
- Classify each DSL area as supported, partial, or missing with traceable evidence.
- Turn the matrix into a phased roadmap that can drive follow-up implementation changes.
- Surface the output from the main project documentation so contributors can find it quickly.

**Non-Goals:**
- Implementing a DSL parser or import/export engine in this change.
- Closing every DSL gap immediately.
- Rewriting the current API-first programming model around the DSL.

## Decisions

### 1. Use a document-first gap matrix
- **Decision:** Make the gap analysis itself the source of truth, with one row per DSL feature area and explicit support status.
- **Rationale:** The DSL surface is broad; a matrix is easier to review, maintain, and turn into backlog items than prose-only notes.
- **Alternative considered:** A narrative-only comparison.
  - **Rejected because:** It hides traceability and makes prioritization harder.

### 2. Anchor the comparison to current code and examples
- **Decision:** Classify support based on concrete code paths and examples in this repository.
- **Rationale:** The current code already demonstrates support for many DSL-adjacent areas; the gap analysis should reward existing coverage rather than starting from a blank slate.
- **Alternative considered:** Compare only against the DSL docs without code evidence.
  - **Rejected because:** It would overstate gaps and miss existing parity.

### 3. Prioritize gaps by dependency and user value
- **Decision:** Split closure work into phases, starting with foundation features and moving toward advanced directives/plugins.
- **Rationale:** DSL parity is too large to tackle in one pass. A phased plan gives a realistic path to close gaps incrementally.
- **Alternative considered:** A single flat backlog list.
  - **Rejected because:** It obscures prerequisites and sequencing.

### 4. Keep the deliverable documentation-centric
- **Decision:** This change updates docs and roadmap artifacts first, not runtime behavior.
- **Rationale:** The current user request is for analysis and a plan, not immediate feature implementation.
- **Alternative considered:** Convert the analysis directly into implementation work.
  - **Rejected because:** It would skip the needed planning layer.

## Risks / Trade-offs

- **[DSL docs evolve over time]** → Mitigation: cite the docs version/date in the analysis and keep the matrix easy to refresh.
- **[Support classifications are subjective at the margins]** → Mitigation: require code evidence for every supported/partial row and call out assumptions explicitly.
- **[Roadmap can become overcommitted]** → Mitigation: limit the first-phase plan to the highest-value, dependency-heavy gaps only.
- **[Docs may diverge from code]** → Mitigation: link the analysis from README/ROADMAP and make it part of the review checklist for future DSL work.

## Migration Plan

1. Publish the DSL gap analysis document.
2. Update the roadmap and main documentation links.
3. Derive follow-on OpenSpec changes from the phased roadmap.
4. Revisit the matrix whenever new DSL-related functionality lands.

## Open Questions

- Should the first implementation phase focus on DSL parsing/import/export, or on modeling the DSL directives against the existing API surface first?
- Do we want to maintain a strict “feature parity” target, or a narrower compatibility target centered on the most common DSL workflows?
- Should plugin/pattern support be treated as core parity or a later ecosystem phase?
