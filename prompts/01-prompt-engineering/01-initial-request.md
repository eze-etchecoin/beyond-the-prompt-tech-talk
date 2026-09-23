# Prompt Engineering · 1 — Pedido inicial

Necesito un algoritmo de **word wrapping** (ajuste de línea) en C#.

## Objetivo

Escribí un método con la firma:

```csharp
string Wrap(string text, int maximumLineLength)
```

## Comportamiento esperado (primera versión)

- Dividir el texto en líneas cuya longitud no supere `maximumLineLength` caracteres.
- Cortar preferentemente **entre palabras** (en los espacios), no en medio de una palabra.
- Separar las líneas con saltos de línea.

## Restricciones

Mantenelo lo más simple posible: una sola clase, sin dependencias externas, sin logging ni configuración.

**Entregá:** solo el código del método y una breve explicación de cómo funciona.
