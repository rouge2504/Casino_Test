# Slot Machine / Tragamonedas

> Unity version: 2023.0.23f1

A slot machine game project developed in Unity. It supports customizable columns, symbols, and a dynamic paytable system based on symbols or patterns.

Un proyecto de tragamonedas desarrollado en Unity. Soporta columnas personalizables, símbolos y un sistema de pagos dinámico basado en símbolos o patrones.

---

## Column Creation / Creación de Columnas

EN:  
To create a new column, simply add new prefabs to the `Symbols` list in the `SlotColumn` script. These prefabs represent the possible symbols that can appear in each column during a spin.

ES:  
Para crear una nueva columna, simplemente agrega nuevos prefabs a la lista `Symbols` en el script `SlotColumn`. Estos prefabs representan los símbolos que pueden aparecer en cada columna durante un giro.

---

## Symbol Prefab Requirements / Requisitos del Prefab de Símbolo

EN:  
Each symbol prefab must include:
- The `Symbol` component
- An `ID` (for logic)
- A `Sprite` (for visuals)

ES:  
Cada prefab de símbolo debe tener:
- El componente `Symbol`
- Un `ID` (para la lógica)
- Un `Sprite` (para lo visual)

---

## Column Behavior / Comportamiento de Columnas

EN:  
Each column supports:
- An `AnimationCurve` for easing
- A spin duration setting

ES:  
Cada columna permite:
- Una `AnimationCurve` para suavidad del giro
- Una duración de giro personalizada

---

## SpinManager

EN:  
The `SpinManager` controls all columns.  
Columns must be ordered from left to right in the Inspector for proper spin sequencing.

ES:  
El `SpinManager` controla todas las columnas.  
Las columnas deben estar ordenadas de izquierda a derecha en el Inspector para que el giro secuencial funcione correctamente.

---

## Paytable System / Sistema de Tabla de Pagos

EN:  
The `Paytable` (a `ScriptableObject`) supports two reward types:

1. Symbol-Based Rewards
   - Defined in the `Entries` list
   - Each entry includes: `symbol ID`, `match count`, and `reward value`

2. Pattern-Based Rewards
   - Uses a 2D matrix (`IntMatrix2D`) to define winning shapes
   - `1` = must match the symbol
   - `0` = ignore cell  
   Note: `1` is not an ID, just a marker

ES:  
La `Paytable` (un `ScriptableObject`) soporta dos tipos de recompensas:

1. Recompensas por símbolo
   - Definidas en la lista `Entries`
   - Cada entrada incluye: `ID del símbolo`, `cantidad`, y `valor del premio`

2. Recompensas por patrón
   - Usa una matriz 2D (`IntMatrix2D`) para definir formas ganadoras
   - `1` = debe coincidir con el símbolo
   - `0` = se ignora la celda  
   Nota: El `1` no es un ID, solo un marcador

---

## Project Structure / Estructura del Proyecto

