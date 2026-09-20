# Loop Engineering — workspace

Este directorio es un **workspace aislado** con su **propia copia** del Minesweeper
(`Minesweeper.Loop.*`, tests en `tests/LoopEngineering/`), sembrada desde el
producto base `src/Minesweeper/`.

## Dinámica de la demo

Se reutiliza el **`AGENTS.md`** definido en la demo de Harness, pero ahora el
agente trabaja **en bucle** sobre las *cards* del tablero de Trello: toma una
card, la implementa con **TDD**, corre `dotnet test` y, cuando pasa, la mueve a
*Done* y toma la siguiente. La **condición de salida** es que no queden cards
pendientes (o que se cumpla el criterio acordado).

> El `AGENTS.md` se trae de la demo de Harness; no se crea aquí de cero.

Backlog de cards y diseño en
[`docs/talk/minesweeper-demo.md`](../../docs/talk/minesweeper-demo.md).
