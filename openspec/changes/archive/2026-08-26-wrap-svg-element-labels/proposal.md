## Why

Long element names overflow their SVG shapes, reducing diagram readability.

## What Changes

- Wrap element names into centred SVG text lines that remain inside the rendered element shape.
- Add regression coverage for whitespace and unbroken long names.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `svg-workspace-rendering`: Render readable element labels within element boundaries.

## Impact

- Updates SVG text output only; no workspace model or public API changes.
