# LibraryManager

API REST de practica para gestionar una biblioteca, libros, lectores y prestamos.

Este proyecto esta intencionalmente incompleto. La idea es implementar cada capa paso a paso para practicar Programacion Backend en .NET con una arquitectura por capas simple.

## Requisitos

- SDK de .NET 10.
- SQL Server o SQL Server Express.
- Visual Studio, Rider o VS Code.

## Como correr la API

```powershell
dotnet restore LibraryManager.sln --ignore-failed-sources
dotnet build LibraryManager.sln --ignore-failed-sources
dotnet run --project LibraryManager.Api
```

## Swagger

Al correr la API en Development, abre:

```text
/swagger
```

## Cadena de conexion

Configura `LibraryManager.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=LibraryManagerDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

## Comandos utiles

```powershell
dotnet ef migrations add InitialCreate --project LibraryManager.DataAccess --startup-project LibraryManager.Api
dotnet ef database update --project LibraryManager.DataAccess --startup-project LibraryManager.Api
```

## Nota

No hay AutoMapper, JWT, MediatR, CQRS, Clean Architecture, Arquitectura Hexagonal ni pruebas automatizadas todavia. Los TODOs marcan el camino de implementacion.
