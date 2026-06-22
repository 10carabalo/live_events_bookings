## Context

`backend/` and `frontend/` are empty. This change creates the entire solution scaffold plus the
cross-cutting infrastructure every later feature depends on, and proves it end-to-end with the
`venue-catalog` and `api-foundation` capabilities. Architecture and conventions are fixed by
`docs/backend-standards.md`, `docs/frontend-standards.md`, and `docs/data-model.md`; this document
records the concrete technical decisions for the skeleton. Tooling note: .NET SDK 10.0.301 is
installed but not on the shell PATH; Angular CLI 22 and Node 24 are available.

## Goals / Non-Goals

**Goals:**
- A buildable .NET 10 Clean Architecture solution and a runnable Angular 22 app.
- Shared infra reused by all later features: EF Core 10 + SQLite `AppDbContext`, initial migration,
  reference seed, global `ProblemDetails` error contract, `IClock`, CORS, OpenAPI + Swagger UI.
- One thin capability (`GET /api/venues`) wired through every layer, with a test in each layer.
- A test harness in every project so later features start test-first.

**Non-Goals:**
- No `Event`/`Reservation` entities, no business rules (RN-01…RN-07), no features RF-01…RF-06.
- No authentication/authorization (not in the assessment scope).
- No deployment/CI config (separate concern).

## Decisions

**Solution layout** — `backend/EventosVivos.sln`; source in `backend/src/EventosVivos.{Domain,
Application,Infrastructure,Api}`, tests in `backend/tests/EventosVivos.{Domain,Application,Api}.Tests`.
Project references enforce the dependency direction: `Api → Application → Domain`;
`Infrastructure → Application, Domain`; `Api → Infrastructure` (composition root only). Domain has
zero external references. Rationale: matches the documented layout and keeps the dependency rule
compiler-enforced.

**Venue identity & shape** — `Venue` is an `int`-keyed reference entity (`id, name, capacity,
city`) per `docs/data-model.md`. It is immutable from the app's perspective (read-only). No enums
needed yet. `Event`/`Reservation` (Guid-keyed) are intentionally deferred to their feature changes.

**Persistence** — EF Core 10 with `Microsoft.EntityFrameworkCore.Sqlite`. `AppDbContext` exposes
`DbSet<Venue>`; configuration via `IEntityTypeConfiguration<Venue>` (max lengths, required). Seed
the three fixed venues with `HasData` so they live in the migration. A single initial migration is
checked in. On startup the Api applies migrations (`db.Database.Migrate()`) so the API is usable
with no manual setup. `NoTracking` is the default query behavior (reads only in this change).

**Error contract** — use ASP.NET Core `AddProblemDetails()` plus a global `IExceptionHandler`
implementation that maps exceptions to RFC 7807 responses (404 for not-found, 500 generic for
unhandled) and logs full detail server-side via `ILogger<T>`. Rationale: `IExceptionHandler` is the
current idiomatic approach (cleaner than custom middleware) and integrates with `ProblemDetails`.

**API docs** — built-in `Microsoft.AspNetCore.OpenApi` (`AddOpenApi()` / `MapOpenApi()`) generates
the document; **Swagger UI** (`Swashbuckle.AspNetCore.SwaggerUI`) renders it at `/swagger` in
Development. Rationale: built-in OpenAPI is the .NET template default; Swagger UI matches the path
already referenced in the docs. (Scalar considered; Swagger UI chosen for familiarity.)

**CORS** — a named policy reads the allowed Angular origin from configuration
(`Cors:AllowedOrigins`), applied globally. Default dev origin `http://localhost:4200`.

**IClock** — `IClock` (Domain) + `SystemClock` (Infrastructure) registered in DI now. It has no
consumer in this change but is pure, behavior-free foundation that every time-based rule
(RN-03/04/06/07) will inject; introducing it here avoids an infrastructure-only change later. This
is the one deliberate exception to "no code ahead of its feature," justified by it being a
zero-logic seam.

**Application layer** — `ListVenues` use case returns `IReadOnlyList<VenueDto>`; manual mapping
(no AutoMapper). `IVenueRepository` (Domain) with `VenueRepository` (Infrastructure). No UnitOfWork
(read-only).

**Frontend** — `ng new` standalone app, zoneless via `provideZonelessChangeDetection()`,
`provideHttpClient()`, Angular Material + a theme. `VenueService` (typed `HttpClient`) exposes
`list(): Observable<Venue[]>`; the venues component renders them via a Material table, consuming the
observable with `toSignal()` (with explicit loading/empty/error states). `resource()`/
`httpResource()` is noted as the signal-native alternative but `toSignal()` keeps the first slice
minimal. Testing uses **Jest** via `jest-preset-angular`; `VenueService` is tested with
`HttpTestingController`.

**Integration tests** — `WebApplicationFactory<Program>` with a dedicated SQLite database
(temp file or in-memory connection) so tests never touch the dev `eventosvivos.db`. Requires
`Program` to be partial/public for the test project.

## Risks / Trade-offs

- **Jest + Angular 22 compatibility** (jest-preset-angular may trail a brand-new major) → if the
  preset is incompatible at apply time, pin the latest compatible versions or fall back to Angular's
  supported Vitest runner; revisit `docs/frontend-standards.md` if the fallback is needed.
- **`dotnet` not on PATH** → blocks build/test/ef at apply time → prepend
  `C:\Program Files\dotnet` to PATH for the apply session (or add permanently) before scaffolding.
- **`Database.Migrate()` on startup** assumes a single instance and trusted migrations → acceptable
  for a local/demo assessment; revisit if deploying multi-instance.
- **Introducing `IClock` with no consumer** → minor "unused seam" now → justified above; covered by
  a trivial registration/resolution check rather than feature tests.
- **Seeding via `HasData`** bakes seed into the migration (vs. a runtime seeder) → simplest and
  deterministic for fixed reference data; fine because venues never change.

## Migration Plan

Greenfield — no production data or rollback concerns. The SQLite file is gitignored and
recreated by `Database.Migrate()`; resetting state is "delete `eventosvivos.db` and re-run." The
initial EF migration is the baseline that every later feature change extends with its own
migration.

## Open Questions

- Confirm `jest-preset-angular` supports Angular 22 at apply time (else use the documented
  fallback).
- Whether to add a `/health` endpoint now or defer — leaning defer (not required by any capability
  in this change).
