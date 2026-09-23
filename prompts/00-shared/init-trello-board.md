# Paso 0 (compartido) — Inicializar el tablero de Trello

> **Objetivo:** crear (o completar) el tablero "Beyond The Prompt tech talk" con las listas y las cards del backlog, listo para las demos de **Harness, Loop y Graph** (las tres usan el mismo tablero).
> **Requisito previo (manual):** MCP de Trello conectado y autenticado (ver [`docs/tools/trello-mcp-setup.md`](../../docs/tools/trello-mcp-setup.md)) contra **tu propia** cuenta/workspace.
> Correr **una sola vez**, antes de la primera demo (Harness).
> **Resultado esperado:** tablero con 5 listas y 9 cards en "Backlog"; ninguna en "To Do" todavía.

---

Preparame el tablero de Trello para la demo de Minesweeper usando el MCP de Trello.

### 1. Buscar o crear el tablero
Buscá si ya existe un tablero **"Beyond The Prompt tech talk"** en mi cuenta. Si existe, reutilizalo. Si no, creá uno nuevo con ese nombre.

### 2. Listas (en este orden)
`Backlog` → `To Do` → `In Progress` → `In Review/Testing` → `Done`

### 3. Cards del backlog (en "Backlog")
Tomá la tabla **"Backlog inicial (cards de Trello)"** de [`docs/talk/minesweeper-demo.md`](../../docs/talk/minesweeper-demo.md) y creá una card por fila:
- **Título:** el nombre de la feature.
- **Descripción:** el criterio de aceptación resumido, tal cual está en la tabla.

Son 9 cards: First-click safety, Chording, Contador de tiempo/movimientos,
Detección de "sin banderas suficientes", Persistencia de mejores tiempos,
Auto-flag/pistas, Render con colores, Dificultad personalizada, Portear el
Core a WPF.

### 4. No promover nada todavía
Las 9 cards quedan en "Backlog". La promoción a "To Do" se hace a mano en cada
demo (Harness la hace card por card; Loop y Graph promueven varias antes de
disparar el bucle/orquestador), según el alcance que se quiera mostrar en vivo.

---

Al terminar, mostrame el link del tablero y la lista de cards creadas (o ya existentes, si reutilizaste el tablero).
