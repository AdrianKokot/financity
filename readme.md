# Financity

Web application for managing personal finances. Demo available at [financity.azurewebsites.net](https://financity.azurewebsites.net).

Account with example data: test@test.mail / Test@Test1

## Tech stack

- .Net 7 (with used libraries: Entity Framework Core 7, [AutoMapper](https://github.com/AutoMapper/AutoMapper), [Serilog](https://github.com/serilog/serilog), [MediatR](https://github.com/jbogard/MediatR), [Fluent Validation](https://github.com/FluentValidation/FluentValidation))
- Angular 15 (with used libraries: [Taiga UI](https://github.com/Tinkoff/taiga-ui))
- PostgreSQL 14 database
- Azure Web Services demo environment
- Github actions cd

## Database

Application was created partially as a project for "SQL and NoSQL databases management" classes at Poznan University of Technology, what lead to database first design. Database creation script is available [here](./api/Financity.Persistence/Migrations/Database.pgsql).
