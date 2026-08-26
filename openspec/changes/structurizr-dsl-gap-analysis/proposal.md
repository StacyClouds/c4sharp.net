## Why

C4Sharp.NET already covers the core C4 model and several view/documentation features, but it still lacks an explicit parity story for the Structurizr DSL surface described at docs.structurizr.com/dsl. We need a concrete gap analysis and a phased plan now so future work can be prioritized against the actual DSL feature set instead of being driven by ad hoc requests.

## What Changes

- Add a formal gap analysis document comparing current C4Sharp.NET capabilities to Structurizr DSL feature areas.
- Classify DSL areas as supported, partial, or missing based on current code and examples.
- Produce a prioritized remediation plan that closes the highest-value gaps first.
- Link the analysis and roadmap from existing project documentation.

## Capabilities

### New Capabilities
- `dsl-gap-analysis`: A documented comparison of current C4Sharp.NET coverage versus Structurizr DSL, with a prioritized plan to close gaps.

## Impact

- Affected docs: `docs/C4-DSL-GAP-ANALYSIS.md`, `ROADMAP.md`, and likely `README.md`/index links.
- Affected code inventory: current C4Sharp.NET core, client, examples, and tests will be referenced as the baseline for the gap matrix.
- Affected planning: future DSL-parity work will be broken into sequenced follow-up changes instead of one monolithic effort.
