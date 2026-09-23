# Graph Engineering · Paso 2 — Crear los subagentes

> **Objetivo:** materializar los roles de `AGENTS.md` como subagentes de Claude Code en `.claude/agents/`. El flujo y las dependencias ya viven en `AGENTS.md` (paso 1); acá solo creamos las piezas.
> Correr con `AGENTS.md` ya creado en `src/GraphEngineering/`.
> **Resultado esperado:** 4 archivos en `.claude/agents/`. No se implementa nada.

---

Leé `src/GraphEngineering/AGENTS.md` y creá los **CUATRO** subagentes descritos ahí en `.claude/agents/`, consistentes con sus roles, dependencias y guardrails.

Cada subagente lleva **frontmatter** (`name`, `description`, `tools`, `model: sonnet`) y un system prompt claro y acotado a su rol.

> Los subagentes **NO se invocan entre sí**: hacen su tarea y devuelven un resultado estructurado al orquestador (el agente root, no un archivo).

| Archivo | Rol | Tools | Devuelve |
|---|---|---|---|
| `graph-analyst.md` | Analista Funcional / BA | Trello (`mcp__trello__*`), Read, Grep, Glob | Propuesta priorizada (con justificación) y, al cierre, follow-ups creados en "Backlog" |
| `graph-dev.md` | Dev | Read, Write, Edit, Bash (dotnet, git), Grep, Glob | Rama `graph/<card-slug>` + resumen del cambio y tests agregados |
| `graph-reviewer.md` | Reviewer de código | Read, Grep, Glob, Bash (git diff) | `OK` o `CAMBIOS: <lista concreta>` — no implementa |
| `graph-qa.md` | QA | Read, Bash (dotnet), Grep | `PASA` o `FALLA: <detalle>` con evidencia (salida de tests) |

En cada system prompt referenciá que el flujo global y los gates están en `AGENTS.md`, e incluí los guardrails comunes.

Al terminar, mostrame el resumen de los 4 subagentes creados. No implementes ninguna historia todavía.
