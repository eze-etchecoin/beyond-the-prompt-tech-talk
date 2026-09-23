# Prompt Engineering · 2 — Validación y casos límite

Partiendo del método `Wrap(string text, int maximumLineLength)` que escribiste, agregá validación de argumentos y manejo de casos límite:

1. Si `maximumLineLength <= 0` → lanzá `ArgumentOutOfRangeException`.
2. Si `text` es `null` → lanzá `ArgumentNullException`.
3. Si `text` está vacío o solo tiene espacios → devolvé una cadena vacía.
4. **Colapsá los espacios repetidos:** sin espacios sobrantes al inicio ni al final de cada línea, ni dobles espacios entre palabras.
5. **Definí y documentá** con un comentario qué ocurre cuando una sola palabra es más larga que `maximumLineLength` (ese caso no puede entrar en una línea).

**Entregá:** el método actualizado y una breve explicación de cada decisión sobre los casos límite. Sin dependencias externas.
