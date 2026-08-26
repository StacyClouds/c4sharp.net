## 1. Core relationship layout API

- [x] 1.1 Add failing Core tests for public vertex construction; ordered add, replace, remove, and clear operations; defensive copies; invalid inputs; serialization round-trips; and layout copying.
- [x] 1.2 Make vertex construction available to Core consumers while preserving data-contract serialization behavior.
- [x] 1.3 Add defensive public operations to add, replace, remove, and clear a relationship view's ordered connector vertices.
- [x] 1.4 Run the focused Core tests and make this group green before starting renderer project work.

## 2. Renderer project setup

- [x] 2.1 Add failing renderer API tests that define workspace rendering results keyed by unique view keys.
- [x] 2.2 Create the multi-targeted `StacyClouds.C4Sharp.Renderer` class library and its test project, and add both to the maintained solution.
- [x] 2.3 Define the public workspace rendering API and result model for SVG documents keyed by unique view keys.
- [x] 2.4 Run the renderer API tests and make this group green before implementing view normalization.

## 3. View normalization and layout

- [x] 3.1 Add failing renderer tests for stable enumeration of every view type, filtered-view tag semantics, unresolved base views, persisted coordinates, dimensions, and fallback viewport/layout behavior.
- [x] 3.2 Enumerate static, dynamic, deployment, and filtered views in a stable order.
- [x] 3.3 Normalize view elements and relationships, including filtered-view tag semantics and clear errors for unresolved base views.
- [x] 3.4 Resolve dimensions, persisted coordinates, viewport bounds, and a non-mutating deterministic fallback grid for unlaid-out views.
- [x] 3.5 Run the normalization and layout tests and make this group green before generating SVG primitives.

## 4. SVG generation

- [x] 4.1 Add failing SVG tests for XML-safe output, titles, dimensions, supported element shapes, labels, resolved styles, arrows, routing, ordered connector vertices, label placement, and dynamic ordering.
- [x] 4.2 Resolve element and relationship styles from tags with documented renderer defaults.
- [x] 4.3 Generate well-formed, XML-safe SVG documents with view titles, dimensions, and reusable arrow markers.
- [x] 4.4 Render supported element shapes, names, descriptions, and metadata using resolved styles.
- [x] 4.5 Render relationship paths through ordered connector vertices, honoring routing, arrows, label position, descriptions, and dynamic ordering.
- [x] 4.6 Run the SVG-generation tests and make this group green before examples and documentation.

## 5. Tests, examples, and documentation

- [x] 5.1 Add failing acceptance tests for the documented renderer example and public connector-editing workflow.
- [x] 5.2 Add a renderer example that writes the SVG documents for a workspace and document the renderer and connector-editing APIs.
- [x] 5.3 Run the example and acceptance tests and make this group green.
- [x] 5.4 Run focused Core and renderer tests, then restore, build, and test the maintained solution across supported target frameworks; resolve failures before handoff.
