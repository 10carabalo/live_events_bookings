## 0. Setup: Create Feature Branch (MANDATORY — FIRST STEP)

- [x] 0.1 Create and switch to feature branch `feature/foundation-walking-skeleton` from `main`
- [x] 0.2 Verify current branch and clean working tree (`git status`)

## 1. Tooling prerequisite check

- [x] 1.1 Ensure `dotnet` resolves on PATH (prepend `C:\Program Files\dotnet` for the session if needed); confirm `dotnet --version` is 10.x
- [x] 1.2 Confirm `ng version` (Angular CLI 22) and `node -v` (>= 20.19/22.12/24)
- [x] 1.3 Ensure EF tooling: `dotnet tool install --global dotnet-ef` (or confirm present)

## 2. Backend: solution & project scaffold

- [x] 2.1 Create `backend/EventosVivos.sln`
- [x] 2.2 Create `backend/src/EventosVivos.Domain` (classlib, net10.0, nullable enabled)
- [x] 2.3 Create `backend/src/EventosVivos.Application` (classlib) referencing Domain
- [x] 2.4 Create `backend/src/EventosVivos.Infrastructure` (classlib) referencing Application + Domain
- [x] 2.5 Create `backend/src/EventosVivos.Api` (webapi) referencing Application + Infrastructure
- [x] 2.6 Create test projects `backend/tests/EventosVivos.{Domain,Application,Api}.Tests` (xUnit + FluentAssertions + Moq) with references
- [x] 2.7 Add all projects to the solution; confirm `dotnet build` succeeds (empty skeleton)
- [x] 2.8 Add NuGet deps: EF Core 10 + `Microsoft.EntityFrameworkCore.Sqlite` (+ `.Design`), `Microsoft.AspNetCore.OpenApi`, `Swashbuckle.AspNetCore.SwaggerUI`, FluentValidation (Api/Application)

## 3. Backend: Domain (TDD)

- [x] 3.1 Write failing tests: `Venue` exposes `Id`/`Name`/`Capacity`/`City`; `IClock.UtcNow` contract
- [x] 3.2 Implement `Venue` entity (read-only reference shape) and `IClock` interface
- [x] 3.3 Add `IVenueRepository` (`Task<IReadOnlyList<Venue>> GetAllAsync(CancellationToken)`) and a base `DomainException`
- [x] 3.4 Run Domain tests green

## 4. Backend: Infrastructure (EF Core + SQLite)

- [x] 4.1 Implement `AppDbContext` with `DbSet<Venue>` and `NoTracking` default
- [x] 4.2 Add `VenueConfiguration : IEntityTypeConfiguration<Venue>` (keys, required, max lengths)
- [x] 4.3 Seed the 3 fixed venues via `HasData` (Auditorio Central 200/Bogotá, Sala Norte 50/Bogotá, Arena Sur 500/Medellín — per docs/data-model.md)
- [x] 4.4 Add initial EF migration `InitialCreate` (`dotnet ef migrations add InitialCreate -p src/EventosVivos.Infrastructure -s src/EventosVivos.Api`); commit migration files
- [x] 4.5 Implement `VenueRepository : IVenueRepository` and `SystemClock : IClock`
- [x] 4.6 Add `AddInfrastructure(IConfiguration)` DI extension (DbContext + repo + clock)

## 5. Backend: Application (TDD)

- [x] 5.1 Write failing test: `ListVenues` returns all venues mapped to `VenueDto` (mocked `IVenueRepository`)
- [x] 5.2 Implement `VenueDto`, `ListVenues` use case (manual mapping), and `AddApplication()` DI extension
- [x] 5.3 Run Application tests green

## 6. Backend: Api wiring

- [x] 6.1 Configure `Program.cs`: `AddProblemDetails()` + global `IExceptionHandler` (404/500 → RFC 7807, no internals leaked, log via `ILogger`)
- [x] 6.2 Add built-in OpenAPI (`AddOpenApi()`/`MapOpenApi()`) + Swagger UI at `/swagger` in Development
- [x] 6.3 Add named CORS policy from `Cors:AllowedOrigins` (default `http://localhost:4200`); apply globally
- [x] 6.4 Apply migrations on startup (`db.Database.Migrate()`); register Application + Infrastructure
- [x] 6.5 Implement `VenuesController` → `GET /api/venues` calling `ListVenues`; make `Program` public/partial for tests
- [x] 6.6 Set SQLite connection string in `appsettings.json`; ensure `*.db` is gitignored

## 7. Backend: Api integration tests (TDD)

- [x] 7.1 Add `WebApplicationFactory<Program>` fixture using a dedicated/isolated SQLite database (not the dev `eventosvivos.db`)
- [x] 7.2 Test `GET /api/venues` → 200 + 3 seeded venues with correct values (venue-catalog spec)
- [x] 7.3 Test unknown route → 404 `application/problem+json` (api-foundation spec)
- [x] 7.4 Test OpenAPI document endpoint → 200 valid document (api-foundation spec)
- [x] 7.5 Run Api tests green

## 8. Frontend: app scaffold

- [x] 8.1 `ng new` Angular 22 app in `frontend/` (standalone, routing, SCSS)
- [x] 8.2 Enable zoneless (`provideZonelessChangeDetection()`), `provideHttpClient()`, animations; remove `zone.js`
- [x] 8.3 Add Angular Material (+ CDK) and a base theme
- [x] 8.4 Configure Jest (`jest-preset-angular`); replace Karma; add `npm test` script; confirm an empty suite runs
- [x] 8.5 Add `environment` files with `apiUrl` pointing at the backend

## 9. Frontend: VenueService (TDD)

- [x] 9.1 Write failing Jest test: `VenueService.list()` GETs `${apiUrl}/api/venues` and returns typed venues (`HttpTestingController`)
- [x] 9.2 Implement `Venue` model and `VenueService` (typed `HttpClient`)
- [x] 9.3 Run the service test green

## 10. Frontend: venues view

- [x] 10.1 Create a standalone `VenuesComponent` (OnPush) rendering venues in a `MatTable`
- [x] 10.2 Consume `VenueService.list()` via `toSignal()`; render explicit loading / empty / error states
- [x] 10.3 Route the app shell to the venues view; verify it builds (`npm run build`)

## 11. Backend: Review and Update Existing Unit Tests (MANDATORY)

- [x] 11.1 Confirm no pre-existing tests need updating (greenfield); ensure new tests cover every scenario in `specs/venue-catalog` and `specs/api-foundation`

## 12. Run Unit Tests and Verify Database State (MANDATORY)

- [x] 12.1 Capture pre-test DB baseline (venue count = 3 after migrate/seed)
- [x] 12.2 Run targeted tests for changed layers (`dotnet test` per project; `npm test` for frontend)
- [x] 12.3 Run the full suites: `dotnet test` (solution) and `npm test`; record pass/fail/runtime
- [x] 12.4 Verify post-test DB state unchanged (reads only; integration tests use an isolated DB)
- [x] 12.5 Create report `openspec/changes/foundation-walking-skeleton/reports/YYYY-MM-DD-step-12-unit-test-and-db-verification.md`
- [x] 12.6 Mark complete only after suites pass and the report exists

## 13. Manual Endpoint Testing with curl (MANDATORY — AGENT MUST EXECUTE)

- [x] 13.1 Start the API (`dotnet run --project src/EventosVivos.Api`)
- [x] 13.2 `curl GET /api/venues` → verify 200 and the 3 seeded venues
- [x] 13.3 `curl` an unknown route → verify 404 `problem+json` shape
- [x] 13.4 `curl` the OpenAPI document endpoint → verify 200 valid document; confirm `/swagger` loads in a browser
- [x] 13.5 Document commands + responses in a report under the change `reports/` folder (all GET / read-only — no DB restoration needed)

## 14. Frontend: E2E Testing with Playwright MCP (MANDATORY if applicable — AGENT MUST EXECUTE)

- [x] 14.1 Start backend and frontend (`npm start`)
- [x] 14.2 Navigate to the venues view; snapshot the loaded state
- [x] 14.3 Verify the 3 seeded venues render from the live API; verify the error state renders when the API is down
- [x] 14.4 Document scenarios/outcomes in a report under the change `reports/` folder

## 15. Update Technical Documentation (MANDATORY)

- [x] 15.1 Update `README` / `docs/development_guide.md` if any setup step differs from what was documented
- [x] 15.2 Confirm `docs/api-spec.yml` matches the implemented `GET /api/venues`
- [x] 15.3 Run `openspec validate --all`; ensure the change validates before archiving
