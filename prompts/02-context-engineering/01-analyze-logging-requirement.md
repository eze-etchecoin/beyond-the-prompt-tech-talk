# Context Engineering · 1 — Analizar el requerimiento de logging

Tenés acceso completo a este repositorio. **Antes de responder**, inspeccioná la solución existente de word wrapping:

- `src/ContextEngineering/WordWrap.Core/` — lógica de dominio
- `src/ContextEngineering/WordWrap.Console/` — aplicación de consola
- `tests/ContextEngineering/WordWrap.Tests/` — tests xUnit

## Tarea

Explicá cómo incorporarías **LOGGING** a esta aplicación.

## Requisitos de tu respuesta

1. **NO modifiques el código todavía.** Esto es un análisis, no una implementación.
2. Identificá qué archivos y componentes cambiarías, y por qué.
3. Compará **al menos DOS alternativas** de diseño, con ventajas y desventajas para una app de consola pequeña y didáctica. Por ejemplo:
   - `Microsoft.Extensions.Logging`
   - Generic Host con `Microsoft.Extensions.Hosting`
   - Un *decorator* alrededor del servicio de word wrapping
   - Un pipeline explícito de procesamiento
4. Respetá las convenciones y el estilo del repo (`.editorconfig`, los `.csproj`, cómo están escritos los tests).
5. Una app de consola **no tiene middleware de ASP.NET** por defecto. Si usás la palabra "middleware", aclará si te referís a Generic Host, a un decorator o a un pipeline propio.
6. Si falta alguna decisión de producto o arquitectura (destino de los logs, nivel de detalle, formato, dependencias aceptables), **formulá esas preguntas** en lugar de asumir una respuesta.

## Entregables

- Un análisis claro
- La lista de archivos afectados
- La comparación de alternativas
- Las preguntas abiertas

> Sin cambios de código.
