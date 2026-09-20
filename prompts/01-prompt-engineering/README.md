# Prompt Engineering — prompts de la demo

Tres prompts **breves y progresivos**, pensados para una interacción **manual paso
a paso** con un asistente de IA. Cada uno se apoya en el resultado del anterior.

| Orden | Archivo | Intención |
| --- | --- | --- |
| 1 | [`01-initial-request.txt`](01-initial-request.txt) | Pedir una primera implementación de un algoritmo básico (word wrapping). |
| 2 | [`02-add-validation.txt`](02-add-validation.txt) | Agregar validaciones y casos límite. |
| 3 | [`03-refactor.txt`](03-refactor.txt) | Pedir una separación más clara de responsabilidades. |

## Cómo usarlos en vivo

1. Enviá el prompt 1 y revisá el resultado con la audiencia.
2. Enviá el prompt 2 sobre ese resultado; observá cómo cambian los casos límite.
3. Enviá el prompt 3 para llegar a una estructura de proyectos similar a la que
   ya vive en `src/ContextEngineering/`.

La idea es mostrar cómo, **solo con prompts**, se puede construir de forma
incremental — y dónde empiezan a aparecer las limitaciones que motivan el resto
de la charla.
