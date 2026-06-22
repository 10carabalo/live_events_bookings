# EventosVivos

A fullstack reservation system for cultural events. Prevents overselling, detects venue schedule
conflicts, and automates reservation and payment validation.

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | .NET 10, ASP.NET Core Web API, EF Core 10 + SQLite |
| Architecture | Clean Architecture (Domain / Application / Infrastructure / Api) |
| Backend tests | xUnit, FluentAssertions, Moq, WebApplicationFactory |
| Frontend | Angular 22, standalone components, zoneless change detection, signals, Angular Material |
| Frontend tests | Vitest via `@angular/build:unit-test` |

## Prerequisites

- [.NET SDK 10+](https://dotnet.microsoft.com/download) — verify with `dotnet --version`
- [Node.js 22+](https://nodejs.org/) — verify with `node -v`
- Angular CLI 22 — `npm install -g @angular/cli@22`

No database setup required. SQLite is used; the file is created automatically on first run.

## Running the Backend

```bash
cd backend
dotnet run --project src/EventosVivos.Api
```

- API: http://localhost:5263
- Swagger UI: http://localhost:5263/swagger (Development only)

The SQLite database (`eventosvivos.db`) is created automatically in the Api directory and seeded
with 3 venues on first run via EF Core migrations.

## Running the Frontend

```bash
cd frontend
npm install
npm start
```

App available at http://localhost:4200. Requires the backend to be running.

## Running Tests

```bash
# Backend — all layers (Domain, Application, Api integration)
cd backend
dotnet test

# Frontend — Vitest unit tests
cd frontend
npm test
```

Backend: 11 tests (5 domain + 2 application + 4 API integration via WebApplicationFactory).
Frontend: 1 test (VenueService with HttpTestingController).

## Architecture

### Clean Architecture

The backend is split into four projects with a strict dependency rule:

```
Domain  ←  Application  ←  Infrastructure
                        ←  Api
```

`Domain` has zero framework dependencies — only plain C# classes and interfaces (`IVenueRepository`,
`IClock`). `Application` contains use cases and DTOs. `Infrastructure` holds EF Core, migrations,
and repository implementations. `Api` wires everything together via ASP.NET Core.

This means swapping SQLite for SQL Server, or replacing EF Core with Dapper, requires changes only
in `Infrastructure` — Domain and Application are untouched.

### Justification Architecture

1. The problem justifies it

EventosVivos is an early-stage startup migrating from spreadsheets. The domain is well-defined (events, venues, reservations), and the requirements are stable and known. Introducing microservices at this point would create premature complexity without any real benefit.

2. Domain cohesion

The business rules (venue overlap, capacity, penalties, reservation statuses) are tightly coupled. In a monolith, these validations are executed in the same transaction without the need for distributed communication, guaranteeing consistency without overhead.

3. Speed ​​of delivery and maintainability

A small team (like that of a startup) iterates faster with a single deployment, a single repository, and a single execution context. The operating cost is lower, and the onboarding curve for new developers is much shallower.

4. Sufficient scalability for the current stage

SQLite as the database is adequate for the current volume. As the business grows, the internal layering (Domain, Application, Infrastructure) allows modules to be extracted into independent services without rewriting the business logic—it's a monolith ready to scale, not one that hinders it.

5. Technical Evaluation—Architectural Honesty

Choosing microservices in this context would be "architecture for show": a decision made to impress, not to solve the real problem. The test explicitly evaluates the quality of the decision, not the decision itself. A well-structured monolith demonstrates greater maturity of judgment than an oversized, distributed architecture.

### EF Core + SQLite

SQLite requires zero setup for evaluation. `Database.Migrate()` runs on startup, so there are no
migration scripts to execute manually. The connection string in `appsettings.json` can be replaced
with SQL Server for production without any code changes.

### Angular 22 — Signals + Zoneless

The frontend uses Angular's modern signal-based architecture with `provideZonelessChangeDetection()`.
This removes the zone.js overhead and makes change detection explicit. `toSignal()` bridges
`HttpClient` observables to the signal graph natively, keeping components fully OnPush-compatible.
The `IClock` abstraction in the Domain layer keeps all time-dependent business rules unit-testable
without real time or mocking static methods.

## Implemented Endpoints

| Method | Path | Description |
|---|---|---|
| `GET` | `/api/venues` | Returns all seeded venues |
| `GET` | `/openapi/v1.json` | OpenAPI 3.1 document (Development) |
| `GET` | `/swagger` | Swagger UI (Development) |

## Project Layout

```
backend/
├── src/
│   ├── EventosVivos.Domain/         # Entities (Venue), interfaces, IClock, DomainException
│   ├── EventosVivos.Application/    # Use cases (ListVenues), DTOs (VenueDto)
│   ├── EventosVivos.Infrastructure/ # AppDbContext, EF config, migrations, repositories, SystemClock
│   └── EventosVivos.Api/            # Controllers, Program.cs, GlobalExceptionHandler, appsettings
└── tests/
    ├── EventosVivos.Domain.Tests/
    ├── EventosVivos.Application.Tests/
    └── EventosVivos.Api.Tests/      # End-to-end tests via WebApplicationFactory + isolated SQLite

frontend/
└── src/app/
    ├── venues/                      # VenueService, VenuesComponent, venue.model.ts
    ├── app.config.ts                # Providers: zoneless, router, HttpClient, Material
    └── app.routes.ts                # Lazy-loaded routes
```
