# Demo compartida: Minesweeper

El **Minesweeper** es el proyecto que hila las demos de **Harness**, **Loop** y
**Graph Engineering**. Es familiar, visual y tiene reglas claras — ideal para
mostrar en vivo cómo un agente trabaja dentro de un entorno bien diseñado.

## Estado actual (punto de partida)

Ya implementado, compilando y con tests:

- `src/Minesweeper/Minesweeper.Core/` — lógica de dominio:
  - `Difficulty` / `BoardSpec`: 3 niveles clásicos
    (Beginner 9x9/10, Intermediate 16x16/40, Expert 16x30/99) con validación.
  - `Game`: colocación de minas (aleatoria con `Random` seedeable, o explícita
    vía `CreateWithMines` para demos deterministas), conteo de adyacencia,
    revelado con **flood-fill**, banderas y detección de victoria/derrota.
- `src/Minesweeper/Minesweeper.Console/` — TUI jugable y determinista
  (acepta dificultad y *seed* por argumentos).
- `tests/Minesweeper/Minesweeper.Tests/` — tests de comportamiento (xUnit).

> Nota didáctica: el núcleo está **deliberadamente incompleto**. Varias features
> quedan como *backlog* para construirlas **en vivo** con el flujo agéntico. Si
> implementáramos todo ahora, no quedaría nada para las demos de Loop y Graph.

## Modelo de trabajo: un workspace por enfoque

`src/Minesweeper/` es el **producto base canónico** (la referencia). Cada demo
trabaja sobre su **propia copia aislada**, sembrada desde esa base, para que las
demos no se pisen y cada una arranque de un estado fijo:

| Enfoque | Workspace (src) | Tests | Proyectos / namespace |
|---|---|---|---|
| Harness | `src/HarnessEngineering/` | `tests/HarnessEngineering/` | `Minesweeper.Harness.*` |
| Loop    | `src/LoopEngineering/`    | `tests/LoopEngineering/`    | `Minesweeper.Loop.*` |
| Graph   | `src/GraphEngineering/`   | `tests/GraphEngineering/`   | `Minesweeper.Graph.*` |

Dinámica prevista: se abre una ventana de Claude Code / VS Code por workspace, y
cada uno corre su propia dinámica.

- **Harness:** se crea **en vivo** un `AGENTS.md` (a partir de un prompt en
  `prompts/03-harness-engineering/`) con los lineamientos, herramientas y reglas.
- **Loop:** se reutiliza ese `AGENTS.md` y se hace trabajar al agente **en bucle**
  sobre las *cards* de Trello (TDD + condición de salida).
- **Graph:** más adelante se definen los agentes especializados y su interacción.

> El tablero de Trello con las *cards* es el backlog común desde el que cada
> workspace toma trabajo.

## Mapa de la demo a los tres enfoques

### 3. Harness Engineering — herramientas, reglas y validación
El "arnés" que enmarca al agente:
- **Herramientas:** el tablero de Trello (vía MCP), `dotnet build`, `dotnet test`.
- **Reglas:** convenciones del repo (`.editorconfig`), estilo de los tests
  (describir comportamiento), "no romper la build ni los tests".
- **Validación:** cada cambio debe compilar y pasar los tests antes de avanzar.
- **Qué se muestra:** cómo, con el arnés puesto, el agente produce cambios
  seguros y verificables en `Minesweeper.Core`.

### 4. Loop Engineering — bucle con condición de salida (Trello + TDD)
- El agente toma una *card* de Trello (una feature del backlog de abajo).
- Aplica **TDD**: escribe/ajusta tests, implementa, corre `dotnet test`.
- **Condición de salida:** la card está "hecha" cuando los tests pasan y el
  criterio de aceptación se cumple; recién ahí mueve la card a *Done* y toma la
  siguiente.
- **Qué se muestra:** un bucle autónomo, acotado y observable a través del tablero.

### 5. Graph Engineering — agentes especializados y reviews
- **Agente Dev:** implementa la card.
- **Agente Reviewer:** revisa el diff (correctitud, estilo, cobertura).
- **Agente QA:** valida criterios de aceptación / prueba casos límite.
- Coordinación entre roles con *hand-offs* y revisiones cruzadas.
- **Qué se muestra:** cómo varios agentes especializados colaboran sobre el mismo
  Minesweeper, con la persona en el rol de *human-in-the-loop* aprobando merges.

## Backlog inicial (cards de Trello)

Estas features **no** están implementadas y son el material de las demos de Loop
y Graph. Cada una es una *card* con criterios de aceptación verificables por tests.

| # | Card | Criterio de aceptación (resumen) |
|---|------|----------------------------------|
| 1 | **First-click safety** | La primera celda revelada nunca es una mina; se recoloca antes del primer `Reveal`. |
| 2 | **Chording** (revelar vecinos) | Sobre una celda numerada con banderas suficientes, revelar sus vecinos no marcados. |
| 3 | **Contador de tiempo / movimientos** | El juego expone tiempo transcurrido y/o número de jugadas. |
| 4 | **Detección de "sin banderas suficientes"** | Ayudas/estado cuando quedan banderas por colocar. |
| 5 | **Persistencia de mejores tiempos** | Guardar y mostrar mejores tiempos por dificultad. |
| 6 | **Auto-flag / pistas** | Marcar automáticamente minas evidentes (modo asistido). |
| 7 | **Render con colores** | Colorear números/banderas/minas en la consola. |
| 8 | **Dificultad personalizada** | Permitir filas/columnas/minas custom validadas por `BoardSpec`. |
| 9 | **Portear el Core a una app WPF** | Frontend WPF sobre el mismo `Core`, sin tocar el dominio. Cierre de la demo. |

> El orden y el alcance son provisionales; se ajustarán al ensayar los tiempos.

### Nota sobre la card 3 (contador de tiempo)

El Core debe exponer el tiempo como **valor consultable** (p. ej. `Elapsed`
calculado desde el timestamp del primer reveal), nunca como algo que "corre":
sin `Timer`, sin bucle, sin hilo propio — ni en `Core` ni en la consola. Así la
consola lo imprime cuando redibuja y un futuro WPF le pone un `DispatcherTimer`
encima sin que el dominio se entere. Si esta regla se rompe, la card 9 se
complica: conviene dejarla explícita en el `AGENTS.md` de cada workspace.

### Card 9 — el port como cierre (y como prueba del arnés)

El WPF va **al final, a propósito**. La consola es el entorno donde el agente
tiene el loop de feedback cerrado: puede ejecutar, leer la salida y compararla
con lo esperado. Una UI WPF le corta ese loop — solo le queda `dotnet test`. Por
eso primero se madura el dominio donde el agente puede verificarse a sí mismo, y
recién después se portea, cuando ya no hay lógica en juego.

Además, el port **valida retroactivamente todo lo anterior**: si durante las
demos se respetó "la lógica va en `Core`, la UI no", el port es casi mecánico; si
se filtró lógica al `Program.cs`, queda expuesto al instante. Por eso sus
criterios de aceptación son verificables sin ojos (`Core` sin cambios, tests
intactos y en verde, build OK) y la validación visual queda como
*human-in-the-loop* en el GATE 2.

Se hace sobre **un solo workspace** — el de Graph, que es el último — no sobre
los tres. La consola no se elimina: quedan dos frontends sobre un `Core`
compartido, que es en sí un argumento a favor del arnés.

El BA (`graph-analyst`) decide cuándo la card entra a *To Do*: la descripción
lleva **precondiciones explícitas** (cards de dominio en Done, tests en verde,
sin lógica en la capa de consola) para que pueda razonar por sí mismo si ya es
elegible o si conviene priorizar antes las cards de dominio pendientes.

## Cómo correrlo

```bash
# Tests
dotnet test tests/Minesweeper/Minesweeper.Tests/Minesweeper.Tests.csproj

# Jugar (dificultad b|i|e y seed opcional para un tablero reproducible)
dotnet run --project src/Minesweeper/Minesweeper.Console -- b 42
```

Comandos en juego: `r <fila> <col>` revela, `f <fila> <col>` marca/desmarca, `q` sale.
