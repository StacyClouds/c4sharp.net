## ADDED Requirements

### Requirement: DSL gap analysis covers documented feature areas
The repository MUST include a gap analysis that compares current C4Sharp.NET capabilities against the Structurizr DSL feature areas documented at docs.structurizr.com/dsl.

#### Scenario: Maintainer reviews the analysis document
- **WHEN** a maintainer opens the gap analysis
- **THEN** the document covers the major DSL areas, including workspace, model, views, styles, documentation, includes, scripts, expressions, identifiers, themes, patterns, plugins, and workspace extension

### Requirement: Gap analysis classifies support status with evidence
The gap analysis MUST classify each feature area as supported, partial, or missing and cite the relevant current code or examples that justify the classification.

#### Scenario: Maintainer checks a feature row
- **WHEN** a maintainer inspects a feature area in the analysis matrix
- **THEN** the row shows the support status and includes the code or example evidence used for that decision

### Requirement: Gap analysis includes a phased closure plan
The repository MUST include a plan that prioritizes DSL gaps into phased follow-up work based on user value and implementation dependency.

#### Scenario: Maintainer reads the roadmap section
- **WHEN** a maintainer reviews the closure plan
- **THEN** the plan identifies ordered phases, the gaps in each phase, and the dependency between phases

### Requirement: Gap analysis links to follow-on work items
The gap analysis MUST identify follow-on work items that can be turned into future implementation changes.

#### Scenario: Maintainer converts analysis into backlog
- **WHEN** a maintainer uses the analysis to create future changes
- **THEN** each major gap is mapped to a concrete follow-on work item with an expected outcome

### Requirement: Project documentation surfaces the DSL gap analysis
The repository MUST link the gap analysis and closure plan from the main project documentation.

#### Scenario: New contributor looks for DSL parity work
- **WHEN** a new contributor reads the README or roadmap
- **THEN** they can find the DSL gap analysis and the plan to close the gaps without searching the repository
