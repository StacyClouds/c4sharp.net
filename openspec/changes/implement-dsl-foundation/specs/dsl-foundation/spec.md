## ADDED Requirements

### Requirement: DSL workspace import boundary
The system MUST provide a DSL-shaped import boundary that can materialize workspace, model, and view structures into the existing C4Sharp.NET model.

#### Scenario: Import a workspace model
- **WHEN** a DSL workspace definition includes model and view sections
- **THEN** the system MUST create the corresponding workspace, model, and view objects

#### Scenario: Preserve supported view structures
- **WHEN** a DSL workspace defines supported view types
- **THEN** the system MUST map them to the existing C4Sharp.NET view model

### Requirement: DSL identifier handling
The system MUST preserve explicit identifiers from DSL input and MUST generate stable identifiers when DSL input omits them.

#### Scenario: Explicit identifier is present
- **WHEN** a DSL element defines an explicit identifier
- **THEN** the system MUST retain that identifier in the imported model

#### Scenario: Identifier is omitted
- **WHEN** a DSL element does not define an identifier
- **THEN** the system MUST assign a consistent identifier using the existing model strategy

### Requirement: DSL implied relationship handling
The system MUST honor DSL implied-relationship behavior during import by using the configured implied-relationship strategy.

#### Scenario: Implied relationships are enabled
- **WHEN** imported content implies a relationship that is not explicitly declared
- **THEN** the system MUST create the implied relationship when the configured strategy allows it

#### Scenario: Implied relationships are suppressed
- **WHEN** the configured strategy does not allow implied relationships for a relationship pattern
- **THEN** the system MUST leave the relationship unset
