# Harness Engineering — prompts

Demo del **arnés**: cómo evolucionamos de *"un agente que escribe código"* a un
proceso explícito donde cada requerimiento **atraviesa etapas con validación y
aprobación humana**.

Se trabaja en el workspace [`src/HarnessEngineering/`](../../src/HarnessEngineering/)
(copia aislada del Minesweeper) y sobre el tablero de Trello
*Beyond The Prompt tech talk*.

## El arnés en 6 etapas

```
Refinamiento → Plan (GATE humano) → Implementación → Testing → Aprobación (GATE humano) → Cierre
```

- **Refinamiento** — leer la card, aclarar criterios, preguntar ante ambigüedad. Sin código.
- **Plan de implementación con validación** — plan (archivos, tests, enfoque, riesgos) validado contra las reglas. **Gate humano** antes de codear.
- **Implementación** — TDD, cambios acotados a la card.
- **Testing** — `dotnet build` + `dotnet test` en verde; verificar criterios y casos límite.
- **Aprobación humana** — revisar diff + resultados. **Gate humano** antes del cierre.
- **Cierre** — rama `harness/<card-slug>` + commit convencional + mover la card a *Done* en Trello. En esta demo el entregable es la rama local (no abrimos PR: la revisión ya ocurre en los dos gates; en un flujo de equipo, acá se abriría el PR).

## Prompts (orden de la demo)

| Orden | Archivo | Rol |
| --- | --- | --- |
| 0 | [`01-naive-agent.txt`](01-naive-agent.txt) | El **"antes"**: pedir la feature a secas, sin arnés (para contrastar). |
| 1 | [`02-generate-agents-md.txt`](02-generate-agents-md.txt) | Meta-prompt que genera el **`AGENTS.md`** con el arnés y sus etapas. |
| 2 | [`03-run-a-card.txt`](03-run-a-card.txt) | Pasar **una card** por el arnés, etapa por etapa, con los dos gates. |

## Flujo sugerido en vivo

1. (Opcional) Correr `01` para mostrar el comportamiento sin arnés.
2. Correr `02` → se crea `src/HarnessEngineering/AGENTS.md` (no se implementa nada aún).
3. Promover en Trello una card de *Backlog* a *To Do*.
4. Correr `03` → la card atraviesa las 6 etapas; el agente frena en Plan y en
   Aprobación esperando tu OK; al final commitea y mueve la card a *Done*.

> El `AGENTS.md` **no está versionado en el repo**: se crea en vivo como parte de
> la demostración (es justamente lo que muestra el paso 1).
