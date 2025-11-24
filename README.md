# 📚 Tabla de Contenidos
- [📚 Tabla de Contenidos](#-tabla-de-contenidos)
- [🎉 Meevent API](#-meevent-api)
- [🚀 Tecnologías utilizadas](#-tecnologías-utilizadas)
- [📦 Instalación](#-instalación)
- [🗄️ Configuración del entorno](#️-configuración-del-entorno)
- [▶️ Ejecutar el proyecto](#️-ejecutar-el-proyecto)
- [🧱 Estructura del proyecto](#-estructura-del-proyecto)
- [📐 Convenciones del proyecto](#-convenciones-del-proyecto)
- [🌱 Flujo de trabajo (GitHub Flow)](#-flujo-de-trabajo-github-flow)
- [📌 Convenciones de ramas y commits](#-convenciones-de-ramas-y-commits)
    - [🧩 Tipos de ramas](#-tipos-de-ramas)
    - [🟩 Tipos de Commit](#-tipos-de-commit)
- [🤝 Colaboradores](#-colaboradores)


# 🎉 Meevent API

La **Meevent API** es el backend oficial de Meevent.  
Provee servicios REST para gestionar usuarios, eventos, categorías, autenticación y más.

Construida con **.NET 8** siguiendo arquitectura por features, buenas prácticas y una configuración estandarizada de nombres y capas.



# 🚀 Tecnologías utilizadas

- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core 8
- SQL Server
- Swagger


# 📦 Instalación

Clona el repositorio:

```bash
git clone https://github.com/LP-React/Meevent-API.git
```

Instala dependencias:

```shell
dotnet restore
```


# 🗄️ Configuración del entorno

Configura la cadena de conexión en **appsettings.Development.json**:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MeeventDb;Trusted_Connection=True;Encrypt=False"
  }
}
```

# ▶️ Ejecutar el proyecto

Crear la base de datos:

```
dotnet ef database update
```

Ejecutar la API:

```
dotnet run
```

# 🧱 Estructura del proyecto

Implementamos Vertical Slice Architecture, separando la aplicación por features.

```
/src
   /Features
      /Users
         UsersController.cs
         UserService.cs
         UserDto.cs
         UserEntity.cs
         UserMapping.cs
         UserValidators.cs
      /Events
         EventsController.cs
         EventService.cs
         EventDto.cs
         EventEntity.cs
         EventMapping.cs
   /Core
      /Context
         AppDbContext.cs
      /Entities
         (Entidades compartidas)
      /Interfaces
      /Migrations
```


# 📐 Convenciones del proyecto

| Tipo / Elemento          | Convención                              | Ejemplo / Descripción / Uso                    |
| ------------------------ | --------------------------------------- | --------------------------------------------- |
| **Entity**               | PascalCase, singular                    | `User`, `EventCategory`                       |
| **DbSet**                | Plural                                  | `Users`, `EventCategories`                    |
| **Propiedad Entity**     | PascalCase                              | `CreatedAt`, `CategoryId`                     |
| **DTO Request**          | PascalCase (código) → snake_case (JSON) | `CreateUserRequest` → `{ "created_at": "..." }` |
| **DTO Response**         | PascalCase (código) → snake_case (JSON) | `UserResponse` → `{ "user_id": 1 }`          |
| **Controller**           | Clase PascalCase, ruta plural kebab-case| `UsersController` → `/api/users`             |
| **Commands / Queries**   | PascalCase                              | `CreateUserCommand`, `GetEventsQuery`        |
| **Servicios / UseCases** | PascalCase + Async                       | `CreateUserAsync()`                           |
| **Tabla DB**             | snake_case, plural                       | `users`, `event_categories`                   |
| **Columna DB**           | snake_case                               | `created_at`, `category_id`                   |
| **Foreign Key**          | snake_case + `_id`                       | `user_id`, `event_id`                         |
| **Rutas API**            | kebab-case en segmentos                  | `/api/event-categories/{category_id}`        |
| **Archivos**             | PascalCase                               | `UserMappings.cs`, `EventEntity.cs`           |




# 🌱 Flujo de trabajo (GitHub Flow)

1. Crear una rama desde main

    ```bash
    git checkout -b feature/nombre-de-la-funcionalidad
    ```

2. Hacer commits siguiendo las [convenciones](#-convenciones-de-ramas-y-commits).

    ```bash
    <tipo>: <descripción breve>
    ```
    
3. Subir la rama al repositorio remoto.

    ```bash
    git push origin -u feature/nombre-de-la-funcionalidad
    ```

4. Crear un Pull Request en Github.

5. Solicitar revisión para futuro merge.


# 📌 Convenciones de ramas y commits

### 🧩 Tipos de ramas

| Tipo de rama | Formato                | Uso                                       |
| ------------ | ---------------------- | ----------------------------------------- |
| **feature**  | `feature/<nombre>`     | Nuevas funcionalidades                    |
| **fix**      | `fix/<bug>`            | Corrección de errores                     |
| **hotfix**   | `hotfix/<descripcion>` | Arreglos urgentes                         |
| **refactor** | `refactor/<modulo>`    | Mejora interna sin cambiar comportamiento |
| **docs**     | `docs/<tema>`          | Solo documentación                        |
| **chore**    | `chore/<tarea>`        | Configuración, scripts, mantenimiento     |


### 🟩 Tipos de Commit

| Tipo         | Descripción                         |
| ------------ | ----------------------------------- |
| **feat**     | Nueva funcionalidad                 |
| **fix**      | Corrección de errores               |
| **docs**     | Documentación                       |
| **style**    | Formato (sin lógica)                |
| **refactor** | Refactor sin cambiar comportamiento |
| **perf**     | Mejora de rendimiento               |
| **test**     | Añadir o modificar tests            |
| **chore**    | Configuraciones o tareas menores    |
| **build**    | Cambios en dependencias o build     |
| **ci**       | Cambios en CI/CD                    |

# 🤝 Colaboradores

- **[LP-React](https://github.com/LP-React)**
- **[Frxnco06](https://github.com/Frxnco06)**
- **[GAlopezdev](https://github.com/GAlopezdev)**
- **[Monsteralan123](https://github.com/Monsteralan123)**
- **[angelessvargas](https://github.com/angelessvargas)**