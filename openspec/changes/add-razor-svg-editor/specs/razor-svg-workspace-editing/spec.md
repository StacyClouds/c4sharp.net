## ADDED Requirements

### Requirement: Provide an optional Razor editor package
The system SHALL provide `StacyClouds.C4Sharp.Editor` as a separately packable Razor class library targeting every framework supported by the renderer. The package SHALL depend on the renderer package and SHALL not require the renderer package to reference Razor or ASP.NET Core.

#### Scenario: Consumer uses only the renderer package
- **WHEN** an application references `StacyClouds.C4Sharp.Renderer` but not `StacyClouds.C4Sharp.Editor`
- **THEN** it can render workspace SVG documents without Razor component dependencies or editor static assets

### Requirement: Navigate workspace views
The workspace editor component SHALL accept a workspace and SHALL provide a collapsible side panel containing a thumbnail for every rendered workspace view. Selecting a thumbnail SHALL load that view into the interactive editor surface. The component SHALL accept an optional initial view key.

#### Scenario: User selects a workspace view
- **WHEN** a user selects a thumbnail in the workspace editor navigator
- **THEN** the component displays the renderer's SVG document for that view in the active editing surface

#### Scenario: Consumer embeds a single view editor
- **WHEN** a consumer supplies a workspace and view key to the focused view editor component
- **THEN** the component displays the renderer's SVG document for that view without a workspace navigator

### Requirement: Persist element drag positions
The editor component SHALL update the matching `ElementView.X` and `ElementView.Y` values in the supplied workspace after an element drag completes, and SHALL raise its layout-changed callback. It SHALL visually move the element during the drag before persisting the final position.

#### Scenario: User moves an element
- **WHEN** a user drags an element to a new SVG coordinate and releases it
- **THEN** the element view in the supplied workspace stores that coordinate and the component rerenders the SVG

#### Scenario: User drags an initially auto-laid-out view
- **WHEN** a user moves one element in a view whose elements use the renderer's deterministic layout
- **THEN** every untouched element remains at its deterministic layout position

### Requirement: Insert connector vertices from double-clicks
The editor component SHALL add a `Vertex` to the relationship view that a user double-clicks. It SHALL insert the vertex at the location that preserves the order of the existing connector path and SHALL raise its layout-changed callback.

#### Scenario: User double-clicks a routed connector
- **WHEN** a user double-clicks the second segment of a relationship containing an existing connector vertex
- **THEN** the workspace relationship view contains the new vertex after the existing vertex and before the destination

### Requirement: Delegate workspace persistence to the host
The workspace editor SHALL raise a save-requested callback when its user invokes Save. It SHALL not write the workspace to disk, a database, or a remote service itself.

#### Scenario: User saves edited layout
- **WHEN** a user invokes Save after changing a workspace layout
- **THEN** the component raises its save-requested callback with the updated workspace

## MODIFIED Requirements

### Requirement: Render meaningful styled SVG
The renderer SHALL produce well-formed SVG with XML-escaped content, view title information, elements, relationship arrows, and labels. It SHALL apply configured element and relationship styles and use documented defaults for unspecified style values. It SHALL include additive, XML-escaped `data-c4-view-key`, `data-c4-element-id`, and `data-c4-relationship-id` attributes that identify the source workspace objects for interactive consumers.

#### Scenario: Label contains XML-special characters
- **WHEN** an element or relationship label includes an ampersand or angle bracket
- **THEN** the generated SVG is well-formed and displays the original label

#### Scenario: Interactive consumer identifies rendered objects
- **WHEN** the renderer outputs a workspace view containing elements and relationships
- **THEN** the SVG identifies its view, every element, and every relationship with the corresponding workspace IDs
