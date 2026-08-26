## Context

Elements render at a fixed 150 by 70 rectangle or a 70-radius circle, but names are emitted as a single SVG text node.

## Goals / Non-Goals

**Goals:** Render centred multi-line labels that fit the default element dimensions.

**Non-Goals:** Measure browser fonts exactly or change stored model names.

## Decisions

- Split labels at word boundaries, splitting overlong words when needed.
- Emit up to three SVG `tspan` lines, truncating the final line with an ellipsis when necessary. This guarantees the label remains inside the default shape height.

## Risks / Trade-offs

- [Approximate character width] → Use a conservative line length and centred text.
