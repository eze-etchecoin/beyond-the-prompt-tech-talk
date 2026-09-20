# Runbook de demos

Guion operativo de cada demostración. Se completará de forma incremental. Para
Loop y Graph todavía **no** se inventan implementaciones: solo se registran los
huecos a definir.

---

## Demo 1 — Prompt Engineering

- **Estado inicial:** sin código; solo los prompts de `prompts/01-prompt-engineering/`.
- **Objetivo:** construir un algoritmo básico guiando al modelo paso a paso, agregando validaciones y luego separando responsabilidades.
- **Archivos / herramientas:** `prompts/01-prompt-engineering/01-initial-request.txt`, `02-add-validation.txt`, `03-refactor.txt`; un asistente de IA en modo interactivo.
- **Resultado esperado:** una primera implementación funcional obtenida en tres pasos manuales.
- **Plan de contingencia:** si el modelo se desvía, retroceder al prompt anterior y mostrar el resultado ya versionado.

---

## Demo 2 — Context Engineering

- **Estado inicial:** la solución de word wrapping ya implementada (`src/ContextEngineering/`, `tests/ContextEngineering/`).
- **Objetivo:** pedirle a un agente **con acceso al repo** cómo incorporar logging, comparando alternativas, sin modificar código todavía.
- **Archivos / herramientas:** `prompts/02-context-engineering/01-analyze-logging-requirement.txt`; el repositorio completo como contexto.
- **Resultado esperado:** un análisis que identifica archivos a tocar, compara al menos dos alternativas y formula preguntas de producto/arquitectura pendientes.
- **Plan de contingencia:** si el agente propone cambiar código, recordarle la restricción "no modificar todavía" y volver a enfocar en el análisis.

---

## Demo 3 — Harness Engineering

- **Estado inicial:** _por definir._
- **Objetivo:** _por definir_ (herramientas, reglas y validación como arnés del agente).
- **Archivos / herramientas:** _por definir._
- **Resultado esperado:** _por definir._
- **Plan de contingencia:** _por definir._

---

## Demo 4 — Loop Engineering

- **Estado inicial:** _por definir._
- **Objetivo:** _por definir_ (bucle autónomo con condición de salida: issues + TDD).
- **Archivos / herramientas:** _por definir._
- **Resultado esperado:** _por definir._
- **Plan de contingencia:** _por definir._

---

## Demo 5 — Graph Engineering

- **Estado inicial:** _por definir._
- **Objetivo:** _por definir_ (agentes especializados y reviews).
- **Archivos / herramientas:** _por definir._
- **Resultado esperado:** _por definir._
- **Plan de contingencia:** _por definir._
