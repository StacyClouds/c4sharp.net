## ADDED Requirements

### Requirement: Render every workspace view
The renderer SHALL produce one standalone SVG document for every static,
dynamic, deployment, and filtered view in a workspace, identified by the
view's unique key.

#### Scenario: Workspace contains multiple view types
- **WHEN** a workspace contains system landscape, system context, container,
  component, dynamic, deployment, and filtered views
- **THEN** rendering the workspace returns one SVG document for each view key

### Requirement: Preserve persisted view geometry
The renderer SHALL use persisted element coordinates, relationship vertices,
routing, label position, and view dimensions when they are present, and SHALL
not mutate the source workspace.

#### Scenario: Relationship includes bend points
- **WHEN** a relationship view has ordered connector vertices and orthogonal
  routing
- **THEN** the SVG path traverses those vertices in order without changing the
  relationship view

### Requirement: Render filtered views from their base geometry
The renderer SHALL render a filtered view using its base view geometry and
SHALL include or exclude elements according to the filtered view's mode and
tags. It SHALL omit relationships whose source or destination is omitted.

#### Scenario: Excluded element participates in a relationship
- **WHEN** an exclude-mode filtered view removes a tagged destination element
- **THEN** the SVG excludes both that element and any relationship to it

### Requirement: Render meaningful styled SVG
The renderer SHALL produce well-formed SVG with XML-escaped content, view
title information, elements, relationship arrows, and labels. It SHALL apply
configured element and relationship styles and use documented defaults for
unspecified style values.

#### Scenario: Label contains XML-special characters
- **WHEN** an element or relationship label includes an ampersand or angle
  bracket
- **THEN** the generated SVG is well-formed and displays the original label

### Requirement: Handle unlaid-out views deterministically
The renderer SHALL render a legible, deterministic fallback layout when a view
does not contain meaningful element coordinates, without persisting fallback
coordinates to the workspace.

#### Scenario: Newly created view has default coordinates
- **WHEN** every element in a view has default coordinates and the view has no
  dimensions
- **THEN** repeated renders produce equivalent SVG with distinct readable
  element positions and a derived viewport
