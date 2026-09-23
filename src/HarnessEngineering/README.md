# Harness Engineering — workspace

Este directorio es un **workspace aislado**: contiene su **propia copia** del
Minesweeper (`Minesweeper.Core` + `Minesweeper.Console`, con tests en
`tests/HarnessEngineering/`), sembrada desde el producto base `src/Minesweeper/`.
Proyectos y namespaces llevan el prefijo `Minesweeper.Harness.*` para no chocar
con las otras copias.

La idea es abrir una ventana de Claude Code / VS Code sobre esta carpeta y
trabajar acá sin afectar los demás enfoques.

## Dinámica de la demo

El "arnés" (lineamientos, herramientas y reglas para el agente) se va a definir
**en vivo**: con un prompt (guardado en
[`prompts/03-harness-engineering/`](../../prompts/03-harness-engineering/))
generaremos un **`AGENTS.md`** en este workspace que fije cómo debe trabajar el
agente (convenciones, `dotnet build`/`dotnet test`, uso del tablero de Trello,
criterio de "no romper la build ni los tests").

> Hay un `AGENTS.md` **base** que solo indica el acceso a Trello vía MCP y el
> tablero a usar; el arnés completo se agrega en vivo como parte de la demostración.

Ver el diseño general en
[`docs/talk/minesweeper-demo.md`](../../docs/talk/minesweeper-demo.md).
