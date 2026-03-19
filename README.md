# MediatR Console App

This repository contains a sample .NET console application demonstrating the use of MediatR, Entity Framework Core, and FluentValidation. The project implements a simple CRUD system for managing `Person` entities, including optimistic concurrency handling and transaction management.

## Features
- CQRS pattern with MediatR
- Entity Framework Core with SQLite
- Optimistic concurrency (manual versioning for SQLite)
- Transaction management via Unit of Work
- Validation with FluentValidation
- AutoMapper for DTO mapping

## Project Structure
- `Domain/` - Entity definitions
- `Data/` - DbContext and seeder
- `Repositories/` - Data access logic
- `Handlers/` - MediatR command/query handlers
- `UnitOfWork/` - Transaction management
- `Validators/` - FluentValidation validators
- `DTO/` - Data transfer objects

## Used Libraries
- [MediatR](https://github.com/jbogard/MediatR)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [FluentValidation](https://fluentvalidation.net/)
- [AutoMapper](https://automapper.org/)

## How to Run
1. Build the project: `dotnet build`
2. Apply migrations: `dotnet ef database update --project MediatRApp/MediatRApp.csproj`
3. Run: `dotnet run --project MediatRApp/MediatRApp.csproj`

## Notes
- The database file (`app.db`) and its auxiliary files are excluded from version control via `.gitignore`.
- Optimistic concurrency is implemented using an integer version field due to SQLite limitations.
