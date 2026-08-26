## MODIFIED Requirements

### Requirement: Render meaningful styled SVG
The renderer SHALL produce well-formed SVG with XML-escaped content, view title information, elements, relationship arrows, and labels. It SHALL apply configured element and relationship styles and use documented defaults for unspecified style values. It SHALL include additive, XML-escaped `data-c4-view-key`, `data-c4-element-id`, and `data-c4-relationship-id` attributes that identify the source workspace objects for interactive consumers. Visible relationship arrows SHALL start and end at the boundaries of their source and destination element shapes, while preserving centre-based interaction geometry for optional editor consumers.

#### Scenario: Label contains XML-special characters
- **WHEN** an element or relationship label includes an ampersand or angle bracket
- **THEN** the generated SVG is well-formed and displays the original label

#### Scenario: Interactive consumer identifies rendered objects
- **WHEN** the renderer outputs a workspace view containing elements and relationships
- **THEN** the SVG identifies its view, every element, and every relationship with the corresponding workspace IDs

#### Scenario: Visible relationship arrow meets an element edge
- **WHEN** a relationship connects two rectangular or circular elements
- **THEN** its visible arrowhead terminates at the destination shape boundary rather than its centre

## ADDED Requirements

### Requirement: Update every live connector during mixed routing edits
The editor SHALL update the endpoint of every directly connected visible relationship polyline while an element is dragged, regardless of whether other relationships have connector vertices.

#### Scenario: A view contains routed and direct relationships
- **WHEN** a user drags an element connected by a direct relationship in a view that also contains a routed relationship
- **THEN** the direct relationship endpoint moves with the element before release
