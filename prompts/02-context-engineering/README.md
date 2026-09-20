# Context Engineering — prompt de la demo

## Estado inicial de la demostración

La solución de word wrapping que vive en `src/ContextEngineering/` y
`tests/ContextEngineering/` representa el **estado inicial** de esta demo. Es una
implementación deliberadamente simple: sin logging, sin inyección de dependencias,
sin hosting ni configuración.

## En qué consiste la demo (futura)

La demostración consistirá en **darle a un agente acceso a este repositorio** y
preguntarle **cómo incorporar logging** a la aplicación. El prompt de partida es
[`01-analyze-logging-requirement.txt`](01-analyze-logging-requirement.txt).

El objetivo es mostrar que el agente **toma una mejor decisión cuando puede
inspeccionar** el repositorio, los tests, las convenciones (`.editorconfig`,
`.csproj`) y el estado actual del código — en contraste con responder "a ciegas"
solo a partir del prompt.

> Importante: en esta etapa **no se implementa el logging todavía**. La demo se
> centra en el *análisis* y la *comparación de alternativas*.

## Posibles caminos futuros (sin elegir uno aún)

- **`Microsoft.Extensions.Logging`** — la abstracción de logging estándar de .NET.
- **Generic Host** con **`Microsoft.Extensions.Hosting`** — para obtener DI,
  configuración y ciclo de vida de forma integrada.
- **Un decorator** alrededor del servicio de word wrapping — para añadir logging
  sin tocar la lógica de dominio.
- **Un pipeline explícito de procesamiento** — encadenar pasos de forma visible.

## Nota sobre la palabra "middleware"

Una aplicación de **consola no tiene middleware de ASP.NET por defecto**. El
concepto de "middleware" pertenece al pipeline HTTP de ASP.NET Core. Si durante la
demo usamos la palabra "middleware", tendremos que **definir explícitamente** a qué
nos referimos:

- al **Generic Host** (`Microsoft.Extensions.Hosting`),
- a un **decorator** sobre el servicio, o
- a un **pipeline propio** de procesamiento que construyamos a mano.

Aclarar esto evita confusiones entre el modelo mental de ASP.NET y lo que realmente
aplica a una app de consola.
