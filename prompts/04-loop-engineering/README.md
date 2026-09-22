# Loop Engineering — prompts

Demo del **bucle autónomo**: cómo pasamos de *"un requerimiento a la vez, con
gates humanos"* (Harness) a un **loop que vacía la lista "To Do"** con validación
automática y **revisión humana asíncrona**.

Se trabaja en el workspace [`src/LoopEngineering/`](../../src/LoopEngineering/)
(copia aislada del Minesweeper) sobre el tablero de Trello *Beyond The Prompt
tech talk*, **reutilizando el `AGENTS.md`** que se creó en la demo de Harness.

## La diferencia con Harness

| | Harness | Loop |
| --- | --- | --- |
| Unidad de trabajo | una card | todas las de "To Do" |
| Gates | 2 gates **humanos** (Plan y Aprobación) | validación **automática** (build+test) |
| Revisión humana | síncrona (frena y espera) | **asíncrona** vía PR (abre el PR, deja la card en *In Review* y sigue) |
| Fin | cierre de esa card | **"To Do" vacía** (condición de salida) |

## Cómo maneja los casos difíciles

- **Card ambigua o bloqueada** → la mueve a *In Review/Testing* con un comentario
  `BLOQUEADO: <razón>` y **continúa** (el loop no se cuelga).
- **Card completada** → rama `loop/<card-slug>` desde `main` + un commit
  convencional + **PR contra `main`** (`gh pr create`) + card a *In Review/Testing*
  con `LISTO PARA REVISIÓN: <resumen + link al PR + cómo probar>`. El entregable de
  cada card es un PR abierto.
- La persona revisa/mergea los PRs **en paralelo**, sin frenar la iteración.

## Prompts

| Orden | Archivo | Rol |
| --- | --- | --- |
| 1 | [`01-adapt-agents-md.txt`](01-adapt-agents-md.txt) | Adapta el `AGENTS.md` de Harness a modo loop (gates automáticos, revisión asíncrona, condición de salida, ramas `loop/`). |
| 2 | [`02-run-the-loop.txt`](02-run-the-loop.txt) | Disparo fino: corre el bucle sobre "To Do" hasta vaciarla siguiendo el `AGENTS.md`. |

## Flujo sugerido en vivo

1. Copiá el `AGENTS.md` generado en la demo de Harness a la raíz de este workspace.
2. Corré `01-adapt-agents-md.txt` → adapta ese `AGENTS.md` a modo loop.
3. Promové en Trello unas cuantas cards de *Backlog* a *To Do*.
4. Corré `02-run-the-loop.txt` → el agente toma card por card, abre un PR por cada
   una completada, las deja en *In Review/Testing* (listas o bloqueadas) y **se
   detiene solo** cuando *To Do* queda vacía.

> El `AGENTS.md` no está versionado: se reutiliza el de Harness (copiado) y se
> adapta en vivo. Mismo patrón que Graph (arnés en `AGENTS.md` + disparo fino).
