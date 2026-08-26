## 1. Test-first package contract

- [x] 1.1 Add failing renderer tests for interactive SVG identifiers.
- [x] 1.2 Add failing editor tests for view navigation, element movement, segment-aware vertex insertion, and save requests.
- [x] 1.3 Add failing package metadata tests or build verification for independent Razor packaging.

## 2. Renderer interaction contract

- [x] 2.1 Emit escaped view, element, and relationship identifiers in renderer SVG output.
- [x] 2.2 Make renderer identifier tests pass without introducing Razor dependencies.

## 3. Razor editor package

- [x] 3.1 Create the multi-targeted, packable Razor class library with package metadata and renderer dependency.
- [x] 3.2 Implement workspace navigation, view selection, save-request, and persisted layout mutation APIs.
- [x] 3.3 Implement scoped JavaScript and styles for SVG drag/drop and double-click vertex insertion.
- [x] 3.4 Make editor behavior tests pass.

## 4. Demonstration and documentation

- [x] 4.1 Update the web demo to use the optional Razor editor package, thumbnail navigator, and host-owned save handling.
- [x] 4.2 Document package installation, static assets, interactive render mode, and workspace persistence responsibility.

## 5. Validation

- [x] 5.1 Run focused renderer and editor tests across supported targets.
- [x] 5.2 Pack the Razor library and verify the renderer remains independently buildable.
- [x] 5.3 Run solution restore, build, and tests.

## 6. Follow-up interaction fixes

- [x] 6.1 Add a failing renderer test for preserving deterministic positions after one element is moved.
- [x] 6.2 Preserve per-element deterministic fallback positions and add live drag feedback.
- [x] 6.3 Run focused renderer and editor tests.

## 7. Live connector drag feedback

- [x] 7.1 Add failing renderer and editor asset tests for connector endpoint feedback.
- [x] 7.2 Emit connector endpoint identifiers and update connected connector endpoints while dragging.
- [x] 7.3 Run focused renderer and editor tests.
