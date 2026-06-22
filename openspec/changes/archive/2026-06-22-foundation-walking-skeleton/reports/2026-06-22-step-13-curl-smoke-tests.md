# Manual Endpoint Smoke Test Report — 2026-06-22

API started via: `dotnet run --project src/EventosVivos.Api --no-build --urls http://localhost:5263`

## Test 1: GET /api/venues

```
GET http://localhost:5263/api/venues
→ 200 OK

[
  { "id": 1, "name": "Auditorio Central", "capacity": 200, "city": "Bogotá" },
  { "id": 2, "name": "Sala Norte",        "capacity": 50,  "city": "Bogotá" },
  { "id": 3, "name": "Arena Sur",         "capacity": 500, "city": "Medellín" }
]
```

**Result:** PASS — 200, all 3 seeded venues with correct values.

## Test 2: Unknown route → 404

```
GET http://localhost:5263/api/does-not-exist
→ 404 Not Found
```

**Result:** PASS — 404 returned (ProblemDetails middleware active).

## Test 3: OpenAPI document

```
GET http://localhost:5263/openapi/v1.json
→ 200 OK
  openapi: 3.1.1
  title: EventosVivos.Api | v1
  paths: /api/venues (1 path)
```

**Result:** PASS — valid OpenAPI 3.1.1 document describing the venues endpoint.

## Swagger UI

Available at `http://localhost:5263/swagger` in Development environment (confirmed locally).

## Notes

- All read-only requests — no DB restoration needed.
- No HTTPS redirect triggered (running on HTTP profile for local smoke tests).
