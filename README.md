#  Bullet Hell Game - Unity 2D

Un juego bullet hell 2D desarrollado en Unity con mecánicas de progresión, sistema de mejoras y múltiples patrones de disparo enemigos.


## Controles

| Acción | Tecla |
|--------|-------|
| **Movimiento** | WASD / Flechas |
| **Modo Preciso** | Left Shift (mantener) |
| **Disparar** | Clic Izquierdo (mantener) |


### Tipos de Enemigos

#### Base Enemy
- **Vida**: 20 HP
- **Patrón**: Disparo directo al jugador
- **Movimiento**: Estático


#### Alternate Shooter
- **Vida**: 30 HP
- **Patrón**: Alterna disparos izquierda/derecha
- **Movimiento**: Patrulla horizontal y vertical


#### Circle Shooter
- **Vida**: 20 HP
- **Patrón**: Círculo de 16 balas rotatorio
- **Movimiento**: Patrulla horizontal y vertical


#### Spiral Shooter
- **Vida**: 20 HP
- **Patrón**: Doble espiral (horaria + antihoraria)
- **Movimiento**: Patrulla horizontal y vertical


#### V-Shooter
- **Vida**: 20 HP
- **Patrón**: Disparo en V hacia el jugador
- **Movimiento**: Estático


####  Wave Shooter
- **Vida**: 25 HP
- **Patrón**: Onda sinusoidal de balas
- **Movimiento**: Patrulla horizontal y vertical


### Jefes

####  Mini Boss 1
- **Vida**: 120 HP
- **Patrón**: Espiral doble continua
- **Movimiento**: Patrulla horizontal lenta


####  Boss Final
- **Vida**: 200 HP
- **Patrones**:
  - **Fase 1** : Círculo expansivo de 25 balas
  - **Fase 2** : 4 balas dirigidas
  - **Fase 3** : spread
- **Movimiento**: Patrulla horizontal



### Engine y Lenguajes
- **Unity**
- **C#**



## Uso de IA

### Uso de Inteligencia Artificial

Este proyecto ha utilizado asistencia de IA en las siguientes areas:

#### Sistema de Fases de Niveles
La arquitectura y lógica del sistema de progresión por fases fue desarrollada con asistencia de IA:
- Diseño de la estructura de WaveManager con soporte para varios niveles
- Sistema de tienda con 3 fases diferenciadas
- Lógica de transición entre niveles
- Sistema de detección de completado de oleadas

**Razón del uso de IA**: 
- Acelerar el diseño de la arquitectura del sistema de progresión
- Optimizar la lógica de gestión de estados del juego
- Implementar patrones de diseño robustos y escalables


