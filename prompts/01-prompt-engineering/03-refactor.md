# Prompt Engineering · 3 — Refactor y separación de responsabilidades

Ahora quiero una separación de responsabilidades más clara. Reorganizá el código así:

| Proyecto | Responsabilidad |
|---|---|
| **Biblioteca de clases** | Lógica de dominio del word wrapping en una clase reutilizable (p. ej. `WordWrapper`), **sin ninguna dependencia de consola**. |
| **Consola** | Entrada/salida: leer argumentos e imprimir el resultado, usando la biblioteca. |
| **Tests** | Proyecto aparte; los tests describen el **comportamiento observable**, no detalles internos de implementación. |

## Restricciones

Mantené la implementación simple y fácil de explicar en vivo: **sin logging, sin inyección de dependencias, sin hosting ni configuración.**

**Entregá:** la estructura de proyectos resultante y qué responsabilidad tiene cada uno.
