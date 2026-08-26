## Context

The renderer represents relationships as SVG polylines whose first and last coordinates are element centres. That is useful for layout and label calculations, but leaves arrowheads hidden inside rectangles or circles. The editor's live endpoint mutation also selects every relationship-marked SVG object, including handle circles, which makes un-routed connectors fail when any routed connector is present.

## Goals / Non-Goals

**Goals:**

- Clip visible connector endpoints to the source and destination shape edges.
- Retain a centre-to-centre invisible geometry path for interactive calculations.
- Restrict live connector endpoint updates to relationship polylines.

**Non-Goals:**

- Change the workspace data model or relationship routing API.
- Implement shape intersections beyond the renderer's rectangle and circle shapes.

## Decisions

- Render a centre-to-centre invisible polyline for interaction geometry and a visible polyline with endpoints clipped to the source and destination shape boundaries. This retains stable editor calculations while making direction visible.
- Use SVG `polyline` selectors for live connector updates. Vertex handle circles share relationship identifiers but are not connector routes.
- Calculate a rectangle boundary intersection using the element centre, half-width, and half-height; use a radial intersection for circles.

## Risks / Trade-offs

- [A connector overlaps an element after a large movement] → Endpoint clipping is recalculated on the persisted rerender.
- [A relationship has zero-length geometry] → Keep the centre point rather than attempting a divide-by-zero boundary intersection.
