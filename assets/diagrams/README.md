# Diagramas

Diagramas de flujo que conceptualizan los tres enfoques con demo agéntica:
**Harness**, **Loop** y **Graph**. Las fuentes están en Mermaid (`.mmd`),
versionadas; las exportaciones estéticas (SVG/PNG) se guardan al lado o en
[`../images/`](../images/).

| Archivo | Conceptualiza | Idea central |
| --- | --- | --- |
| [`harness-flow.mmd`](harness-flow.mmd) | El **arnés** | 6 etapas + **2 gates humanos**; revisión **síncrona** (el agente frena y espera). |
| [`loop-flow.mmd`](loop-flow.mmd) | El **bucle** | Vacía "To Do" con gates **automáticos** (build+test); revisión **asíncrona** vía PR. |
| [`graph-flow.mmd`](graph-flow.mmd) | El **grafo** | Orquestador + subagentes con ciclos de feedback; review de **agentes** + aprobación **humana** del PR. |

## Convención de color (`classDef`)

Cada nodo lleva una clase para que el color sea consistente entre los tres
diagramas y fácil de trasladar a la PPT o de describirle a una IA de imágenes.
La paleta está alineada con la **estética corporativa de Avenga** (neutros
cálidos + rojo de marca), tomada del `theme` de las plantillas
`docs/presentation/*.pptx`:

| Clase | Significado | Color (fill / stroke) |
| --- | --- | --- |
| `stage` / `step` / `agent` | Etapa del proceso o agente | crema / taupe · `#F2ECE1` / `#8A7F6E` |
| `trello` | Lista del tablero de Trello | arena · `#E7DAC1` / `#B39B6A` |
| `orq` / `auto` | Orquestador / validación automática | dorado cálido · `#F5E0C4` / `#C68A3A` |
| `gate` / `human` | **Intervención humana** (gate o revisión) | **rojo de marca** · `#FBDED4` / `#DD2C00` |
| `pr` | Apertura/merge de **PR** | musgo · `#E4E6D0` / `#7E8A4E` |
| `blocked` | Card/historia bloqueada | óxido · `#F0D6CC` / `#B0553C` |

Referencia de marca: texto `#47433B`, fondo `#FBF8F3`, rojo Avenga `#DD2C00`,
coral `#FEA890`, taupe `#857B6D`, beige `#B2A893`.

> El rojo de marca queda **reservado para la persona** (`gate` y `human` nunca
> coexisten en un mismo diagrama). Un bloque `%%{init}%%` al inicio de cada
> `.mmd` pinta también los nodos sin clase, las flechas y las etiquetas en
> tonos cálidos. Los colores son editables: cambialos en el `classDef` (o en el
> `init`) de cada archivo, o al exportar.

## Cómo previsualizar / exportar

- **Rápido:** pegar el contenido en [mermaid.live](https://mermaid.live) y exportar SVG/PNG.
- **CLI (opcional):** `npx @mermaid-js/mermaid-cli -i harness-flow.mmd -o harness-flow.svg`
- **VS Code:** extensión *Markdown Preview Mermaid Support* o *Mermaid Editor*.
