# Graph Engineering — prompts

Demo del **grafo de agentes**: el salto de *"un agente que hace todo"* a un
conjunto de **agentes especializados que colaboran**, con **ciclos de feedback** y
**human-in-the-loop** en los puntos clave.

Se trabaja en el workspace [`src/GraphEngineering/`](../../src/GraphEngineering/)
(proyectos `Minesweeper.Graph.*`) sobre el tablero de Trello *Beyond The Prompt
tech talk*. El **arnés del grafo** (roles, dependencias, flujo y gates) se define
en un `AGENTS.md` del workspace; los agentes se implementan como **subagentes de
Claude Code** (`.claude/agents/graph-*.md`) que orquesta el agente root.

## Agentes

| Agente | Rol | Consume | Produce |
| --- | --- | --- | --- |
| **Orquestador** (root) | Coordina el grafo, rutea el trabajo, mueve las cards y aplica los gates. | estado del tablero | asignaciones + transiciones |
| **graph-analyst** (BA) | Prioriza y refina el *Backlog*, selecciona N historias y crea follow-ups. | Backlog | To Do refinado + nuevas cards |
| **graph-dev** | Implementa la historia con TDD en `graph/<card-slug>`. | historia | rama + commit |
| **graph-reviewer** | Revisa el diff (correctitud, estilo, convenciones, cobertura). | rama del Dev | OK / cambios |
| **graph-qa** | Valida criterios + casos límite; corre build+test. | rama revisada | pasa / falla |

## El grafo (con sus ciclos y gates)

```
Backlog ──▶ graph-analyst ──▶ [GATE 1: aprobación del plan] ──▶ To Do
                                                                  │
                                                   Orquestador toma la historia
                                                                  ▼
                                                              graph-dev ──▶ graph-reviewer
                                                                  ▲               │
                                                     (cambios) ───┘               ▼ (OK)
                                                                  ▲            graph-qa
                                                       (falla) ───┴───────────────┤
                                                                                  ▼ (pasa)
                                          [GATE 2: PR → aprobación humana → merge] ──▶ Done
                                                                                  │
                                                                                  ▼
                                            graph-analyst crea follow-ups ──▶ Backlog
```

- **Ciclos de feedback:** Dev⇄Reviewer y Dev⇄QA (máx. 2 vueltas; si no converge,
  la historia se marca BLOQUEADA y el grafo sigue).
- **Human-in-the-loop:** GATE 1 (aprobar la selección/priorización del BA) y
  GATE 2 (aprobar y mergear el **PR** de cada historia). Ojo: la review de código
  la hacen los agentes (Reviewer + QA); el PR es la aprobación **humana** encima.
- **Lazo grande:** el entregable retroalimenta el Backlog vía el BA — el sistema
  se genera trabajo nuevo (para triage humano).

> El tablero de Trello ya debería existir con su backlog (se crea **una sola
> vez**, antes de la demo de Harness, con
> [`prompts/00-shared/init-trello-board.txt`](../00-shared/init-trello-board.txt)).
> Si estás corriendo Graph de forma independiente, corré ese prompt primero.

## Prompts

| Orden | Archivo | Rol |
| --- | --- | --- |
| 1 | [`01-generate-agents-md.txt`](01-generate-agents-md.txt) | Crea el `AGENTS.md` del workspace: roles, dependencias, flujo y gates. |
| 2 | [`02-generate-subagents.txt`](02-generate-subagents.txt) | Crea los 4 subagentes `graph-*` en `.claude/agents/`, consistentes con el `AGENTS.md`. |
| 3 | [`03-run-the-graph.txt`](03-run-the-graph.txt) | Disparo fino: el orquestador corre una pasada siguiendo el `AGENTS.md`. |

## Flujo sugerido en vivo

1. Correr `01` → se crea `src/GraphEngineering/AGENTS.md` (el arnés del grafo).
2. Correr `02` → se crean los subagentes `graph-*` (no se implementa nada aún).
3. Correr `03` → el orquestador: pide plan al BA (GATE 1), y por cada historia
   corre Dev → Reviewer → QA con feedback, abre el PR y frena esperando tu
   aprobación para mergearlo (GATE 2), y al final el BA propone follow-ups en
   Backlog.

> El `AGENTS.md` y los subagentes se crean en vivo como parte de la demo (pasos 1
> y 2); no se pre-versionan. Convención de ramas coherente con las otras demos:
> `graph/<card>`.
