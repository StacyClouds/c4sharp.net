## Purpose

Expose public, safe editing APIs for relationship connector geometry.

## Requirements

### Requirement: Create connector vertices publicly
The Core public API SHALL allow consumers to create a vertex with optional
coordinates or with an explicit X and Y coordinate.

#### Scenario: Create a positioned vertex
- **WHEN** a consumer creates a vertex with X and Y coordinates
- **THEN** the vertex exposes those coordinates for relationship layout

### Requirement: Edit ordered relationship vertices
The Core public API SHALL allow consumers to add, replace, remove, and clear
the ordered connector vertices of a relationship view.

#### Scenario: Add multiple connector vertices
- **WHEN** a consumer adds two vertices to a relationship view
- **THEN** the relationship view reports both vertices in the order added

#### Scenario: Replace connector vertices
- **WHEN** a consumer replaces a relationship view's vertices with a new
  ordered collection
- **THEN** the relationship view reports only the replacement vertices in that
  order

### Requirement: Protect relationship layout state
The relationship vertex editing API SHALL defensively copy supplied and
returned collections, and SHALL reject invalid null vertex arguments without
partially modifying the relationship layout.

#### Scenario: Caller mutates an input collection after replacement
- **WHEN** a consumer changes the collection used to replace connector vertices
- **THEN** the relationship view's stored vertices remain unchanged

### Requirement: Preserve layout serialization compatibility
Connector vertices edited through the public API SHALL retain their order and
coordinates through workspace serialization, deserialization, and layout-copy
operations.

#### Scenario: Round-trip an edited connector
- **WHEN** a workspace containing an edited relationship vertex list is
  serialized and deserialized
- **THEN** the hydrated relationship view has the same ordered vertex
  coordinates
