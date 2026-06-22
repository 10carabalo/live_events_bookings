# Unit Test and DB Verification Report — 2026-06-22

## Test results

### Backend (`dotnet test`)

| Project               | Passed | Failed | Skipped |
|-----------------------|--------|--------|---------|
| Domain.Tests          | 5      | 0      | 0       |
| Application.Tests     | 2      | 0      | 0       |
| Api.Tests             | 4      | 0      | 0       |
| **Total**             | **11** | **0**  | **0**   |

### Frontend (`ng test --watch=false`)

| Test Files | Passed | Failed |
|------------|--------|--------|
| 1          | 1      | 0      |

## DB state

After `Database.Migrate()` on startup, the `eventosvivos.db` SQLite database contains 3 reference venues:

| id | name              | capacity | city     |
|----|-------------------|----------|----------|
| 1  | Auditorio Central | 200      | Bogotá   |
| 2  | Sala Norte        | 50       | Bogotá   |
| 3  | Arena Sur         | 500      | Medellín |

The integration tests use an isolated SQLite file per test run (`tmp/<guid>.db`), so the dev DB is never touched by tests.

## Notes

- Cleanup `IOException` on Windows (SQLite WAL lock) is now suppressed in `ApiWebApplicationFactory.Dispose` — all test assertions pass.
- Vitest is used for frontend tests (Angular 22 native runner — `jest-preset-angular` was not yet compatible with Angular 22 at apply time).
