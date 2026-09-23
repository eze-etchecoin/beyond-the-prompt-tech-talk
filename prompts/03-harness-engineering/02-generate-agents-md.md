# Harness Engineering · Paso 1 — Construir el arnés (generar `AGENTS.md`)

> **Objetivo:** pasar de "un agente que escribe código" a un **arnés** con etapas por las que atraviesa cada requerimiento, con validación y aprobación humana.
> Correr en una ventana de Claude Code abierta sobre `src/HarnessEngineering/`.
> **Resultado esperado:** el agente crea `AGENTS.md` en la raíz del workspace. No implementa ninguna feature todavía.

---

Vas a trabajar como un agente de ingeniería enmarcado por un **arnés**: un proceso explícito con etapas, validaciones y puntos de aprobación humana. Tu primera tarea **NO es escribir código de features**, sino generar el `AGENTS.md` que defina ese arnés. Ya existe un `AGENTS.md` base con la sección de Trello: extendelo y conservá esa sección tal cual.

## Antes de escribirlo, inspeccioná el contexto real (no inventes)

- El código del juego: `Minesweeper.Core`, `Minesweeper.Console`.
- Los tests en `tests/HarnessEngineering/` y su estilo (describen comportamiento).
- Convenciones: `.editorconfig`, los `.csproj` (net10.0, nullable, implicit usings), xUnit.
- La documentación en `docs/talk/` (especialmente `minesweeper-demo.md`) y el tablero de Trello **"Beyond The Prompt tech talk"**.

## Contenido del `AGENTS.md`

### 1. Contexto y alcance
- Qué es este workspace (copia aislada del Minesweeper).
- Stack y convenciones detectadas.
- Restricciones: sin dependencias innecesarias, sin secretos en el repo, implementación simple y explicable.

### 2. Herramientas disponibles
- **Build y test:** `dotnet build` y `dotnet test` (deben quedar en verde).
- **Ejecución:** `dotnet run` de la consola para verificación manual.
- **Git** para el control de cambios.
- **Trello vía MCP** para tomar y cerrar cards. Cargar la skill `trello-use` antes de usar sus tools.

### 3. El arnés: etapas de cada requerimiento
Cada etapa con su objetivo, entradas, salidas y criterio de avance:

| Etapa | Qué pasa |
|---|---|
| **a) Refinamiento** | Leer la card, aclarar criterios de aceptación, detectar ambigüedades y **preguntar** si falta una decisión de producto o arquitectura. Reformular el alcance. Sin código. |
| **b) Plan con validación** 🛑 | Proponer el plan (archivos, tests, enfoque, riesgos) y validarlo contra reglas y convenciones. **GATE HUMANO:** requiere aprobación antes de implementar. |
| **c) Implementación** | TDD: primero tests que fallen que describan el comportamiento, luego el código mínimo. Cambios acotados a la card. |
| **d) Testing** | `dotnet build` + `dotnet test`; verificar criterios y casos límite. Nada avanza en rojo. |
| **e) Aprobación humana** 🛑 | Presentar diff y resultados de tests. **GATE HUMANO:** OK explícito antes del cierre. |
| **f) Cierre** | Commit en rama `harness/<card-slug>` (desde `main`) con mensaje convencional (`feat: ...`), mover la card a **"Done"** vía MCP y dejar un resumen (incluida la rama). |

> En esta demo el entregable es la rama local con el commit; no abrimos PR porque la revisión ocurre en vivo etapa por etapa (en un flujo de equipo, acá se abriría un PR contra `main`).

### 4. Gates y human-in-the-loop
- Marcar explícitamente los **DOS** puntos donde el agente debe frenar: fin del **Plan (b)** y **Aprobación (e)**.
- En el resto de las etapas avanza solo, siempre dentro de las reglas.

### 5. Reglas / guardrails (no negociables)
- No romper la build ni los tests.
- No exceder el alcance de la card (sin refactors oportunistas).
- Ante ambigüedad, preguntar en vez de asumir.
- Respetar el estilo del repo y de los tests.
- No commitear secretos.

### 6. Definition of Done
- Criterios de aceptación cumplidos y cubiertos por tests.
- `dotnet build` y `dotnet test` en verde.
- Aprobación humana registrada.
- Commit hecho y card en "Done".

---

Escribí el `AGENTS.md` en español, **conciso y operativo** (guía real de trabajo, no texto decorativo). Al terminar, mostrame un resumen de las etapas y los gates, y **esperá indicaciones**. No implementes ninguna feature aún.
