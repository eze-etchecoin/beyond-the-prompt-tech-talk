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

> El orden y el alcance son provisionales; se ajustarán al ensayar los tiempos.

## Cómo correrlo

```bash
# Tests
dotnet test tests/Minesweeper/Minesweeper.Tests/Minesweeper.Tests.csproj

# Jugar (dificultad b|i|e y seed opcional para un tablero reproducible)
dotnet run --project src/Minesweeper/Minesweeper.Console -- b 42
```

Comandos en juego: `r <fila> <col>` revela, `f <fila> <col>` marca/desmarca, `q` sale.
