# Task Tracker Mini API

A lightweight RESTful API built with **ASP.NET Core Minimal APIs and .NET 8**, designed to manage tasks while demonstrating database integration, logging, and automated testing.

## Tech Stack

- **Backend:** C#, ASP.NET Core Minimal APIs (.NET 8)
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core
- **Integration Testing:** xUnit, ASP.NET Core `WebApplicationFactory`, SQLite
- **Logging:** Microsoft.Extensions.Logging
- **API Documentation:** Swagger / OpenAPI

## Features

- CRUD operations for tasks
- Input validation and error handling
- Structured application logging
- PostgreSQL database integration
- Unit and integration tests
- Isolated test database using SQLite in-memory

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/tasks` | Get all tasks |
| GET | `/tasks/{id}` | Get a task by ID |
| POST | `/tasks` | Create a task |
| PUT | `/tasks/{id}` | Update a task |
| DELETE | `/tasks/{id}` | Delete a task |

## Getting Started

1. Clone the repository.
2. Configure the PostgreSQL connection string named `TaskTracker` in `appsettings.json`.
3. Apply the Entity Framework Core migrations.
4. Run the API using Visual Studio or the .NET CLI.
5. Open Swagger to explore and test the endpoints.

## Testing

Run the automated tests with:

```bash
dotnet test
```

The test suite covers task creation, validation, CRUD operations, HTTP responses, and error scenarios.

## Purpose

This project was built to strengthen practical knowledge of **Minimal APIs, Entity Framework Core, logging, dependency injection, and automated testing** in ASP.NET Core.
