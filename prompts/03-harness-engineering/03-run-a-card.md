# Harness Engineering · Paso 2 — Pasar una card por el arnés

> **Objetivo:** mostrar cómo UNA card atraviesa las seis etapas de `AGENTS.md`, con los dos gates humanos (Plan y Aprobación).
> Correr en la misma ventana, con `AGENTS.md` ya creado.
> **Antes:** promover en Trello la card elegida de "Backlog" a "To Do".

---

Seguí el proceso definido en `AGENTS.md` **al pie de la letra**.

Tomá la card **"First-click safety"** del tablero "Beyond The Prompt tech talk" (lista "To Do") y hacela atravesar el arnés etapa por etapa:

1. **Refinamiento** — leé la card y sus criterios de aceptación; si algo es ambiguo, preguntame antes de seguir.
2. **Plan** — presentame el plan validado contra reglas y convenciones. 🛑 **FRENÁ** y esperá mi aprobación antes de codear.
3. **Implementación** — con TDD, una vez aprobado el plan.
4. **Testing** — corré build y tests; verificá los criterios de aceptación.
5. **Aprobación** — mostrame el diff y los resultados. 🛑 **FRENÁ** y esperá mi OK antes del cierre.
6. **Cierre** — al aprobar, commit en la rama `harness/<card-slug>` (desde `main`) y mové la card a **"Done"** en Trello.

> Recordá: no rompas la build ni los tests, no excedas el alcance de la card y respetá el estilo del repo.
