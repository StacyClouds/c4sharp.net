# C4 DSL Gap Analysis

Baseline: [Structurizr DSL](https://docs.structurizr.com/dsl) compared with the current C4Sharp.NET codebase.

## Summary

C4Sharp.NET already covers a strong core of the C4 model API: model elements, relationships, the major view types, styles, themes, animations, filtered views, deployment nodes, and documentation composition. The foundation phase now also has a structured DSL-shaped import boundary. The main remaining gaps are the DSL-specific text surface and its directive ecosystem: includes, scripts, expressions, plugins, workspace extension, and a reusable pattern/archetype layer.

## Feature Matrix

| DSL area | Status | Current C4Sharp.NET evidence | Gap / note |
|---|---|---|---|
| Workspace / model | Supported | `Workspace`, `Model`, `JsonReader`, `JsonWriter`, `ViewSet`, `ViewConfiguration` | Core model serialization and view configuration already exist. |
| DSL workspace import boundary | Partial | `StacyClouds.C4Sharp.Dsl.DslWorkspaceImporter`, `DslWorkspace`, `DslModel`, `DslViews` | Foundation import boundary exists for structured workspace/model/view inputs; DSL text parsing and directives remain future work. |
| Views (system/context/container/component/dynamic/deployment/filtered) | Supported | `ViewSet.CreateSystemLandscapeView`, `CreateSystemContextView`, `CreateContainerView`, `CreateComponentView`, `CreateDynamicView`, `CreateDeploymentView`, `CreateFilteredView` | Major DSL view types are already represented in the API. |
| Styles / themes | Supported | `ViewConfiguration.Styles`, `ViewConfiguration.Theme(s)`, `Styles`, `ElementStyle`, `RelationshipStyle` | Theme URLs and styling are supported, though not via a DSL text parser. |
| Animations | Supported | `StaticView.AddAnimation(...)` and the example programs in `StacyClouds.C4Sharp.Examples/` | Animation steps are modeled directly in the API. |
| Documentation sections | Partial | `StructurizrDocumentationTemplate`, `AddContextSection`, `AddDeploymentSection`, etc. | Documentation is supported as file-based composition, not as DSL text directives. |
| Implied relationships | Partial | `Model.ImpliedRelationshipsStrategy`, `DefaultImpliedRelationshipsStrategy`, `CreateImpliedRelationshipsUnlessAnyRelationshipExistsStrategy`, `CreateImpliedRelationshipsUnlessSameRelationshipExistsStrategy` | The strategy seam exists, but there is no DSL parser/directive layer. |
| Deployment groups / deployment modeling | Partial | `DeploymentNode.Add(...)`, `ContainerInstance.DeploymentGroup`, `SoftwareSystemInstance.DeploymentGroup`, examples in `BigBankPlc.cs` and `HttpHealthChecks.cs` | The model supports deployment groups and nested deployment nodes; DSL syntax and import/export parity are still missing. |
| Identifiers | Partial | `IdGenerator`, `SequentialIntegerIdGeneratorStrategy`, `Model.IdGenerator` | There is a generator seam, but not the DSL’s identifier policies/directives. |
| Groups | Missing | No grouping API or DSL grouping surface found in the codebase | DSL group semantics are not represented today. |
| Includes | Missing | No DSL include/import layer found in core, client, or examples | No parser or include resolution exists today. |
| Scripts | Missing | No DSL script execution surface found | No `!script`-style support or execution model exists. |
| Expressions | Missing | No expression parser/evaluator found | No support for DSL expressions/templating. |
| Archetypes / patterns | Missing | Only hand-built examples and samples exist | No reusable DSL pattern layer or archetype catalog exists. |
| Plugins | Missing | No plugin extension surface found | No Mermaid/PlantUML-style DSL plugin system exists. |
| Workspace extension | Missing | No workspace extension loader found | No DSL workspace extension mechanism exists. |
| ADRs | Missing | Documentation examples exist, but no DSL ADR directive/import model | No DSL ADR feature exists. |

## Evidence Notes

- Core model and view support is visible in `StacyClouds.C4Sharp.Core/View/ViewSet.cs`, `ViewConfiguration.cs`, `StaticView.cs`, `Model.cs`, `DeploymentNode.cs`, and the example programs under `StacyClouds.C4Sharp.Examples/`.
- Documentation composition is visible in `StacyClouds.C4Sharp.Examples/StructurizrDocumentationExample.cs` and `FinancialRiskSystem.cs`.
- Custom ID generation and implied-relationships strategy seams are visible in `StacyClouds.C4Sharp.Core/Model/IdGenerator.cs`, `SequentialIntegerIdGeneratorStrategy.cs`, `ImpliedRelationshipsStrategy.cs`, and `DefaultImpliedRelationshipsStrategy.cs`.

## Phased Closure Plan

### Phase 1 — DSL foundation
- Add a DSL import/export boundary that can represent workspace/model/view structures.
- Close identifier semantics and implied-relationships parity where the DSL requires it.
- Preserve the current API-first model while enabling DSL-shaped inputs.

### Phase 2 — DSL directives and composition
- Add include resolution.
- Add script/expression support.
- Add better documentation-text composition so DSL docs can round-trip more naturally.

### Phase 3 — Ecosystem parity
- Add groups, archetypes/pattern support, and workspace extension support.
- Add plugin support (for example Mermaid/PlantUML integrations).
- Add ADR-style document handling if needed for parity.

## Recommended First Follow-up Change

Start with a DSL parser/import layer for the foundation areas:
1. workspace
2. model
3. views
4. identifiers
5. implied relationships

That gives the highest leverage because it unlocks the remaining directive ecosystem and provides the base for future compatibility work.

## Current Foundation Status

- `StacyClouds.C4Sharp.Dsl.DslWorkspaceImporter` now covers structured workspace/model/view imports.
- Explicit identifiers are preserved and missing identifiers are generated deterministically.
- Imported relationships still honor the configured implied-relationship strategy.
