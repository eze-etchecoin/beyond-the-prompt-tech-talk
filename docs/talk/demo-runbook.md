# Runbook de demos

Guion operativo de cada demostración. Se completará de forma incremental. Para
Loop y Graph todavía **no** se inventan implementaciones: solo se registran los
huecos a definir.

---

## Demo 1 — Prompt Engineering

- **Estado inicial:** sin código; solo los prompts de `prompts/01-prompt-engineering/`.
- **Objetivo:** construir un algoritmo básico guiando al modelo paso a paso, agregando validaciones y luego separando responsabilidades.
- **Archivos / herramientas:** `prompts/01-prompt-engineering/01-initial-request.txt`, `02-add-validation.txt`, `03-refactor.txt`; un asistente de IA en modo interactivo.
- **Resultado esperado:** una primera implementación funcional obtenida en tres pasos manuales.
- **Plan de contingencia:** si el modelo se desvía, retroceder al prompt anterior y mostrar el resultado ya versionado.

---

## Demo 2 — Context Engineering

- **Estado inicial:** la solución de word wrapping ya implementada (`src/ContextEngineering/`, `tests/ContextEngineering/`).
- **Objetivo:** pedirle a un agente **con acceso al repo** cómo incorporar logging, comparando alternativas, sin modificar código todavía.
- **Archivos / herramientas:** `prompts/02-context-engineering/01-analyze-logging-requirement.txt`; el repositorio completo como contexto.
- **Resultado esperado:** un análisis que identifica archivos a tocar, compara al menos dos alternativas y formula preguntas de producto/arquitectura pendientes.
- **Plan de contingencia:** si el agente propone cambiar código, recordarle la restricción "no modificar todavía" y volver a enfocar en el análisis.

---

> Demos 3 a 5 comparten el proyecto **Minesweeper** (`src/Minesweeper/`).
> Diseño y backlog: [`minesweeper-demo.md`](minesweeper-demo.md).

## Demo 3 — Harness Engineering

- **Estado inicial:** `Minesweeper.Core` compilando y con tests en verde.
- **Objetivo:** mostrar el "arnés" (herramientas, reglas y validación) que hace que los cambios del agente sean seguros y verificables.
- **Archivos / herramientas:** `src/Minesweeper/`, `tests/Minesweeper/`, `.editorconfig`, `dotnet build`/`dotnet test`, tablero de Trello (MCP).
- **Resultado esperado:** un cambio pequeño aplicado dentro del arnés, que compila y mantiene los tests en verde.
- **Plan de contingencia:** si algo rompe la build, mostrar cómo el arnés lo detecta y se revierte al último estado verde.

---

## Demo 4 — Loop Engineering

- **Estado inicial:** backlog de *cards* cargado en Trello (ver `minesweeper-demo.md`).
- **Objetivo:** un bucle autónomo que toma una card, la implementa con **TDD** y la deja lista para revisión (abre un PR) cuando los tests pasan; la condición de salida del loop es "To Do" vacía.
- **Archivos / herramientas:** `src/LoopEngineering/`, `tests/LoopEngineering/`, Trello (MCP), `dotnet test`, git + `gh` (un PR por card).
- **Resultado esperado:** al menos una card (p. ej. *first-click safety*) implementada, con su **PR abierto contra `main`** y la card en *In Review/Testing* (tests nuevos en verde). La persona mergea el PR en paralelo.
- **Plan de contingencia:** si el bucle se traba, tomar una card más simple del backlog o mostrar el resultado ya versionado.

---

## Demo 5 — Graph Engineering

- **Estado inicial:** una card en *In Progress* lista para pasar por revisión.
- **Objetivo:** agentes especializados (Dev, Reviewer, QA) colaborando con revisiones cruzadas y *human-in-the-loop* aprobando y mergeando el **PR** de cada historia (GATE 2).
- **Archivos / herramientas:** `src/GraphEngineering/`, `tests/GraphEngineering/`, Trello (MCP), git + `gh` (PR en GATE 2), diff.
- **Resultado esperado:** una card que atraviesa Dev → Review → QA → **PR** → Done, con feedback visible entre agentes y la persona aprobando/mergeando el PR. (Distinción clave: la review de código la hacen los agentes; el PR es la aprobación humana.)
- **Plan de contingencia:** si la coordinación se complica, reducir a dos roles (Dev + Reviewer) y narrar el resto.
