# Loop Engineering · Paso 2 — Disparar el bucle

> **Requisito:** `AGENTS.md` ya adaptado a modo loop en `src/LoopEngineering/` (paso 1).
> **Antes:** promover en Trello las cards a trabajar de "Backlog" a "To Do".
> Correr en la ventana abierta sobre `src/LoopEngineering/`.

---

Trabajá en **MODO LOOP** siguiendo el flujo de `src/LoopEngineering/AGENTS.md`.

Iterá tomando cards de **"To Do"** del tablero "Beyond The Prompt tech talk" **hasta que quede VACÍA**. Por cada card seguí el ciclo del `AGENTS.md`:

- Rama `loop/<card-slug>`
- TDD
- Build + test
- Cierre asíncrono: PR contra `main` y card en **"In Review/Testing"**

**Sin frenar a pedir aprobación humana.** Si una card es ambigua o irrecuperable, marcala **BLOQUEADA** y seguí.

## Al terminar

Cuando "To Do" quede vacía, frená y dame un resumen:

- ✅ Cards **LISTAS PARA REVISIÓN** (con link a su PR)
- ⛔ Cards **BLOQUEADAS** (con su razón)
- 🌿 Ramas / PRs generados
