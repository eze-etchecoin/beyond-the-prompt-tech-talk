# Graph Engineering · Paso 1 — Construir el arnés del grafo (`AGENTS.md`)

> **Objetivo:** definir en `AGENTS.md` los roles del grafo, sus dependencias, el flujo de orquestación y los gates humanos. Es la fuente de verdad del grafo.
> Correr en una ventana de Claude Code sobre el repo, con `src/GraphEngineering/` como área de código.
> **Resultado esperado:** se crea `src/GraphEngineering/AGENTS.md`. No se implementan historias ni se crean subagentes (eso es el paso 2).

---

Vas a montar un **grafo de agentes especializados** para desarrollar features del Minesweeper en `src/GraphEngineering/` (proyectos `Minesweeper.Graph.*`), usando el tablero "Beyond The Prompt tech talk".

Tu tarea **NO es escribir código ni crear subagentes**, sino generar el `AGENTS.md` que defina el grafo. Ya existe uno base con la sección de Trello: extendelo y conservá esa sección tal cual. Antes, inspeccioná el contexto real: código, tests en `tests/GraphEngineering/`, convenciones (`.editorconfig`, `.csproj`, estilo de tests) y el tablero.

## 1. Contexto y alcance
- Qué es este workspace (copia aislada del Minesweeper), stack y convenciones.
- **Guardrails comunes:** no romper build/tests, no exceder el alcance de la historia, respetar el estilo, no commitear secretos.
- **Ramas y PRs:** `graph/<card-slug>` (una por historia, desde `main`); cierre = PR contra `main` (remoto `origin`) con `gh pr create`, mergeado con `gh pr merge` **recién tras GATE 2**.
- El tablero de Trello es la fuente de verdad del estado de cada historia.

## 2. Roles del grafo
El **ORQUESTADOR** (agente root: coordina, rutea y aplica los gates) y cuatro subagentes (`.claude/agents/graph-*`, se crean en el paso siguiente). Para cada uno: responsabilidad, qué consume, qué produce y qué **NO** hace.

| Rol | Responsabilidad | Devuelve |
|---|---|---|
| **graph-analyst** (BA) | Lee "Backlog", prioriza, refina criterios y selecciona N historias. Si una card declara precondiciones, las verifica contra el estado real (tablero + código + tests); si no se cumplen, la deja en "Backlog" y prioriza lo que las habilita. Al final crea follow-ups en "Backlog" (triage humano, no a To Do). No escribe código. | Propuesta priorizada |
| **graph-dev** | Implementa UNA historia con TDD en `graph/<card-slug>`. | Rama + resumen |
| **graph-reviewer** | Revisa el diff: correctitud, estilo, convenciones, cobertura. No implementa. | `OK` / `CAMBIOS: <lista>` |
| **graph-qa** | Valida criterios y casos límite; corre build + test. | `PASA` / `FALLA: <detalle>` |

## 3. Dependencias (el grafo, con sus ciclos)

```text
Backlog → graph-analyst → [GATE 1] → To Do

Orquestador toma la historia
   → graph-dev
   → graph-reviewer   (⇄ Dev si pide cambios)
   → graph-qa         (⇄ Dev si falla)
   → [GATE 2] → Done

Cierre: graph-analyst crea follow-ups → Backlog
```

Los subagentes **NO se invocan entre sí**: devuelven un resultado estructurado al orquestador, que decide el siguiente paso.

## 4. Flujo de orquestación (una pasada)
1. **Planificación:** el orquestador invoca a graph-analyst para priorizar y proponer N historias refinadas. 🛑 **GATE 1:** esperar aprobación del plan; con el OK, el BA mueve las historias a "To Do".
2. **Por cada historia (en orden):** mover a "In Progress" → graph-dev implementa en rama → graph-reviewer revisa (`CAMBIOS` vuelve a Dev, máx. 2 vueltas) → con `OK`, graph-qa valida (`FALLA` vuelve a Dev, máx. 2 vueltas). Si un ciclo no converge → **BLOQUEADA** (comentario en "In Review/Testing") y continuar.
3. **Con `PASA` de QA:** el **orquestador** pushea y abre el PR contra `main` (`gh pr create`) con resumen + veredicto del reviewer (`OK`) + evidencia de QA (`PASA`). 🛑 **GATE 2:** mostrar el PR (diff + resultados) y esperar aprobación; con el OK, `gh pr merge` y card a "Done". El PR lo abre el orquestador, no los subagentes.
4. **Cierre de la pasada:** graph-analyst propone follow-ups en "Backlog".

## 5. Gates y human-in-the-loop
- 🛑 **GATE 1** — aprobar el plan del BA antes de mover historias a "To Do".
- 🛑 **GATE 2** — aprobar y mergear el PR de cada historia antes de cerrarla en "Done".

Sin OK, no se avanza. **Distinción clave:** la review de código la hacen los agentes (reviewer + QA); el PR es la **aprobación humana** sobre ese trabajo ya revisado.

## 6. Definition of Done (por historia)
- Criterios de aceptación cumplidos y cubiertos por tests.
- Reviewer `OK` y QA `PASA`; build + test en verde.
- PR aprobado por la persona, mergeado a `main` y card en "Done".

---

Escribí el `AGENTS.md` en español, conciso y accionable. Al terminar, mostrame un resumen de roles, dependencias y los dos gates. No crees subagentes ni implementes historias todavía.
