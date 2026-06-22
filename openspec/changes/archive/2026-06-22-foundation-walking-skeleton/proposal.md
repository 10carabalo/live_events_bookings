## Why

The repository is configured (docs, specs, agents, OpenSpec) for .NET 10 + Angular 22, but
`backend/` and `frontend/` are empty — there is no solution, no app, and no shared infrastructure.
No feature (RF-01…RF-06) can be implemented until the Clean Architecture skeleton and the
cross-cutting concerns every feature depends on (persistence, error contract, clock, API docs,
CORS) exist and are proven to work end-to-end. This change establishes that foundation as a
runnable "walking skeleton" and validates the full stack with the single lowest-risk, rule-free
capability available: listing the seeded reference venues.

## What Changes

- **NEW** .NET 10 solution `backend/EventosVivos.sln` with four Clean Architecture projects
  (`Domain`, `Application`, `Infrastructure`, `Api`) plus three xUnit test projects
  (`Domain.Tests`, `Application.Tests`, `Api.Tests`).
- **NEW** Angular 22 application shell under `frontend/` — standalone, zoneless
  (`provideZonelessChangeDetection()`), `provideHttpClient()`, Angular Material, Jest configured.
- **NEW** EF Core 10 `AppDbContext` + SQLite provider, `Venue` entity configuration, an initial
  migration, and `HasData` seed for the three fixed reference venues (Auditorio Central / Sala
  Norte / Arena Sur). Migration is applied on startup.
- **NEW** cross-cutting API behaviors: global RFC 7807 `ProblemDetails` exception middleware,
  built-in OpenAPI document + Swagger UI, and CORS configured for the Angular origin.
- **NEW** `IClock` / `SystemClock` abstraction (injectable; required for the time-based rules in
  later features) wired into DI.
- **NEW** `GET /api/venues` end-to-end: `Venue` domain entity + `IVenueRepository` →
  `VenueRepository` (EF) → `ListVenues` use case + `VenueDto` → `VenuesController`, consumed by an
  Angular `VenueService` and a minimal venues view.
- **NEW** first automated tests in every layer (domain/seed, application use case with a mocked
  repository, API integration via `WebApplicationFactory`, and an Angular service Jest test).
- Out of scope (deferred to later vertical-slice changes): the `Event` and `Reservation` entities,
  business rules RN-01…RN-07, and features RF-01…RF-06. Each arrives with its own entity and
  migration.

## Capabilities

### New Capabilities
- `venue-catalog`: Expose the read-only, seeded reference venues. Clients can retrieve the full
  list of venues (id, name, capacity, city) via `GET /api/venues`. Venues are never created,
  updated, or deleted through the API.
- `api-foundation`: Cross-cutting API contract for the whole service — errors are returned as
  RFC 7807 `ProblemDetails`, an OpenAPI document and Swagger UI are available, CORS permits the
  configured SPA origin, and the database schema + reference seed data are present on startup.

### Modified Capabilities
<!-- None — greenfield. openspec/specs/ is empty; no existing capability requirements change. -->

## Impact

- **Code**: creates the entire `backend/` solution and `frontend/` app from empty directories.
- **Database**: introduces `eventosvivos.db` (SQLite, gitignored), the initial schema, and the
  `Venue` seed. Establishes the migration baseline all later changes extend.
- **APIs**: first live endpoint `GET /api/venues`; establishes the global error contract, OpenAPI
  surface, and CORS policy that every later endpoint inherits.
- **Dependencies**: backend — EF Core 10 (+ SQLite provider), FluentValidation,
  `Microsoft.AspNetCore.OpenApi`, Swagger UI, xUnit + FluentAssertions + Moq. Frontend — Angular 22,
  Angular Material/CDK, Jest + testing utilities.
- **Tooling prerequisite**: .NET SDK 10 and Angular CLI 22 must be resolvable on PATH at apply
  time (`.NET 10.0.301` is installed but not currently on the shell PATH).
