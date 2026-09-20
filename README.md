# beyond-the-prompt-tech-talk

Material de la charla técnica **"Más allá del prompt: arquitecturas para agentes de IA en el ciclo de vida del software"**.

## Propósito

Este repositorio acompaña a la charla y funciona como soporte vivo de las demostraciones. Reúne la presentación, los prompts usados en vivo, el código de ejemplo, los tests y la documentación de apoyo. Está pensado para **crecer de forma incremental**: en esta primera iteración prioriza una estructura clara y una documentación mínima, no una plataforma completa de agentes.

## De qué trata la charla

La charla recorre una evolución conceptual sobre cómo colaboramos con agentes de IA a lo largo del ciclo de vida del software (SDLC). No se trata solo de "escribir mejores prompts", sino de diseñar el **entorno** en el que un agente opera: el contexto que recibe, las herramientas y reglas que lo enmarcan, los bucles de trabajo que ejecuta y la coordinación entre agentes especializados.

### Evolución conceptual

1. **Prompt Engineering** — construir una solución guiando al modelo paso a paso mediante prompts.
2. **Context Engineering** — darle al agente acceso al repositorio, tests y convenciones para que decida mejor.
3. **Harness Engineering** — dotar al agente de herramientas, reglas y validación (el "arnés" que lo enmarca).
4. **Loop Engineering** — bucles de trabajo autónomos con una condición de salida clara (p. ej. TDD + issues).
5. **Graph Engineering** — coordinación de agentes especializados y revisiones cruzadas.
6. **Integración de artefactos agénticos en el SDLC** — cómo todo lo anterior se inserta en el flujo real de desarrollo.

## Audiencia prevista

- **Developers** junior, semi-senior y senior.
- Perfiles de **distintos lenguajes** (los ejemplos usan C#/.NET, pero los conceptos son transversales).
- **QA, Data, DevOps y Business Analysis**: los paradigmas aplican a testing, pipelines de datos, automatización e ingeniería de requisitos.

## Mapa de carpetas

| Carpeta | Contenido | Relación con la charla |
| --- | --- | --- |
| `docs/presentation/` | Presentación PowerPoint y sus exportaciones (p. ej. PDF). | Soporte visual de la charla. |
| `docs/talk/` | Guion, runbook de demos y timing. | Preparación y ejecución en vivo. |
| `docs/references/` | Enlaces y material de referencia. | Lecturas de apoyo. |
| `prompts/01-prompt-engineering/` | Prompts progresivos para construir a mano. | Demo de Prompt Engineering. |
| `prompts/02-context-engineering/` | Prompt para que un agente inspeccione el repo. | Demo de Context Engineering. |
| `prompts/03-harness-engineering/` | Placeholder documentado. | Demo de Harness Engineering. |
| `prompts/04-loop-engineering/` | Placeholder documentado. | Demo de Loop Engineering. |
| `prompts/05-graph-engineering/` | Placeholder documentado. | Demo de Graph Engineering. |
| `src/ContextEngineering/` | Solución de word wrapping (Core + Console). | Código base de la demo de contexto. |
| `src/PromptEngineering/`, `src/HarnessEngineering/`, `src/LoopEngineering/`, `src/GraphEngineering/` | Placeholders documentados. | Ejemplos futuros. |
| `tests/ContextEngineering/` | Tests xUnit del word wrapping. | Describen el comportamiento del ejemplo. |
| `assets/images/`, `assets/diagrams/` | Imágenes y diagramas. | Recursos gráficos. |
| `scripts/` | Scripts auxiliares. | Automatización de apoyo. |

## Estado actual de cada ejemplo

| Paradigma           | Ejemplo                                   | Estado                    |
| ------------------- | ----------------------------------------- | ------------------------- |
| Prompt Engineering  | Construcción incremental mediante prompts | Preparación               |
| Context Engineering | Word wrapping y evolución del contexto    | Implementado inicialmente |
| Harness Engineering | Herramientas, reglas y validación         | Placeholder               |
| Loop Engineering    | GitHub Issues, TDD y condición de salida  | Placeholder               |
| Graph Engineering   | Agentes especializados y reviews          | Placeholder               |

## Requisitos

- **.NET SDK 10** (este repo se fijó a la versión disponible localmente vía [`global.json`](global.json), con `rollForward: latestFeature`).
- Verificá tu instalación con:

  ```bash
  dotnet --list-sdks
  ```

Todos los proyectos apuntan a `net10.0`, con *nullable reference types* e *implicit usings* habilitados.

## Comandos básicos

```bash
# Restaurar dependencias
dotnet restore

# Compilar toda la solución
dotnet build BeyondThePrompt.sln

# Ejecutar los tests
dotnet test BeyondThePrompt.sln

# Ejecutar la consola con valores por defecto (ideal para demo en vivo)
dotnet run --project src/ContextEngineering/WordWrap.Console

# Ejecutar la consola con argumentos propios: "<texto>" <largoMáximo>
dotnet run --project src/ContextEngineering/WordWrap.Console -- "un texto de ejemplo bastante largo" 15
```

## Advertencia sobre los ejemplos

> Los ejemplos de este repositorio **priorizan la claridad didáctica por sobre la complejidad de producción**. Están diseñados para explicarse en vivo: evitan abstracciones innecesarias, dependencias externas y configuración accidental. No los tomes como referencia de arquitectura lista para producción.

## Licencia

**Pendiente de definición.** Todavía no se eligió una licencia para este repositorio.
