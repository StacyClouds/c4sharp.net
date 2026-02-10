# C4 DSL Gap Analysis

This document identifies features in the [Structurizr DSL](https://docs.structurizr.com/dsl) that are missing or different in C4Sharp.NET.

## Analysis Date
February 2026

## Methodology
This analysis compares C4Sharp.NET (formerly Structurizr for .NET) against the current Structurizr DSL specification and identifies gaps.

## Feature Comparison

### ✅ Fully Supported Features

- **Core Model Elements**
  - Person
  - Software System
  - Container
  - Component
  - Relationships
  - Tags
  - Properties

- **Views**
  - System Context Diagram
  - Container Diagram
  - Component Diagram
  - Dynamic Diagram
  - Deployment Diagram
  - System Landscape Diagram
  - Filtered Views

- **Styling**
  - Element styles (background, color, shape, icon, etc.)
  - Relationship styles (color, thickness, dashed, etc.)
  - Themes support
  - Corporate branding

- **Documentation**
  - Markdown and AsciiDoc support
  - Documentation sections
  - Architecture Decision Records (ADRs)
  - Multiple documentation formats (Structurizr, arc42, Viewpoints & Perspectives)
  - Images and diagrams

### ⚠️ Partially Supported Features

- **Workspace Features**
  - ✅ Basic workspace configuration
  - ✅ User/role management
  - ❌ Workspace extends (DSL only)
  - ❌ Include files (DSL only)
  - ❌ Variables and constants (DSL only)

- **Model Features**
  - ✅ Basic enterprise boundary
  - ❌ Group elements
  - ❌ Model identifiers for referencing
  - ❌ Implied relationships configuration

### ❌ Missing Features

1. **DSL-Specific Features** (Not applicable to code-based API)
   - Script/file-based definition
   - Include/import statements
   - Variable substitution
   - Expressions and calculations

2. **Recent DSL Additions** (Need investigation)
   - Custom elements and relationships
   - Element metadata
   - Relationship perspectives
   - Auto-layout customization
   - Animation steps (beyond basic dynamic views)

3. **Advanced Styling**
   - Workspace-level themes from URLs
   - Element border styles
   - Relationship positions
   - Custom diagram keys

4. **Deployment Features**
   - Infrastructure nodes groups
   - Deployment environments beyond basic support
   - Container instances with custom properties

5. **View Features**
   - Image views
   - Enterprise context diagrams
   - Custom diagrams beyond standard C4 types
   - View animation steps

## Priority Assessment

### High Priority Gaps

These features would significantly enhance C4Sharp.NET:

1. **Group Elements Support**
   - Common requirement for organizing complex diagrams
   - Present in DSL, missing in .NET
   - Relatively straightforward to implement

2. **Implied Relationships Configuration**
   - Control automatic relationship creation
   - Important for large models
   - Moderate implementation complexity

3. **Enhanced Deployment Support**
   - Better infrastructure modeling
   - Container instances with properties
   - Deployment environment grouping

### Medium Priority Gaps

Useful but not critical:

1. **Animation Steps**
   - Enhanced dynamic views
   - Better storytelling in diagrams
   - Complex implementation

2. **Custom Element Properties**
   - Extensibility for domain-specific needs
   - Metadata attachment
   - Moderate complexity

3. **Advanced Styling Options**
   - More granular control
   - Border customization
   - Element positioning hints

### Low Priority Gaps

Nice-to-have features:

1. **Image Views**
   - Supplementary documentation
   - Low complexity

2. **Custom View Types**
   - Beyond standard C4
   - Complex implementation
   - Limited use cases

## Recommendations

1. **Maintain Core C4 Compatibility**
   - Focus on standard C4 model elements
   - Ensure compatibility with Structurizr visualization

2. **Prioritize Common Use Cases**
   - Group elements (high demand)
   - Better deployment modeling
   - Relationship control

3. **Document DSL vs. Code Differences**
   - Clear documentation of what's different
   - Migration guide from DSL to code
   - When to use each approach

4. **Consider Future Enhancements**
   - Track DSL evolution
   - Evaluate new features for inclusion
   - Community feedback on priorities

## Implementation Plan

### Phase 1: Group Elements
- Design API for grouping
- Implement in model
- Update serialization
- Add tests
- Update documentation

### Phase 2: Implied Relationships
- Add configuration options
- Implement relationship rules
- Update model hydration
- Add tests

### Phase 3: Enhanced Deployment
- Extend deployment model
- Add infrastructure grouping
- Container instance properties
- Update views

### Phase 4: Advanced Features
- Animation steps
- Custom properties
- Advanced styling

## Resources

- [Structurizr DSL Reference](https://docs.structurizr.com/dsl)
- [C4 Model](https://c4model.com)
- [Structurizr Cloud](https://structurizr.com)
- [C4Sharp.NET Repository](https://github.com/StacyClouds/c4sharp.net)

## Conclusion

C4Sharp.NET provides solid support for core C4 modeling with .NET. The main gaps are in:
1. Group elements for better organization
2. Advanced deployment features
3. Some newer DSL features

These gaps are opportunities for enhancement rather than critical issues. The library is fully functional for standard C4 modeling use cases.

## Next Steps

1. Create GitHub issues for high-priority gaps
2. Gather community feedback on priorities
3. Start implementation of top-requested features
4. Regular review against DSL updates
