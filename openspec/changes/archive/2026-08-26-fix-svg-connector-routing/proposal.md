## Why

Connector arrowheads currently terminate at element centres, obscuring their direction. Additionally, live movement of a box only updates un-routed connectors if every relationship is un-routed.

## What Changes

- Route visible connector endpoints to the edges of rendered element shapes while retaining centre-to-centre geometry for connector calculations.
- Ensure live box movement updates every directly connected relationship, regardless of whether other relationships contain vertices.
- Add regression tests for endpoint clipping and mixed routed/un-routed connector movement.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `svg-workspace-rendering`: Render connector endpoints at element boundaries and preserve live editing of mixed connector routes.

## Impact

- Updates renderer SVG geometry and editor static interaction code.
- Does not change the workspace public API or persistence model.
