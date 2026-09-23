# Graph Engineering · Paso 3 — Disparar una pasada del grafo

> **Requisitos:** `AGENTS.md` creado (paso 1) y subagentes `graph-*` creados (paso 2).
> Correr como el **ORQUESTADOR** (agente root), con `src/GraphEngineering/` como área de código.

---

Actuá como **ORQUESTADOR** del grafo y corré **UNA pasada completa** siguiendo el flujo de `src/GraphEngineering/AGENTS.md`.

## Puntos a respetar

- **Ruteo:** invocá a `graph-analyst`, `graph-dev`, `graph-reviewer` y `graph-qa`; ellos no se llaman entre sí, los ruteás vos. El PR de GATE 2 lo abrís y mergeás **vos**, no los subagentes.
- **Dos gates humanos:**
  - 🛑 **GATE 1** — aprobar el plan del BA antes de mover historias a "To Do".
  - 🛑 **GATE 2** — con el `PASA` de QA, abrís el PR contra `main` con los veredictos de reviewer/QA en la descripción y esperás mi aprobación antes de mergear y cerrar la card en "Done".
  - No avances sin mi OK en esos puntos.
- **Ciclos** Dev ⇄ Reviewer y Dev ⇄ QA con **máximo 2 vueltas**; si no converge, marcá la historia **BLOQUEADA** y seguí.
- El tablero "Beyond The Prompt tech talk" es la **fuente de verdad**.

## Al terminar, dame el resumen

- ✅ Historias completadas y mergeadas (con link a su PR)
- ⛔ Historias bloqueadas (con razón)
- 🌿 Ramas / PRs generados
- 📋 Follow-ups creados en "Backlog"
