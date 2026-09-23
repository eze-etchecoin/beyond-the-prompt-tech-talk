# AGENTS.md

## Tablero de trabajo (Trello)

- Tenés acceso a Trello a través del **MCP de Trello**. Antes de usar cualquier
  tool de Trello (`trelloRead*`, `trelloWrite*`, `trelloSearch`), cargá la skill
  `trello-use`.
- Las cards de trabajo están en el tablero **"Beyond The Prompt tech talk"**.
  Buscalo por nombre (`trelloSearch`) y tomá de ahí las cards, sus listas y
  criterios de aceptación.
- Si el MCP de Trello no está disponible, frená y avisá: no inventes el contenido
  de las cards.

## 1. Contexto y alcance

Este directorio (`src/LoopEngineering/`) es un **workspace aislado**: una
copia propia del Minesweeper (`Minesweeper.Core` + `Minesweeper.Console`, con
tests en `tests/LoopEngineering/Minesweeper.Tests/`), sembrada desde el
producto base `src/Minesweeper/`. Proyectos y namespaces usan el prefijo
`Minesweeper.Loop.*` para no chocar con las otras copias (Harness, Graph). No
toques nada fuera de `src/LoopEngineering/` y `tests/LoopEngineering/`.

El Core está **deliberadamente incompleto**: el backlog de features vive como
cards en Trello y se implementa acá, de a una, en **modo LOOP autónomo** con
validación automática y revisión asíncrona.

**Stack y convenciones a respetar:**
- .NET / C#, `net10.0`, `ImplicitUsings` y `Nullable` habilitados en todos los
  `.csproj`.
- `.editorconfig` del repo: 4 espacios de indentación en `.cs` (2 en
  json/yml/csproj), `crlf`, llave siempre en línea nueva
  (`csharp_new_line_before_open_brace = all`), llaves obligatorias
  (`csharp_prefer_braces`), namespaces *file-scoped*, `using` de `System.*`
  primero.
- Tests con **xUnit**, estilo *given/when/then* implícito en el nombre del
  método (`Metodo_condicion_resultado` en `snake_case` con mayúscula inicial,
  ver `GameTests.cs`), describiendo comportamiento observable a través de la
  API pública — no implementación interna. Se prefieren tableros deterministas
  vía `Game.CreateWithMines(...)` para asserts exactos.
- Regla de dominio existente y no negociable: el tiempo/estado transcurrido se
  expone como **valor consultable** (nunca con `Timer`, hilo propio o bucle),
  ni en `Core` ni en `Console`.

**Restricciones:**
- No introducir dependencias/paquetes nuevos sin que lo pida explícitamente la
  card o lo apruebe la persona en el gate del plan.
- No commitear secretos ni credenciales.
- Mantené la implementación simple y explicable: preferí la solución más
  directa que cumpla el criterio de aceptación.

## 2. Herramientas disponibles

- **Build:** `dotnet build` (o sobre la solución/proyecto de este workspace).
- **Test:** `dotnet test tests/LoopEngineering/Minesweeper.Tests/Minesweeper.Loop.Tests.csproj`
  (deben quedar en verde antes de commitear).
- **Ejecución manual:** `dotnet run --project src/LoopEngineering/Minesweeper.Console -- <dificultad b|i|e> <seed opcional>`
  para verificación jugable.
- **Git** para control de cambios (una rama por card, `loop/<card-slug>`).
- **GitHub CLI (`gh`)** para abrir PRs automáticamente.
- **Trello vía MCP** para tomar y mover cards (ver sección de arriba). Recordá
  cargar la skill `trello-use` antes de usar sus tools.

## 3. Condición de salida (modo loop)

El agente itera tomando cards de la lista **"To Do"** del tablero "Beyond The
Prompt tech talk" (por posición, de arriba hacia abajo) hasta que **"To Do"
quede vacía**. Ese es el único criterio de finalización: no se frena a pedir
OK entre cards.

## 4. El ciclo: seis etapas por cada card

Cada card atraviesa estas seis etapas, en orden. A diferencia de un arnés con
gates humanos, acá la validación de etapas b) y e) es **automática**; la
persona revisa **en paralelo, de forma asíncrona**, vía Pull Request.

**a) Tomar la card**
- Tomar la card de más arriba de "To Do" → moverla a **"In Progress"**.

**b) Refinamiento del requerimiento**
- Objetivo: leer la card, aclarar criterios de aceptación, detectar
  ambigüedades.
- Si la card es **ambigua** o depende de una decisión que no se puede resolver
  sola (sin humano disponible para preguntar en el momento): **no crear rama**,
  mover la card a **"In Review/Testing"** con el comentario
  `BLOQUEADO: <razón>` y pasar a la siguiente card (ver sección 6).
- Si el alcance es claro: reformularlo en una o dos frases y seguir.

**c) Plan + rama — [GATE AUTOMÁTICO]**
- Crear la rama `loop/<card-slug>` desde `main`.
- Definir brevemente archivos a tocar, tests a agregar/ajustar y enfoque,
  validándolo contra las reglas de este archivo. No requiere aprobación
  humana: el agente sigue solo si el plan respeta el alcance de la card y las
  convenciones del repo.

**d) Implementación (TDD)**
- Primero tests que fallen describiendo el comportamiento esperado, luego el
  código mínimo para pasarlos. Cambios acotados al alcance de la card.
- Avanza cuando el ciclo rojo→verde está completo para el alcance acordado.

**e) Testing — [GATE AUTOMÁTICO]**
- Correr `dotnet build` y `dotnet test`; verificar uno por uno los criterios
  de aceptación de la card (autochequeo, sin intervención humana).
- Avanza cuando build y tests están en verde y los criterios están cubiertos.
  **Nada se commitea en rojo.**

**f) Cierre**
- Si verde y criterios cumplidos:
  1. Commit convencional en la rama (`feat: ...`, `fix: ...`).
  2. Pushear la rama y abrir el PR contra `main` con `gh pr create`.
  3. Volver a `main`.
  4. Mover la card a **"In Review/Testing"** con el comentario
     `LISTO PARA REVISIÓN: <resumen + link al PR + cómo probar>`.
- Si no se puede completar (build/tests en rojo sin solución dentro del
  alcance, bloqueo técnico, etc.):
  1. Descartar los cambios de la rama y volver a `main` limpio.
  2. Mover la card a **"In Review/Testing"** con el comentario
     `BLOQUEADO: <razón>` (sin PR).
- Continuar con la siguiente card de "To Do".

El entregable de cada card exitosa es un **PR abierto contra `main`**
(remoto `origin`). La persona revisa y mergea el PR en paralelo, sin que eso
detenga el loop.

## 5. Manejo de bloqueos

Las cards ambiguas o irrecuperables **se saltean y se anotan**: salen de
"To Do" hacia "In Review/Testing" con el comentario `BLOQUEADO: <razón>`, sin
rama ni PR. Así la condición de salida (sección 3) siempre es alcanzable y el
loop nunca se cuelga esperando una decisión humana en vivo.

## 6. Reglas / guardrails (no negociables)

- Nunca dejar un commit con build o tests en rojo.
- No exceder el alcance de la card: sin refactors oportunistas no pedidos.
- Ante ambigüedad que no se puede resolver sola, no asumir: bloquear la card
  (sección 5) en vez de adivinar.
- Respetar el estilo del repo y de los tests existentes.
- No commitear secretos.

## 7. Definition of Done (por card)

- Card exitosa: criterios de aceptación cumplidos y cubiertos por tests,
  `dotnet build` y `dotnet test` en verde, commit convencional en
  `loop/<card-slug>`, rama pusheada, PR abierto contra `main` (`gh pr create`),
  card en "In Review/Testing" con comentario `LISTO PARA REVISIÓN: ...`
  (resumen + link al PR + cómo probar).
- Card bloqueada: sin rama ni PR, card en "In Review/Testing" con comentario
  `BLOQUEADO: <razón>`.
- Loop completo: lista "To Do" vacía.
