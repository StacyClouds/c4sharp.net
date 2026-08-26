## Context

The Core `ViewSet.CopyLayoutInformationFrom` API already matches views by key
and delegates element and relationship matching to the established layout merge
strategy. It copies dimensions only when the destination has none and preserves
complete relationship layout, including vertices, routing, and label position.

## Decision

Add `SvgWorkspaceRenderer.Render(Workspace workspace, Workspace predecessor)`.
It validates both inputs, invokes
`workspace.Views.CopyLayoutInformationFrom(predecessor.Views)` once, then
delegates to the existing one-workspace renderer.

## Consequences

The successor is intentionally mutated so applications can save the reused
layout. The predecessor is not mutated. Since copying targets only collections
already declared by the successor, predecessor-only views, elements, and
relationships cannot be restored. New successor objects retain default stored
coordinates and use the existing deterministic rendering fallback.

No renderer-local merge algorithm is introduced: Core owns matching rules and
layout-copy semantics.
