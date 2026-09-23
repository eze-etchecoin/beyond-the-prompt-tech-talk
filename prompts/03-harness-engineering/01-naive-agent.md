# Harness Engineering · Paso 0 — El "antes": agente sin arnés

> **Objetivo:** mostrar en vivo qué hace un agente cuando solo le pedimos código, sin lineamientos, sin plan, sin gates de validación ni criterio de cierre.
> Correr en `src/HarnessEngineering/` **antes** de que exista `AGENTS.md`.
> Claude Code ya trae su propio arnés por defecto (planifica, escribe tests, corre el build); las restricciones de abajo lo apagan a propósito para simular el agente "naive" de los inicios.

---

Implementá **"first-click safety"** en el Minesweeper: que la primera celda revelada nunca sea una mina.

Trabajá como un asistente de código básico, **sin proceso**:

- ❌ No hagas plan ni preguntas: andá directo a modificar el código.
- ❌ No escribas ni modifiques tests.
- ❌ No corras build, tests ni ningún comando: solo editá archivos.
- ❌ No revises convenciones del repo ni leas más archivos que los imprescindibles.
- ❌ No hagas commits, ramas ni toques el tablero de Trello.

Al terminar, respondé solo con **un resumen de una línea** de lo que cambiaste.
