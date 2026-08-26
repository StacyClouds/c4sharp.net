window.c4sharpEditor = window.c4sharpEditor || {
  initialize(host, component) {
    if (host.dataset.editorReady) return; host.dataset.editorReady = 'true'; let drag;
    host.addEventListener('pointerdown', e => { const node = e.target.closest('[data-c4-element-id]'), svg = e.target.closest('svg'); if (!node || !svg) return; drag = { id: node.dataset.c4ElementId, svg }; node.setPointerCapture(e.pointerId); e.preventDefault(); });
    host.addEventListener('pointerup', async e => { if (!drag) return; const p = point(drag.svg, e.clientX, e.clientY), d = drag; drag = null; await component.invokeMethodAsync('MoveElement', d.id, Math.round(p.x), Math.round(p.y)); });
    host.addEventListener('dblclick', async e => { const edge = e.target.closest('[data-c4-relationship-id]'), svg = e.target.closest('svg'); if (!edge || !svg) return; const p = point(svg, e.clientX, e.clientY); await component.invokeMethodAsync('AddRelationshipVertex', edge.dataset.c4RelationshipId, Math.round(p.x), Math.round(p.y)); });
  },
  dispose(host) { delete host.dataset.editorReady; }
};
function point(svg, x, y) { const p = svg.createSVGPoint(); p.x = x; p.y = y; return p.matrixTransform(svg.getScreenCTM().inverse()); }
