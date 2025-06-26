# FitodietCalc

Aplicación de escritorio para cálculo dietético profesional, desarrollada en C# y WPF (.NET 8).

## Estado actual

- Fase 1: Cálculo de necesidades calóricas con dos fórmulas (Harris-Benedict y Mifflin-St Jeor).
- Gestión básica de pacientes y evaluaciones.

## Estructura del proyecto

- Models: Definición de entidades (Paciente, Evaluacion).
- ViewModels: Lógica de presentación y binding.
- Views: Interfaces de usuario (pantallas y formularios).
- Services: Cálculos energéticos y lógica reutilizable.
- Data: Repositorio y contexto SQLite.
- Helpers: Funciones auxiliares (ej. cálculo IMC).
- Resources: Recursos estáticos (estilos, imágenes).

## Próximos pasos

- Implementar formularios de entrada de datos.
- Añadir persistencia SQLite.
- Planificación de dietas (Fase 2).
