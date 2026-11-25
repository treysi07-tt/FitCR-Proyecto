# Fit.CR — Sistema de gestión de gimnasio

## Descripción del proyecto

Fit.CR es una aplicación de escritorio desarrollada en C# utilizando Windows Forms, diseñada para facilitar la gestión de las actividades de un gimnasio.

El sistema permite que los clientes puedan llevar un seguimiento de sus rutinas, visitas, medidas corporales, objetivos personales, clases grupales e insignias obtenidas.

También cuenta con funcionalidades para entrenadores personales y entrenadores de gimnasio, quienes pueden gestionar clientes, programar clases, registrar asistencias y consultar estadísticas generales.

## Objetivo del proyecto

Desarrollar una aplicación que permita organizar y administrar las actividades de un gimnasio, aplicando los conocimientos adquiridos durante el curso de programación, incluyendo programación orientada a objetos, principios SOLID, arquitectura MVC y patrones de diseño.

## Tecnologías utilizadas

- C#
- .NET 10
- Windows Forms
- JSON para el almacenamiento de datos
- LINQ para consultas y procesamiento de información
- Git y GitHub para el control de versiones
- Trello para la planificación y seguimiento de tareas

## Tipos de usuarios

### Cliente

- Registro e inicio de sesión.
- Creación, consulta, edición y eliminación de rutinas.
- Registro de rutinas completadas.
- Consulta de las últimas cinco rutinas.
- Registro e historial de visitas al gimnasio.
- Registro de invitados.
- Seguimiento de medidas corporales.
- Cálculo del índice de masa corporal (IMC).
- Creación y seguimiento de objetivos personales.
- Obtención automática de insignias positivas y negativas.
- Consulta de notificaciones.
- Inscripción y participación en clases grupales.

### Entrenador personal

- Inicio de sesión.
- Gestión de clientes asignados.
- Consulta del progreso de los clientes.
- Asignación y consulta de rutinas.
- Consulta de objetivos personales.
- Consulta de estadísticas generales.
- Gestión de clases cuando tiene una actividad asignada.

### Entrenador de gimnasio

- Inicio de sesión.
- Programación y cancelación de clases.
- Consulta de participantes.
- Registro de asistencias.
- Consulta de estadísticas generales.

## Clases disponibles

El sistema permite gestionar cinco tipos de actividades:

- Yoga.
- Zumba.
- Spinning.
- CrossFit.
- Gimnasio abierto.

## Arquitectura del proyecto

El proyecto utiliza una organización basada en el patrón arquitectónico MVC (Modelo, Vista y Controlador), complementada con servicios y repositorios para separar las responsabilidades de cada componente.

| Carpeta | Responsabilidad |
|---|---|
| Models | Contiene las entidades y estructuras de datos del sistema. |
| Views | Contiene los formularios y las interfaces gráficas. |
| Controllers | Gestiona las operaciones y validaciones del sistema. |
| Interfaces | Define los contratos utilizados por los componentes. |
| Repositories | Gestiona la lectura y escritura de los archivos JSON. |
| Services | Contiene funcionalidades complementarias y procesos reutilizables. |
| Data | Contiene los datos iniciales de demostración. |

## Programación orientada a objetos

Durante el desarrollo se aplicaron los siguientes conceptos:

- **Encapsulamiento:** organización de los atributos y comportamientos dentro de las clases.
- **Herencia:** utilización de clases base para compartir características entre los diferentes tipos de usuarios.
- **Polimorfismo:** implementación de comportamientos específicos según el tipo de usuario.
- **Abstracción:** utilización de clases abstractas e interfaces para representar comportamientos y contratos comunes.

## Principios SOLID

El sistema incorpora principios SOLID mediante la separación de responsabilidades, la utilización de interfaces, la reutilización de componentes y la dependencia de abstracciones.

Los controladores, modelos, servicios, repositorios y formularios poseen responsabilidades diferenciadas para facilitar el mantenimiento del código.

## Patrón de diseño Repository

Se implementó el patrón Repository mediante la interfaz genérica `IRepositorio<T>` y la clase `RepositorioJson<T>`.

Este patrón permite separar la lógica de almacenamiento de datos de las operaciones realizadas por los controladores, facilitando la reutilización del código para diferentes entidades.

## Seguridad

Las contraseñas no se almacenan directamente como texto plano en los archivos JSON.

El sistema utiliza PBKDF2 con SHA-256 y una sal aleatoria para generar y verificar los hashes de las contraseñas.

También se realizan validaciones para evitar nombres de usuario duplicados y accesos con credenciales incorrectas.

## Almacenamiento de datos

La información del sistema se almacena mediante archivos JSON, sin necesidad de instalar un servidor de bases de datos.

El proyecto incluye datos iniciales de demostración que permiten comprobar las funcionalidades desde la primera ejecución.

| Información | Cantidad de registros iniciales |
|---|---:|
| Clientes | 50 |
| Entrenadores personales | 41 |
| Entrenadores de gimnasio | 41 |
| Rutinas | 350 |
| Visitas al gimnasio | 1500 |
| Medidas corporales | 300 |
| Objetivos | 150 |
| Invitados | 100 |
| Clases | 100 |
| Inscripciones a clases | 300 |
| Asistencias a clases | 200 |
| Insignias obtenidas | 1319 |
| Notificaciones | 4633 |
| **Total** | **9084** |

Los 13 archivos JSON contienen al menos 40 registros cada uno. Además, cada cliente de demostración cuenta con siete rutinas registradas.

## Requisitos de ejecución

- Sistema operativo Windows.
- SDK de .NET 10.
- Git, si se desea clonar el repositorio.
- Visual Studio o Visual Studio Code para consultar y modificar el código fuente.

## Instalación y ejecución

1. Clonar el repositorio:

```powershell
git clone https://github.com/treysi07-tt/FitCR-Proyecto.git
```

2. Ingresar a la carpeta del proyecto:

```powershell
cd FitCR-Proyecto
```

3. Restaurar las dependencias:

```powershell
dotnet restore
```

4. Compilar el proyecto:

```powershell
dotnet build
```

5. Ejecutar la aplicación:

```powershell
dotnet run
```

Durante la primera ejecución, el sistema copia automáticamente los archivos JSON iniciales a la carpeta de almacenamiento utilizada por la aplicación.

Las modificaciones realizadas posteriormente se conservan al cerrar y volver a ejecutar el sistema.

## Usuarios de demostración

Todas las siguientes cuentas utilizan la contraseña `FitCR1234`.

| Usuario | Tipo de cuenta |
|---|---|
| cliente_demo_001 | Cliente con alta participación |
| cliente_demo_015 | Cliente con participación intermedia |
| cliente_demo_050 | Cliente inactivo |
| personal_demo_001 | Entrenador personal |
| gimnasio_demo_001 | Entrenador de Yoga |
| gimnasio_demo_002 | Entrenador de Zumba |
| gimnasio_demo_003 | Entrenador de Spinning |
| gimnasio_demo_004 | Entrenador de CrossFit |
| gimnasio_demo_005 | Entrenador de Gimnasio abierto |
| carlosfit | Entrenador personal |
| andreafit | Entrenadora de gimnasio |

Las cuentas y contraseñas compartidas se incluyen exclusivamente para facilitar la demostración académica del sistema.

## Estadísticas

El sistema permite consultar estadísticas mediante filtros por rangos de fechas, incluyendo:

- Clientes con mayor y menor asistencia.
- Cumplimiento de rutinas.
- Cumplimiento de objetivos.
- Cambios en el peso corporal.
- Participación y asistencia a clases.
- Resumen general de las actividades del gimnasio.

## Insignias y notificaciones

Fit.CR cuenta con un catálogo de 38 insignias relacionadas con los hábitos, actividades, objetivos y participación de los clientes.

Las insignias se evalúan automáticamente según las condiciones establecidas en el sistema.

Las notificaciones permiten consultar acontecimientos relacionados con las rutinas, visitas, invitados, medidas corporales, objetivos, clases e insignias.

## Limitaciones

- La aplicación está diseñada para ejecutarse en Windows.
- El almacenamiento mediante JSON está orientado a una aplicación local.
- No se implementó una base de datos centralizada para el acceso simultáneo desde diferentes computadoras.
- Las notificaciones se sincronizan al ingresar al menú del cliente y al regresar de las funcionalidades correspondientes.
- Las cuentas de demostración utilizan una contraseña compartida con fines académicos.

## Control de versiones y planificación

Durante el desarrollo se utilizó Git y GitHub para mantener un historial de los cambios realizados.

La planificación del proyecto se organizó mediante épicas, funcionalidades, elementos del backlog y tareas, utilizando Trello como herramienta de seguimiento.

## Contexto académico

Proyecto académico desarrollado para la Universidad Politécnica Internacional (UPI), con el propósito de aplicar los conocimientos adquiridos durante el curso de programación mediante el desarrollo de una aplicación funcional de escritorio.