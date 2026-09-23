# Loop Engineering · Paso 1 — Adaptar el arnés a modo loop

> **Objetivo:** reutilizar el arnés de Harness y convertirlo en un **bucle autónomo** que vacía "To Do", con validación automática y revisión asíncrona.
> **Requisito previo (manual):** copiar el `AGENTS.md` de la demo de Harness a `src/LoopEngineering/AGENTS.md`.
> Correr en una ventana de Claude Code abierta sobre `src/LoopEngineering/`.
> **Resultado esperado:** se actualiza `AGENTS.md` para modo loop. No se implementa ninguna historia todavía.

---

Ya existe un `AGENTS.md` (copiado de Harness). **Adaptalo a modo LOOP**, conservando las seis etapas (refinamiento → plan → implementación → testing → aprobación → cierre) pero cambiando cómo se validan y cómo interviene la persona:

### 1. Condición de salida (nuevo)
Iterar tomando cards de **"To Do"** del tablero "Beyond The Prompt tech talk" hasta que quede **VACÍA**. Ese es el único criterio de finalización.

### 2. Gates: de humanos a automáticos
Eliminar los dos gates humanos (Plan y Aprobación). Validación **automática**: `dotnet build` + `dotnet test` en verde + autochequeo de los criterios de aceptación. El agente no frena a pedir OK.

### 3. Revisión humana asíncrona vía PR
Al terminar una card: push de la rama y PR contra `main` (`gh pr create`); card en **"In Review/Testing"** con el comentario `LISTO PARA REVISIÓN: <resumen + link al PR + cómo probar>`, y el loop sigue con la siguiente. La persona revisa y mergea en paralelo. El repo tiene remoto `origin`.

### 4. Ramas y PRs
Rama `loop/<card-slug>` desde `main`; al terminar, push, PR y volver a `main`.
**Una card = una rama = un commit = un PR.**

### 5. Ciclo por card
1. Tomar la card de más arriba de "To Do" (por posición) → mover a **"In Progress"**.
2. **Refinamiento:** si es ambigua o depende de una decisión no resoluble → "In Review/Testing" con `BLOQUEADO: <razón>` y pasar a la siguiente (sin rama).
3. Crear rama `loop/<card-slug>` desde `main`.
4. Implementación TDD acotada al alcance; luego `dotnet build` + `dotnet test`.
5. **Cierre:**
   - ✅ Verde y criterios cumplidos → commit convencional, push, PR (`gh pr create`), volver a `main`, card a "In Review/Testing" con `LISTO PARA REVISIÓN...` + link al PR.
   - ❌ No se puede completar → descartar cambios, volver a `main` limpio, card a "In Review/Testing" con `BLOQUEADO: <razón>` (sin PR).
6. Continuar con la siguiente card.

### 6. Manejo de bloqueos
**Saltear y anotar:** las cards ambiguas o irrecuperables salen de "To Do", así la condición de salida siempre es alcanzable y el loop no se cuelga.

### 7. Guardrails (se mantienen)
- Nunca un commit con build o tests en rojo.
- No exceder el alcance de la card.
- Respetar el estilo del repo.
- No commitear secretos.

---

Conservá lo que siga aplicando y reescribí solo las secciones de gates/flujo/cierre. Al terminar, mostrame **qué cambió respecto del arnés de Harness**. No implementes ninguna card todavía.
