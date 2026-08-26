window.c4sharpEditor = {
  initialize(host, component) {
    if (host.dataset.editorReady) return; host.dataset.editorReady = 'true'; let drag;
    const pointerDown = e => { const node = e.target.closest('[data-c4-element-id]'), svg = e.target.closest('svg'); if (!node || !svg) return; drag = { id: node.dataset.c4ElementId, node, svg, center: elementCenter(node) }; e.preventDefault(); };
    const pointerMove = e => { if (!drag) return; const p = point(drag.svg, e.clientX, e.clientY); drag.node.setAttribute('transform', `translate(${p.x - drag.center.x},${p.y - drag.center.y})`); };
    const pointerUp = async e => { if (!drag) return; const p = point(drag.svg, e.clientX, e.clientY), d = drag; drag = null; d.node.removeAttribute('transform'); await component.invokeMethodAsync('MoveElement', d.id, Math.round(p.x), Math.round(p.y)); };
    const doubleClick = async e => { const edge = e.target.closest('[data-c4-relationship-id]'), svg = e.target.closest('svg'); if (!edge || !svg) return; const p = point(svg, e.clientX, e.clientY); await component.invokeMethodAsync('AddRelationshipVertex', edge.dataset.c4RelationshipId, Math.round(p.x), Math.round(p.y)); };
    host.c4sharpEditorHandlers = { pointerDown, pointerMove, pointerUp, doubleClick };
    host.addEventListener('pointerdown', pointerDown, true);
    host.addEventListener('pointermove', pointerMove, true);
    window.addEventListener('pointerup', pointerUp);
    host.addEventListener('dblclick', doubleClick, true);
  },
  dispose(host) { const handlers = host.c4sharpEditorHandlers; if (handlers) { host.removeEventListener('pointerdown', handlers.pointerDown, true); host.removeEventListener('pointermove', handlers.pointerMove, true); window.removeEventListener('pointerup', handlers.pointerUp); host.removeEventListener('dblclick', handlers.doubleClick, true); } delete host.c4sharpEditorHandlers; delete host.dataset.editorReady; }
};
function point(svg, x, y) { const p = svg.createSVGPoint(); p.x = x; p.y = y; return p.matrixTransform(svg.getScreenCTM().inverse()); }
function elementCenter(node) { const rect = node.querySelector('rect'); if (rect) return { x: Number(rect.getAttribute('x')) + Number(rect.getAttribute('width')) / 2, y: Number(rect.getAttribute('y')) + Number(rect.getAttribute('height')) / 2 }; const circle = node.querySelector('circle'); return { x: Number(circle.getAttribute('cx')), y: Number(circle.getAttribute('cy')) }; }
