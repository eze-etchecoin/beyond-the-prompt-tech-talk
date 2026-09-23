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

## Modo de trabajo: LOOP

Este AGENTS.md define un modo de operación **loop**: el agente itera cards de
forma autónoma, sin frenar a pedir aprobación humana en el medio. La
validación de cada card es automática (build + tests + autochequeo de
criterios de aceptación) y la revisión humana ocurre **después**, de forma
asíncrona, sobre un PR abierto contra `main`.

### Condición de salida

El agente sigue iterando mientras la lista **"To Do"** del tablero tenga
cards. El loop termina cuando **"To Do" queda vacía**. Ese es el único
criterio de finalización — no hay límite de iteraciones ni checkpoint humano
intermedio.

### Gates: de humanos a automáticos

A diferencia de un flujo de tipo Harness (donde el plan y la aprobación final
los valida una persona), en modo loop **no hay gates humanos síncronos**:

- No se pide OK del plan antes de implementar.
- No se pide aprobación final antes de cerrar la card.
- El único gate es automático: `dotnet build` + `dotnet test` en verde, más un
  autochequeo del agente contra los criterios de aceptación de la card.

Si el gate automático no pasa, la card no se cierra con PR (ver manejo de
bloqueos).

### Revisión humana: asíncrona y no bloqueante (vía PR)

La revisión humana no desaparece: se mueve fuera del camino crítico del loop.

- Al terminar una card, el agente pushea la rama y abre un PR contra `main`
  con `gh pr create` (el repo tiene remoto `origin`).
- La card se deja en la lista **"In Review/Testing"** con un comentario:
  `LISTO PARA REVISIÓN: <resumen> — <link al PR> — cómo probar: <pasos>`.
- El loop sigue de inmediato con la siguiente card, sin esperar a que la
  persona revise o mergee el PR.
- La persona revisa y mergea (o pide cambios) en paralelo, fuera del loop.
- El entregable de cada card es **un PR abierto contra `main`**.

### Ramas y PRs (una por card)

- Cada card se trabaja en su propia rama, creada desde `main`:
  `loop/<card-slug>`.
- Regla: **una card = una rama = un commit = un PR**.
- Al terminar la card (con o sin éxito) se vuelve a `main` antes de tomar la
  siguiente.

### Ciclo por card

1. **Tomar** la card de más arriba de "To Do" (por posición) → moverla a
   "In Progress".
2. **Refinamiento**: si la card es ambigua o depende de una decisión que el
   agente no puede resolver por sí solo → moverla directamente a
   "In Review/Testing" con el comentario `BLOQUEADO: <razón>` y pasar a la
   siguiente card, **sin crear rama**.
3. **Rama**: crear `loop/<card-slug>` desde `main` (asegurarse de partir de
   `main` actualizado).
4. **Implementación (TDD, acotada al alcance)**:
   - Escribir el/los test(s) que expresan el criterio de aceptación.
   - Implementar lo mínimo necesario para pasarlos, sin exceder el alcance de
     la card.
   - Correr `dotnet build` + `dotnet test`.
5. **Cierre**:
   - Si build y tests están en verde y los criterios de aceptación se
     cumplen → commit convencional en la rama, push, `gh pr create` contra
     `main`, volver a `main`, y mover la card a "In Review/Testing" con
     `LISTO PARA REVISIÓN: <resumen + link al PR + cómo probar>`.
   - Si no se puede completar (build/tests en rojo, alcance irresoluble,
     etc.) → descartar los cambios de la rama, volver a `main` limpio, y
     mover la card a "In Review/Testing" con `BLOQUEADO: <razón>` (sin PR).
6. **Continuar** con la siguiente card de "To Do" hasta que la lista quede
   vacía.

### Manejo de bloqueos

Las cards ambiguas o irrecuperables **no se quedan trabadas en "To Do"**: se
sacan de ahí y se anotan en "In Review/Testing" con `BLOQUEADO: <razón>`. Así
la condición de salida (To Do vacía) siempre es alcanzable y el loop nunca se
cuelga esperando una decisión humana.

### Guardrails

- Nunca dejar un commit con build o tests en rojo.
- No exceder el alcance de la card.
- Respetar el estilo y las convenciones del repo existente.
- No commitear secretos.
