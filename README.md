# EduPro - Clean Architecture API

Backend API for **EduPro**, an educational platform where users browse courses and collections, upload study files, and get AI-assisted answers and summaries about those files. Built as a graduation project, together with a teammate (Bayan Eid).

## Tech stack

- ASP.NET Core 8 Web API
- Clean Architecture (Api / Core / Data / Infrastructure / Service layers)
- CQRS with MediatR
- Entity Framework Core with SQL Server
- JWT authentication with refresh tokens, and Google sign-in
- Email (SMTP) for password reset
- Serilog logging (console and SQL Server)
- Swagger / OpenAPI

## Solution structure

| Project | Responsibility |
|---|---|
| `EduPro.Api` | Controllers, middleware, app startup |
| `EduPro.Core` | Features (commands, queries, handlers, validators), pipeline behaviors, mapping |
| `EdoPro.Data` | Entities, result models, route constants |
| `EduPro.Infrastructure` | Database context, repositories, service registration |
| `EduPro.Service` | Services such as authentication, email, and the AI service client |

## Features

- Authentication: register, sign in, refresh token, Google sign-in, password reset by email
- Authorization and role management
- Courses, categories and collections
- File upload and management (EduFile), with question answering and summaries through an external AI service
- Favorites, user profile, and statistics

## Running locally

1. Install the .NET 8 SDK and SQL Server.
2. Copy `EduPro.Api/appsettings.Example.json` to `EduPro.Api/appsettings.json` and fill in your own values (database connection, JWT secret, email credentials, Google client ID, AI service URL).
3. Create the database and apply EF Core migrations if the project includes them.
4. Run:

```
dotnet run --project EduPro.Api
```

## Notes

- Real secrets are not stored in this repository. `appsettings.json` and uploaded files are git-ignored.
- This is a clean snapshot of the project code. The original team repository is private.
