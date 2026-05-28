# GUIA DEL PROYECTO LIBRARYMANAGER

## 1. Descripcion del proyecto

LibraryManager es una API REST para gestionar una biblioteca: autores, categorias, libros, lectores y prestamos.

## 2. Objetivo de aprendizaje

Practicar ASP.NET Core Web API, Controllers, ActionResult<T>, IActionResult, Entity Framework Core, SQL Server, DbContext, DbSet, migraciones, DTOs, FluentValidation, servicios, repositorios, inyeccion de dependencias, async/await, LINQ, Swagger, appsettings.json, respuestas HTTP y mapeo manual.

## 3. Arquitectura usada

Arquitectura por capas simple con bibliotecas de clases:

- `LibraryManager.Api`: entrada HTTP.
- `LibraryManager.DTOs`: contratos de entrada y salida.
- `LibraryManager.Models`: entidades del sistema y enum.
- `LibraryManager.DataAccess`: DbContext y repositorios.
- `LibraryManager.Services`: reglas de aplicacion y mapeo manual.
- `LibraryManager.Validators`: validacion de DTOs.
- `LibraryManager.ExternalServices`: practica opcional con HttpClientFactory.

## 4. Que NO es este proyecto

No es Clean Architecture. No es Arquitectura Hexagonal. No usa CQRS, MediatR, AutoMapper, JWT ni pruebas automatizadas todavia.

## 5. Explicacion de cada proyecto de la solucion

`LibraryManager.Api` recibe peticiones HTTP, llama servicios y devuelve `Ok`, `CreatedAtAction`, `BadRequest`, `NotFound`, `Conflict` o `NoContent`.

`LibraryManager.Models` contiene `Author`, `Category`, `Book`, `Reader`, `Loan` y `LoanStatus`. No depende de ningun otro proyecto.

`LibraryManager.DTOs` contiene DTOs de entrada y salida. No depende de ningun otro proyecto.

`LibraryManager.DataAccess` contiene `LibraryContext`, interfaces de repositorios e implementaciones con EF Core.

`LibraryManager.Services` contiene interfaces y servicios. Aqui deben vivir reglas de negocio y mapeos manuales.

`LibraryManager.Validators` contiene validadores de FluentValidation para DTOs.

`LibraryManager.ExternalServices` es un modulo opcional para practicar `HttpClientFactory`.

## 6. Referencias entre proyectos

`Api` referencia `DTOs`, `Services`, `Validators`, `DataAccess` y `ExternalServices`.

`Services` referencia `DTOs`, `Models` y `DataAccess`.

`DataAccess` referencia `Models`.

`Validators` referencia `DTOs`.

`Models` y `DTOs` no referencian a nadie.

## 7. Entidades del sistema

`Author`: `AuthorId`, `Name`, `BirthDate`, `Books`.

`Category`: `CategoryId`, `Name`, `Description`, `Books`.

`Book`: `BookId`, `Title`, `Isbn`, `PublicationYear`, `AvailableCopies`, `AuthorId`, `Author`, `CategoryId`, `Category`, `Loans`.

`Reader`: `ReaderId`, `Name`, `Email`, `Phone`, `Loans`.

`Loan`: `LoanId`, `BookId`, `Book`, `ReaderId`, `Reader`, `LoanDate`, `ReturnDate`, `Status`.

`LoanStatus`: `Active`, `Returned`.

## 8. Relaciones entre entidades

Un `Author` tiene muchos `Book`.

Una `Category` tiene muchos `Book`.

Un `Book` tiene muchos `Loan`.

Un `Reader` tiene muchos `Loan`.

`Loan` relaciona `Book` y `Reader` para representar prestamos historicos.

## 9. DTOs requeridos

`AuthorDto`, `AuthorInsertDto`, `AuthorUpdateDto`.

`CategoryDto`, `CategoryInsertDto`, `CategoryUpdateDto`.

`BookDto`, `BookInsertDto`, `BookUpdateDto`.

`ReaderDto`, `ReaderInsertDto`, `ReaderUpdateDto`.

`LoanDto`, `LoanInsertDto`.

## 10. Endpoints requeridos

Authors:

- `GET /api/authors`
- `GET /api/authors/{id}`
- `POST /api/authors`
- `PUT /api/authors/{id}`
- `DELETE /api/authors/{id}`

Categories:

- `GET /api/categories`
- `GET /api/categories/{id}`
- `POST /api/categories`
- `PUT /api/categories/{id}`
- `DELETE /api/categories/{id}`

Books:

- `GET /api/books`
- `GET /api/books/{id}`
- `POST /api/books`
- `PUT /api/books/{id}`
- `DELETE /api/books/{id}`
- `GET /api/books/search?title=`
- `GET /api/books/by-author/{authorId}`
- `GET /api/books/by-category/{categoryId}`
- `GET /api/books/available`

Readers:

- `GET /api/readers`
- `GET /api/readers/{id}`
- `POST /api/readers`
- `PUT /api/readers/{id}`
- `DELETE /api/readers/{id}`

Loans:

- `GET /api/loans`
- `GET /api/loans/{id}`
- `GET /api/loans/active`
- `GET /api/loans/by-reader/{readerId}`
- `POST /api/loans`
- `PUT /api/loans/{id}/return`

ExternalContent opcional:

- `GET /api/external-content/books`
- `GET /api/external-content/quote`

## 11. Reglas de negocio

Authors:

- El nombre es obligatorio.
- El nombre debe tener minimo 2 caracteres.
- No se debe eliminar un autor si tiene libros asociados.

Categories:

- El nombre es obligatorio.
- El nombre no debe repetirse.
- No se debe eliminar una categoria si tiene libros asociados.

Books:

- El titulo es obligatorio.
- El ISBN es obligatorio.
- El ISBN no debe repetirse.
- `AvailableCopies` no puede ser negativo.
- `AuthorId` debe existir.
- `CategoryId` debe existir.
- No se debe eliminar un libro con prestamos activos.

Readers:

- El nombre es obligatorio.
- El email es obligatorio.
- El email debe tener formato valido.
- No se debe eliminar un lector con prestamos activos.

Loans:

- El libro debe existir.
- El lector debe existir.
- El libro debe tener copias disponibles.
- Al crear un prestamo, se reduce `AvailableCopies`.
- Al devolver un prestamo, se aumenta `AvailableCopies`.
- No se puede devolver dos veces el mismo prestamo.
- No se puede prestar un libro con 0 copias disponibles.

## 12. Validaciones esperadas

FluentValidation valida forma de datos: obligatorios, longitudes, email, ids mayores a 0 y numeros no negativos.

Los servicios validan reglas que requieren base de datos: duplicados, existencia de relaciones y prestamos activos.

## 13. Respuestas HTTP esperadas

- `200 OK` para consultas exitosas.
- `201 Created` para recursos creados.
- `204 NoContent` para eliminaciones y devoluciones sin cuerpo.
- `400 BadRequest` para validaciones incorrectas.
- `404 NotFound` cuando no exista un recurso.
- `409 Conflict` para ISBN repetido, categoria repetida o reglas de negocio incumplidas.

## 14. Flujo para crear un autor

`POST /api/authors` recibe `AuthorInsertDto`. El controller valida con FluentValidation. El service crea `Author`, llama repositorio, guarda y devuelve `AuthorDto`. El controller responde `CreatedAtAction`.

## 15. Flujo para crear una categoria

`POST /api/categories` recibe `CategoryInsertDto`. El service debe verificar que el nombre no exista. Si existe, responder `Conflict` desde el controller. Si no existe, guardar y devolver `CategoryDto`.

## 16. Flujo para crear un libro

`POST /api/books` recibe `BookInsertDto`. Validar titulo, ISBN, copias, `AuthorId` y `CategoryId`. El service debe verificar ISBN unico, autor existente y categoria existente. Luego guardar y devolver `BookDto`.

## 17. Flujo para registrar un lector

`POST /api/readers` recibe `ReaderInsertDto`. Validar nombre y email. El service puede verificar email unico. Luego guardar y devolver `ReaderDto`.

## 18. Flujo para crear un prestamo

`POST /api/loans` recibe `LoanInsertDto`. Verificar que libro y lector existan, que `AvailableCopies` sea mayor a 0, crear `Loan` con `Status Active`, reducir `AvailableCopies` y guardar cambios.

## 19. Flujo para devolver un prestamo

`PUT /api/loans/{id}/return` busca el prestamo. Si no existe, `NotFound`. Si ya fue devuelto, `Conflict`. Si esta activo, establecer `ReturnDate`, `Status Returned`, aumentar `AvailableCopies` y devolver `NoContent`.

## 20. Uso esperado de LINQ

En Books practicar:

- `Where`
- `Select`
- `OrderBy`
- `Any`
- `FirstOrDefaultAsync`
- `ToListAsync`

## 21. Uso esperado de async/await

Todos los accesos a base de datos y servicios externos deben usar `Task`, `async` y `await`.

## 22. Uso esperado de servicios

Los servicios coordinan reglas de negocio, llamadas a repositorios y mapeos manuales entre entidades y DTOs.

## 23. Uso esperado de repositorios

Los repositorios encapsulan consultas y persistencia con Entity Framework Core. Son especificos por entidad, no genericos todavia.

## 24. Uso esperado de FluentValidation

Los controllers inyectan `IValidator<TDto>`, validan DTOs y devuelven `BadRequest` si hay errores.

## 25. Uso opcional de HttpClientFactory

`ExternalServices` permite practicar `AddHttpClient`, `BaseAddress` desde `appsettings.json`, `HttpClient` y deserializacion JSON.

## 26. Paso a paso recomendado para construir el proyecto

Fase 1: Crear solucion, proyectos y referencias.

Fase 2: Crear entidades y relaciones.

Fase 3: Configurar DbContext y SQL Server.

Fase 4: Crear migracion inicial.

Fase 5: Implementar CRUD de Authors.

Fase 6: Implementar CRUD de Categories.

Fase 7: Implementar CRUD de Books.

Fase 8: Implementar DTOs y mapeos manuales.

Fase 9: Agregar FluentValidation.

Fase 10: Agregar servicios.

Fase 11: Agregar repositorios.

Fase 12: Crear CRUD de Readers.

Fase 13: Crear prestamos.

Fase 14: Devolver prestamos.

Fase 15: Agregar busquedas y filtros con LINQ.

Fase 16: Agregar practica opcional con HttpClientFactory.

Fase 17: Limpiar, refactorizar y revisar respuestas HTTP.

Fase 18: Refactor futuro con AutoMapper, pero no implementarlo todavia.

## 27. Checklist de avance

- [x] Solucion creada.
- [x] Proyectos base creados.
- [x] Referencias entre proyectos configuradas.
- [x] Entidades base creadas.
- [x] DTOs base creados.
- [x] DbContext creado.
- [x] Repositorios especificos creados.
- [x] Servicios base creados con TODOs.
- [x] Validadores base creados.
- [x] Controllers base creados.
- [ ] Migracion inicial creada.
- [ ] CRUD Authors implementado.
- [ ] CRUD Categories implementado.
- [ ] CRUD Books implementado.
- [ ] CRUD Readers implementado.
- [ ] Prestamos implementados.
- [ ] Devolucion de prestamos implementada.
- [ ] Conflicts de negocio implementados.

## 28. Refactors futuros

- AutoMapper.
- Repositorio generico.
- Middleware global de errores.
- Logging.
- Autenticacion.
- Pruebas.

## 29. Cosas que NO debo implementar todavia

No implementar AutoMapper, JWT, MediatR, CQRS, Clean Architecture, Arquitectura Hexagonal, pruebas automatizadas ni repositorio generico.

No resolver toda la logica en la maqueta inicial. Implementar paso a paso con calma y entendiendo cada capa.
