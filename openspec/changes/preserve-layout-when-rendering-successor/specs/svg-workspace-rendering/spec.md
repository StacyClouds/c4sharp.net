## ADDED Requirements

### Requirement: Render a successor with predecessor layout
The renderer SHALL provide an overload that accepts an updated successor
workspace and a predecessor workspace. Before producing SVG, it SHALL copy
reusable layout from matching predecessor views into matching successor views.
It SHALL mutate only the successor and SHALL leave the predecessor unchanged.

#### Scenario: Matching objects retain saved layout
- **WHEN** matching successor and predecessor views contain a matching element
  and relationship with saved predecessor geometry
- **THEN** the successor receives the element coordinates, relationship
  vertices, routing, label position, and unspecified dimensions before SVG is
  rendered

#### Scenario: Successor removes predecessor objects
- **WHEN** a predecessor contains a view, element, or relationship absent from
  the successor
- **THEN** rendering does not restore that object in the successor or SVG

#### Scenario: Successor introduces a new object
- **WHEN** a successor-only element has default stored coordinates
- **THEN** it remains unpersisted and renders using the deterministic fallback

#### Scenario: No successor view matches
- **WHEN** the predecessor has no view with a successor view key
- **THEN** rendering produces the ordinary deterministic successor SVG

#### Scenario: An input is null
- **WHEN** either workspace argument is null
- **THEN** the overload throws `ArgumentNullException` with the relevant
  parameter name
