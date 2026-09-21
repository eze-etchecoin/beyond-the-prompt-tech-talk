# Graph Engineering — workspace

Este directorio es un **workspace aislado** con su **propia copia** del Minesweeper
(`Minesweeper.Graph.*`, tests en `tests/GraphEngineering/`), sembrada desde el
producto base `src/Minesweeper/`.

## Dinámica de la demo

Un **orquestador** (agente root) coordina cuatro **subagentes especializados**
(`.claude/agents/graph-*`): **Analista Funcional (BA)**, **Dev**, **Reviewer** y
**QA**, con ciclos de feedback (Dev⇄Reviewer, Dev⇄QA) y dos **gates humanos**
(aprobar el plan del BA y aprobar el merge final). El BA además crea historias
nuevas en *Backlog* a partir de los entregables.

El grafo completo, los prompts y el flujo en vivo están en
[`prompts/05-graph-engineering/`](../../prompts/05-graph-engineering/).

Convención de ramas: `graph/<card-slug>` (coherente con `harness/` y `loop/`).
